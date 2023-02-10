using System;
using System.Drawing;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ChaiCooking.Services
{
    public interface ILocalMediaStorageManager
    {
        Task<string> getPathForMediaAsync(FileResult media);
        Task<string> getPathForImageAsync(FileResult media);
        Task<Size> getSizeForMedia(FileResult media);
        Task<String> getOrientationForMedia(FileResult media);
        Task<string> trimIfNeeded(FileResult media);
    }
}

