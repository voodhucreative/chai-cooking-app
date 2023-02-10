using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Provider;
using ChaiCooking.Services;
using Java.IO;
using Xamarin.Essentials;
using System.Drawing;
using GOALD.Droid.Services;

[assembly: Xamarin.Forms.Dependency(typeof(LocalMediaStorageManager))]
namespace GOALD.Droid.Services
{
    public class LocalMediaStorageManager : ILocalMediaStorageManager
    {
        private Context context = Application.Context;
        public async Task<string> getPathForMediaAsync(FileResult media)
        {
            var mediaPath = SaveMediaToDCIM(media, 1, "video/mp4");
            if (needsTrimming(media.FullPath))
            {
                throw new Exception("Selected Video Exceeds 30 seconds, Please Open The Gallery App and Trim it Down");
            }

            await Task.Delay(10);
            return mediaPath;
        }

        private string SaveMediaToDCIM(FileResult media, int mediaType, string mimeIdentifier)
        {
            //return media.FullPath;

            // commenting this out for now as it seems to break things when recording a video
            // we'll need to test it still works on older devices before binning it though...

            File originalFile = new File(media.FullPath);
            var contentValues = new ContentValues();
            var contentResolver = context.ContentResolver;
            contentValues.Put(MediaStore.Video.Media.InterfaceConsts.Title, media.FileName);
            contentValues.Put(MediaStore.Video.Media.InterfaceConsts.DisplayName, media.FileName);
            contentValues.Put(MediaStore.MediaColumns.MimeType, mimeIdentifier);
            contentValues.Put(MediaStore.Video.Media.InterfaceConsts.DateAdded, DateTime.Now.Millisecond / 1000);
            Android.Net.Uri externalContentUri = MediaStore.Video.Media.ExternalContentUri;
            if (mediaType == 0)
                externalContentUri = MediaStore.Images.Media.ExternalContentUri;
            else if (mediaType == 1)
                externalContentUri = MediaStore.Video.Media.ExternalContentUri;
            else
                externalContentUri = MediaStore.Audio.Media.ExternalContentUri;

            if (Android.OS.Build.VERSION.SdkInt >= BuildVersionCodes.Q)
            {
                contentValues.Put(MediaStore.Video.Media.InterfaceConsts.RelativePath, Android.OS.Environment.DirectoryDcim);
                contentValues.Put(MediaStore.Video.Media.InterfaceConsts.DateAdded, DateTime.Now.Millisecond / 1000);
                contentValues.Put(MediaStore.Video.Media.InterfaceConsts.IsPending, true);

                Android.Net.Uri contentUri = context.ContentResolver.Insert(externalContentUri, contentValues);
                if (contentUri != null)
                {
                    try
                    {
                        if (WriteFileToStream(originalFile, context.ContentResolver.OpenOutputStream(contentUri)))
                        {
                            contentValues.Put(MediaStore.MediaColumns.IsPending, false);
                            context.ContentResolver.Update(contentUri, contentValues, null, null);
                        }

                        //return contentUri.ToString();
                    }
                    catch (Exception e)
                    {
                        context.ContentResolver.Delete(contentUri, null, null);
                        throw e;
                    }
                }
            }
            else
            {
                var filePath = media.FullPath;
                int pathSeparator = filePath.LastIndexOf('/');
                int extensionSeparator = filePath.LastIndexOf('.');
                String filename = pathSeparator >= 0 ? filePath.Substring(pathSeparator + 1) : filePath;
                String extension = extensionSeparator >= 0 ? filePath.Substring(extensionSeparator + 1) : "";
                File directory = new File(context.GetExternalFilesDir(Android.OS.Environment.DirectoryDcim).AbsolutePath);
                directory.Mkdirs();

                File file;
                int fileIndex = 1;
                String filenameWithoutExtension = extension.Length > 0 ? filename.Substring(0, filename.Length - extension.Length - 1) : filename;
                String newFilename = filename;
                do
                {
                    file = new File(directory, newFilename);
                    newFilename = filenameWithoutExtension + fileIndex++;
                    if (extension.Length > 0)
                        newFilename += "." + extension;
                } while (file.Exists());

                try
                {
                    if (WriteFileToStream(originalFile, new FileOutputStream(file)))
                    {
                        contentValues.Put(MediaStore.Video.Media.InterfaceConsts.Data, file.AbsolutePath);
                        context.ContentResolver.Insert(externalContentUri, contentValues);

                        // Refresh the Gallery
                        Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                        mediaScanIntent.SetData(Android.Net.Uri.FromFile(file));
                        context.SendBroadcast(mediaScanIntent);
                    }

                    //return file.AbsolutePath;
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }

            return media.FullPath;

        }

        private static bool WriteFileToStream(File file, System.IO.Stream outputStream)
        {
            try
            {
                InputStream inputStream = new FileInputStream(file);
                try
                {
                    byte[] buf = new byte[1024];
                    int len;
                    while ((len = inputStream.Read(buf)) > 0)
                        outputStream.Write(buf, 0, len);
                }
                finally
                {
                    try
                    {
                        inputStream.Close();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                try
                {
                    outputStream.Close();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return true;
        }

        private static bool WriteFileToStream(File file, Java.IO.FileOutputStream outputStream)
        {
            try
            {
                InputStream inputStream = new FileInputStream(file);
                try
                {
                    byte[] buf = new byte[1024];
                    int len;
                    while ((len = inputStream.Read(buf)) > 0)
                        outputStream.Write(buf, 0, len);
                }
                finally
                {
                    try
                    {
                        inputStream.Close();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                try
                {
                    outputStream.Close();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return true;
        }

        public bool needsTrimming(string path)
        {
            var retriever = new MediaMetadataRetriever();
            retriever.SetDataSource(path);
            var length = retriever.ExtractMetadata(MetadataKey.Duration);
            var lengthseconds = Convert.ToInt32(length) / 1000;
            return lengthseconds > 120;
        }

        public async Task<string> trimIfNeeded(FileResult media)
        {
            if (needsTrimming(media.FullPath))
            {
                throw new Exception("Selected Video Exceeds 30 seconds, Please Trim it Down");
            }
            else
            {
                await Task.Delay(10);
                var newFile = System.IO.Path.Combine(Xamarin.Essentials.FileSystem.CacheDirectory, media.FileName);
                using (var stream = await media.OpenReadAsync())
                using (var newStream = System.IO.File.OpenWrite(newFile))
                    await stream.CopyToAsync(newStream);
                return media.FullPath;
            }
        }

        public async Task<string> getPathForImageAsync(FileResult media)
        {
            await Task.Delay(10);
            return SaveMediaToDCIM(media, 0, "image/jpg");
        }

        public async Task<Size> getSizeForMedia(FileResult media)
        {
            await Task.Delay(10);
            var metaDataRetriever = new MediaMetadataRetriever();
            metaDataRetriever.SetDataSource(media.FullPath);
            Android.Graphics.Bitmap frame = metaDataRetriever.GetFrameAtTime(0);
            var size = new Size(frame.Width, frame.Height);
            return size;
        }

        public async Task<string> getOrientationForMedia(FileResult media)
        {
            await Task.Delay(10);
            var metaDataRetriever = new MediaMetadataRetriever();
            metaDataRetriever.SetDataSource(media.FullPath);
            Android.Graphics.Bitmap frame = metaDataRetriever.GetFrameAtTime(0);

            return (frame.Width > frame.Height) ? "landscape" : "portrait";
        }
    }
}
