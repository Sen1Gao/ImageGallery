using BasicArgs;
using Frontend.Interfaces;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace Frontend.Services
{
    public class HttpCommunication : IHttpCommunication,IDisposable
    {
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions option;
        private bool disposedValue;

        public HttpCommunication()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:5027/");

            option = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
        }

        public async Task<bool> VerifyAsync(SigninInfoArgs args)
        {
            var content = new StringContent(JsonSerializer.Serialize(args), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/signin/verify", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var temp = JsonSerializer.Deserialize<bool>(result, option);
                return temp;
            }
            return false;
        }

        public async Task<bool> RegisterAsync(SigninInfoArgs args)
        {
            var content = new StringContent(JsonSerializer.Serialize(args), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/signin/register", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var temp = JsonSerializer.Deserialize<bool>(result, option);
                return temp;
            }
            return false;
        }

        public async Task<bool> UploadImageAsync(string filePath, string description, string tag)
        {
            using(var formData=new MultipartFormDataContent())
            {
                var fileContent=new ByteArrayContent(await File.ReadAllBytesAsync(filePath));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                formData.Add(fileContent, "file","image.jpg");
                formData.Add(new StringContent(description), "description");
                formData.Add(new StringContent(tag), "tag");
                var response = await httpClient.PostAsync("api/image/upload", formData);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var temp = JsonSerializer.Deserialize<bool>(result, option);
                    return temp;
                }
                return false;

            }
        }

        public async Task<List<ImageCardInfo>> GetImageCardInfosAsync(int page, int pageSize)
        {
            var response = await httpClient.GetAsync($"api/image/getImageInfo?page={page}&pageSize={pageSize}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var temp = JsonSerializer.Deserialize<List<ImageCardInfo>>(result, option);
                if (temp != null)
                {
                    return temp;
                }
            }
            return new List<ImageCardInfo>();
        }

        public async Task<BitmapImage> GetImageAsync(string imageName)
        {
            var response = await httpClient.GetAsync($"api/image/getImage?imageName={imageName}");
            if (response.IsSuccessStatusCode)
            {
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                    bitmap.Freeze();
                    return bitmap;
                }
            }
            return null;
        }

        public async Task<List<ReviewInfo>> GetReviewInfosAsync(int imageID)
        {
            var response = await httpClient.GetAsync($"api/image/getReviews?imageID={imageID}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var temp = JsonSerializer.Deserialize<List<ReviewInfo>>(result, option);
                if (temp != null)
                {
                    return temp;
                }
            }
            return new List<ReviewInfo>();
        }

        public async Task<bool> UploadReviewAsync(ReviewInfo reviewInfo)
        {
            var content = new StringContent(JsonSerializer.Serialize(reviewInfo), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("api/image/uploadReview", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var temp = JsonSerializer.Deserialize<bool>(result, option);
                return temp;
            }
            return false;
        }

        public async Task<bool> DeleteImageAsync(int imageID,string imageName)
        {
            var response = await httpClient.DeleteAsync($"api/image/deleteImage/{imageID}/{imageName}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var temp = JsonSerializer.Deserialize<bool>(result, option);
                return temp;
            }
            return false;
        }

        public async Task<bool> UpdateImageInfoAsync(ImageCardInfo imageCardInfo)
        {
            var content = new StringContent(JsonSerializer.Serialize(imageCardInfo), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync("api/image/update", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var temp = JsonSerializer.Deserialize<bool>(result, option);
                return temp;
            }
            return false;
        }

        public async Task<List<ImageCardInfo>> GetImagesBySearchAsync(string tag)
        {
            var response = await httpClient.GetAsync($"api/image/search?tag={tag}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                var temp = JsonSerializer.Deserialize<List<ImageCardInfo>>(result, option);
                if (temp != null)
                {
                    return temp;
                }
            }
            return new List<ImageCardInfo>();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }
                httpClient.Dispose();
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~HttpCommunication()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

    }
}
