using System;
using System.IO;
using System.Threading.Tasks;
using AssetsLibrary;
using Foundation;
using ChaiCooking.iOS.Services;
using ChaiCooking.Services;
using Photos;
using Xamarin.Essentials;
using Xamarin.Forms;
using UIKit;
using System.Linq;
using AVFoundation;
using CoreGraphics;
using ImageIO;

[assembly: Dependency(typeof(LocalMediaStorageManager))]
namespace ChaiCooking.iOS.Services
{
    public class LocalMediaStorageManager : UIVideoEditorControllerDelegate, ILocalMediaStorageManager
    {
        NSUrl urlToSave;

        public async Task<string> getPathForMediaAsync(FileResult media)
        {
            string path;
            if (needsTrimming(media.FullPath))
            {
                await SaveMovieToCameraRollAsync(NSUrl.FromString(media.FullPath));
                path = await showVideoEditorController(media);
            }
            else
            {
                path = media.FullPath;
            }

            await SaveMovieToCameraRollAsync(NSUrl.FromString(path));
            return path;
        }

        public async Task<string> getPathForImageAsync(FileResult media)
        {
            var newFile = Path.Combine(FileSystem.CacheDirectory, media.FileName);
            using (var stream = await media.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);
            var bytes = await File.ReadAllBytesAsync(newFile);
            var imageData = new UIImage(NSData.FromArray(bytes));
            var image = imageData.AsJPEG();
            var orientation = RotateImage(imageData, 100, "jpg");
            orientation.SaveToPhotosAlbum((savedImage, error) =>
            {
                if (error != null)
                {
                    throw new Exception(error.LocalizedDescription);
                }
            });

            var finalImageName = Path.GetFileNameWithoutExtension(media.FileName) + ".jpg";
            var memoryStream = new MemoryStream();
            orientation.AsJPEG().AsStream().CopyTo(memoryStream);
            Stream finalImageData = orientation.AsJPEG().AsStream();
            var finalImageFile = Path.Combine(FileSystem.CacheDirectory, finalImageName);
            using (var newStream = File.OpenWrite(finalImageFile))
                await finalImageData.CopyToAsync(newStream);
            return finalImageFile;
        }

        async Task SaveMovieToCameraRollAsync(NSUrl url)
        {
            this.urlToSave = url;
            if (PHPhotoLibrary.AuthorizationStatus == PHAuthorizationStatus.Authorized)
            {
                PHPhotoLibrary.SharedPhotoLibrary.PerformChanges(SaveVideo, null);
            }
            else
            {
                var status = await PHPhotoLibrary.RequestAuthorizationAsync();
                if (status == PHAuthorizationStatus.Authorized)
                {
                    PHPhotoLibrary.SharedPhotoLibrary.PerformChanges(SaveVideo, null);
                }
            }
        }

        private void SavePhoto()
        {
            if (urlToSave != null)
            {
                PHAssetChangeRequest.FromImage(this.urlToSave);
            }
        }

        private void SaveVideo()
        {
            if (urlToSave != null)
            {
                PHAssetChangeRequest.FromVideo(this.urlToSave);
            }
        }

        private async Task<string> showVideoEditorController(FileResult media)
        {
            try
            {
                var editController = new UIVideoEditorController();
                var canEdit = UIVideoEditorController.CanEditVideoAtPath(media.FullPath);
                editController.VideoPath = media.FullPath;
                editController.VideoMaximumDuration = 30;
                var vc = GetCurrentViewController(true);
                var tcs = new TaskCompletionSource<string>(editController);
                editController.Delegate = new EditVideoDelegate
                {
                    CompletedHandler = path =>
                    {
                        try
                        {
                            tcs.TrySetResult(path);
                        }
                        catch (Exception e)
                        {
                            tcs.TrySetException(e);
                        }
                    }
                };
                editController.ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen;
                await vc.PresentViewControllerAsync(editController, true);
                var result = await tcs.Task;
                await vc.DismissViewControllerAsync(true);
                editController.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static UIImage RotateImage(UIImage image, int compressionQuality, string pathExtension)
        {
            UIGraphics.BeginImageContext(new CGSize(image.Size.Height, image.Size.Width));
            var bitmap = UIGraphics.GetCurrentContext();
            bitmap.TranslateCTM(image.Size.Height / 2, image.Size.Width / 2);
            bitmap.RotateCTM((nfloat)(Math.PI / 2));
            bitmap.ScaleCTM((nfloat)1.0, (nfloat)(-1.0));
            var origin = new CGPoint(-(image.Size.Width / 2), -(image.Size.Height / 2));
            bitmap.DrawImage(new CGRect(origin, image.Size), image.CGImage);
            var newImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            return newImage;
        }

        internal static UIViewController GetCurrentViewController(bool throwIfNull = true)
        {
            UIViewController viewController = null;

            var window = UIApplication.SharedApplication.KeyWindow;

            if (window != null && window.WindowLevel == UIWindowLevel.Normal)
                viewController = window.RootViewController;

            if (viewController == null)
            {
                window = UIApplication.SharedApplication
                    .Windows
                    .OrderByDescending(w => w.WindowLevel)
                    .FirstOrDefault(w => w.RootViewController != null && w.WindowLevel == UIWindowLevel.Normal);

                if (window == null && throwIfNull)
                    throw new InvalidOperationException("Could not find current view controller.");
                else
                    viewController = window?.RootViewController;
            }

            while (viewController?.PresentedViewController != null)
                viewController = viewController.PresentedViewController;

            if (throwIfNull && viewController == null)
                throw new InvalidOperationException("Could not find current view controller.");

            return viewController;
        }

        public bool needsTrimming(string path)
        {
            var url = NSUrl.FromFilename(path);
            AVUrlAsset avasset = new AVUrlAsset(url: url);
            var length = avasset.Duration.Seconds;
            return length > 120;
        }

        public async Task<string> trimIfNeeded(FileResult media)
        {
            if (needsTrimming(media.FullPath))
            {
                return await showVideoEditorController(media);
            }
            else
            {
                await Task.Delay(10);
                // save the file into local storage
                var newFile = Path.Combine(FileSystem.CacheDirectory, media.FileName);
                using (var stream = await media.OpenReadAsync())
                using (var newStream = File.OpenWrite(newFile))
                    await stream.CopyToAsync(newStream);
                return newFile;
            }
        }

        public async Task<System.Drawing.Size> getSizeForMedia(FileResult media)
        {
            await Task.Delay(10);
            var url = NSUrl.FromFilename(media.FullPath);
            AVUrlAsset avasset = new AVUrlAsset(url: url);
            var height = avasset.NaturalSize.Height;
            var width = avasset.NaturalSize.Width;
            return new System.Drawing.Size((int)height, (int)width);
        }

        public async Task<string> getOrientationForMedia(FileResult media)
        {
            await Task.Delay(10);
            var url = NSUrl.FromFilename(media.FullPath);
            AVUrlAsset avasset = new AVUrlAsset(url: url);
            CGSize size = avasset.NaturalSize;
            return (size.Width > size.Height) ? "landscape" : "portrait";

        }
    }

    class EditVideoDelegate : UIVideoEditorControllerDelegate
    {
        public Action<string> CompletedHandler { get; set; }

        public override void VideoSaved(UIVideoEditorController editor, string editedVideoPath)
        {
            CompletedHandler?.Invoke(editedVideoPath);
        }

        public override void UserCancelled(UIVideoEditorController editor)
        {
            CompletedHandler?.Invoke(null);
        }

        public override void Failed(UIVideoEditorController editor, NSError error)
        {
            CompletedHandler?.Invoke(null);
        }
    }
}

