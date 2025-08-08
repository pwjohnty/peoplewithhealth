using Azure.Storage.Blobs;
using CommunityToolkit.Mvvm.Messaging;
using Mopups.Services;
using SkiaSharp;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Devices;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Maui.Devices;

namespace PeopleWith;

public partial class TakeProfilePicture : ContentPage
{
    CrashDetected crashHandler = new CrashDetected();
    private static readonly HttpClient Client = new HttpClient();
    private byte[] ResizedImage;
    string ImageFileName = string.Empty;
    bool ExisitingPhoto = false;
    bool Rotate = false; 

    //Change the following 
    private const string StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=peoplewithappiamges;AccountKey=9maBMGnjWp6KfOnOuXWHqveV4LPKyOnlCgtkiKQOeA+d+cr/trKApvPTdQ+piyQJlicOE6dpeAWA56uD39YJhg==;EndpointSuffix=core.windows.net";
    private const string Container = "profileuploads";
    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.SentryCrashDetected(Ex);
            await Navigation.PushAsync(new ErrorPage("Dashboard"), false);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }

    private void ConfigureClient()
    {
        try
        {
            if (!Client.DefaultRequestHeaders.Contains("X-MS-CLIENT-PRINCIPAL"))
            {
                Client.DefaultRequestHeaders.Add("X-MS-CLIENT-PRINCIPAL", "eyAgCiAgImlkZW50aXR5UHJvdmlkZXIiOiAidGVzdCIsCiAgInVzZXJJZCI6ICIxMjM0NSIsCiAgInVzZXJEZXRhaWxzIjogImpvaG5AY29udG9zby5jb20iLAogICJ1c2VyUm9sZXMiOiBbIjFFMzNDMEFDLTMzOTMtNEMzNC04MzRBLURFNUZEQkNCQjNDQyJdCn0=");
                Client.DefaultRequestHeaders.Add("X-MS-API-ROLE", "1E33C0AC-3393-4C34-834A-DE5FDBCBB3CC");
            }
        }
        catch (Exception Ex)
        {
            //Empty
        }
    }
    public TakeProfilePicture()
    {
        InitializeComponent();
    }

    public TakeProfilePicture(string Imagepassed )
    {
        InitializeComponent();
        string Passed = Imagepassed;
        var imagestring = $"https://peoplewithappiamges.blob.core.windows.net/profileuploads/{Passed}?t={DateTime.UtcNow.Ticks}";
        ProfilePic.Source = ImageSource.FromUri(new Uri(imagestring));
        TakePhotobtn.Text = "Retake Photo";
        ImageFileName = Passed;
        ExisitingPhoto = true;
        Deletebtn.IsVisible = true;
    }


    public async Task<FileResult> CapturePhotoAsync()
    {
        try
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();

                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Permission Required", "Camera access is required to take a photo", "Close");
                    return null;
                }
            }

            if (MediaPicker.Default.IsCaptureSupported)
            {
                return await MediaPicker.Default.CapturePhotoAsync();
            }

            return null;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
            return null;
        }
    }

    public async Task<FileResult> PickPhotoAsync()
    {
        try
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Photos>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Photos>();

                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Permission Required", "Photo library access is required to upload an image", "Close");
                    return null;
                }
            }


            return await MediaPicker.Default.PickPhotoAsync();

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
            return null;
        }
    }




    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new PrivacyPolicy(), false);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void TakePhoto_Clicked(object sender, EventArgs e)
    {
        try
        {
            var photo = await CapturePhotoAsync();
            if (photo != null)
            {
                using var stream = await photo.OpenReadAsync();
                Rotate = true; 
                var resizedImage = await ResizeImageAsync(stream, 1024, 1024, 80);
                Rotate = false;
                if (resizedImage != null)
                {
                    ResizedImage = resizedImage;
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        ProfilePic.Source = ImageSource.FromStream(() => new MemoryStream(resizedImage));
                    });
                    Savebtn.IsVisible = true;
                    Deletebtn.IsVisible = false;
                    TakePhotobtn.Text = "Retake Photo";

                    var backrandom = new Random();
                    var backrandomnum = backrandom.Next(1000, 10000000);
                    var Filename = Helpers.Settings.UserKey + "-" + DateTime.Now.ToString("dd-MM-yyyy-ss-mm-HH") + "-" + backrandomnum + ".Jpeg";
                    if (string.IsNullOrEmpty(ImageFileName))
                    {
                        //New Instance
                        ImageFileName = Filename;
                    }

                    await Task.Delay(2000);
                    //ProfilePic.Source = ImageSource.FromStream(() => new MemoryStream(resizedImage));
                    //Update FIleName
                  
                 
                    //ProfilePic.Rotation = 90;
                   
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    //private async Task<byte[]> ResizeImageAsync(Stream imageStream, int maxWidth, int maxHeight, int quality)
    // {
    //    try
    //  {
    //using var ms = new MemoryStream();
    //await imageStream.CopyToAsync(ms);

    //using var original = SKBitmap.Decode(ms.ToArray());

    //float scale = Math.Min((float)maxWidth / original.Width, (float)maxHeight / original.Height);
    //int newWidth = (int)(original.Width * scale);
    //int newHeight = (int)(original.Height * scale);

    //using var resized = original.Resize(new SKImageInfo(newWidth, newHeight),
    //                                    new SKSamplingOptions(SKFilterMode.Linear));

    //// Rotate 90 degrees
    //using var rotated = new SKBitmap(resized.Height, resized.Width);
    //using (var canvas = new SKCanvas(rotated))
    //{
    //    //Android only fix 
    //    if (Rotate && DeviceInfo.Current.Platform == DevicePlatform.Android)
    //    {
    //        canvas.Translate(rotated.Width / 2, rotated.Height / 2);
    //        canvas.RotateDegrees(90);
    //        canvas.Translate(-resized.Width / 2, -resized.Height / 2);
    //        canvas.DrawBitmap(resized, 0, 0);
    //    }
    //    else
    //    {
    //        canvas.DrawBitmap(resized, 0, 0);
    //    }
    //}

    //using var image = SKImage.FromBitmap(rotated);
    //return image.Encode(SKEncodedImageFormat.Png, quality).ToArray();



    //using var ms = new MemoryStream();
    //await imageStream.CopyToAsync(ms);

    //using var original = SKBitmap.Decode(ms.ToArray());
    //if (original == null) return null;

    //float scale = Math.Min((float)maxWidth / original.Width, (float)maxHeight / original.Height);
    //int newWidth = (int)(original.Width * scale);
    //int newHeight = (int)(original.Height * scale);

    //using var resized = original.Resize(new SKImageInfo(newWidth, newHeight),
    //                                    new SKSamplingOptions(SKFilterMode.Linear));

    //using var image = SKImage.FromBitmap(resized);
    //return image.Encode(SKEncodedImageFormat.Png, quality).ToArray();

    //  }
    //   catch (Exception ex)
    //  {
    //       NotasyncMethod(ex);
    //       return null;
    //   }
    //  }

    public async Task<byte[]> ResizeImageAsync(Stream imageStream, int maxWidth, int maxHeight, int quality)
    {
        try
        {
            using var ms = new MemoryStream();
            await imageStream.CopyToAsync(ms);
            var imageData = ms.ToArray();

            using var codecStream = new MemoryStream(imageData);
            using var codec = SKCodec.Create(codecStream);
            if (codec == null)
                return null;

            var orientation = codec.EncodedOrigin;

            using var original = SKBitmap.Decode(codec);
            if (original == null)
                return null;

            float scale = Math.Min((float)maxWidth / original.Width, (float)maxHeight / original.Height);
            int newWidth = (int)(original.Width * scale);
            int newHeight = (int)(original.Height * scale);

            using var resized = original.Resize(new SKImageInfo(newWidth, newHeight), new SKSamplingOptions(SKFilterMode.Linear));
            if (resized == null)
                return null;

            using var corrected = ApplyExifOrientation(resized, orientation);

            using var image = SKImage.FromBitmap(corrected);
            using var encodedData = image.Encode(SKEncodedImageFormat.Png, quality);
            return encodedData.ToArray();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error resizing image: {ex.Message}");
            return null;
        }
    }

    private SKBitmap ApplyExifOrientation(SKBitmap bitmap, SKEncodedOrigin orientation)
    {
        int width = bitmap.Width;
        int height = bitmap.Height;

        // Swap dimensions for 90 or 270 rotation
        bool rotate90 = orientation == SKEncodedOrigin.RightTop ||
                        orientation == SKEncodedOrigin.LeftBottom ||
                        orientation == SKEncodedOrigin.RightBottom ||
                        orientation == SKEncodedOrigin.LeftTop;

        var correctedBitmap = new SKBitmap(
            rotate90 ? height : width,
            rotate90 ? width : height
        );

        using var canvas = new SKCanvas(correctedBitmap);
        switch (orientation)
        {
            case SKEncodedOrigin.RightTop:
                canvas.Translate(height, 0);
                canvas.RotateDegrees(90);
                break;
            case SKEncodedOrigin.BottomRight:
                canvas.Translate(width, height);
                canvas.RotateDegrees(180);
                break;
            case SKEncodedOrigin.LeftBottom:
                canvas.Translate(0, width);
                canvas.RotateDegrees(270);
                break;
            case SKEncodedOrigin.TopRight:
                canvas.Scale(-1, 1);
                canvas.Translate(-width, 0);
                break;
            case SKEncodedOrigin.LeftTop:
                canvas.Translate(height, 0);
                canvas.RotateDegrees(90);
                canvas.Scale(-1, 1);
                canvas.Translate(-height, 0);
                break;
            case SKEncodedOrigin.BottomLeft:
                canvas.Translate(width, height);
                canvas.RotateDegrees(180);
                canvas.Scale(-1, 1);
                canvas.Translate(-width, 0);
                break;
            case SKEncodedOrigin.RightBottom:
                canvas.Translate(0, width);
                canvas.RotateDegrees(270);
                canvas.Scale(-1, 1);
                canvas.Translate(-width, 0);
                break;
            default:
                // TopLeft (no rotation needed)
                break;
        }

        canvas.DrawBitmap(bitmap, 0, 0);
        return correctedBitmap;
    }




    //private async Task<byte[]> ResizeImageAsync(Stream imageStream, int maxWidth, int maxHeight, int quality)
    //{
    //    try
    //    {
    //        using var memoryStream = new MemoryStream();
    //        await imageStream.CopyToAsync(memoryStream);
    //        byte[] imageData = memoryStream.ToArray();

    //        using var original = SKBitmap.Decode(imageData);

    //        float scale = Math.Min((float)maxWidth / original.Width, (float)maxHeight / original.Height);
    //        int newWidth = (int)(original.Width * scale);
    //        int newHeight = (int)(original.Height * scale);

    //        using var resized = original.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.High);
    //        using var image = SKImage.FromBitmap(resized);
    //        using var outputStream = new MemoryStream();

    //        image.Encode(SKEncodedImageFormat.Png, quality).SaveTo(outputStream);

    //        return outputStream.ToArray();
    //    }
    //    catch (Exception Ex)
    //    {
    //        NotasyncMethod(Ex);
    //        return null;
    //    }
    //}

    private async void UploadPhoto_Clicked(object sender, EventArgs e)
    {
        try
        {
            var photo = await PickPhotoAsync();
            if (photo != null)
            {
                using var stream = await photo.OpenReadAsync();
                Rotate = false;
                var resizedImage = await ResizeImageAsync(stream, 1024, 1024, 80);

                if (resizedImage != null)
                {
                    ResizedImage = resizedImage;
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        ProfilePic.Source = ImageSource.FromStream(() => new MemoryStream(resizedImage));
                    });

                    Savebtn.IsVisible = true;
                    Deletebtn.IsVisible = false;
                    TakePhotobtn.Text = "Take A Photo";
                    var backrandom = new Random();
                    var backrandomnum = backrandom.Next(1000, 10000000);
                    var Filename = Helpers.Settings.UserKey + "-" + DateTime.Now.ToString("dd-MM-yyyy-ss-mm-HH") + "-" + backrandomnum + ".Jpeg";
                    if (string.IsNullOrEmpty(ImageFileName))
                    {
                        //New Instance
                        ImageFileName = Filename;
                    }
                    await Task.Delay(2000);
                    //ProfilePic.Source = ImageSource.FromStream(() => new MemoryStream(resizedImage));
                    //Update FIleName
                                             
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void UploadtoBlobStorage()
    {
        try
        {
            // Parse the connection string and create a blob client
            BlobServiceClient blobServiceClient = new BlobServiceClient(StorageConnectionString);

            // Get a reference to the container
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(Container);

            // Get a reference to the blob
            BlobClient blobClient = containerClient.GetBlobClient(ImageFileName);

            using var stream = new MemoryStream(ResizedImage);
            var response = await blobClient.UploadAsync(stream, overwrite: true);

            //Checks Succesful
            if (response.GetRawResponse().Status is >= 200 and < 300)
            {
               bool isDelete = false; 
               await UpdateProfilePic(isDelete);
            }

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public async Task UpdateProfilePic(bool isDelete)
    {
        try
        {
            string id = Helpers.Settings.UserKey;
            var url = $"https://pwapi.peoplewith.com/api/user/userid/{id}";

            if(isDelete == true)
            {
                ImageFileName = null; 
            }

            var payload = new
            {
                profilepicture = ImageFileName
            };

            // Serialize the object into JSON
            string json = System.Text.Json.JsonSerializer.Serialize(payload);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            ConfigureClient();
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Patch, url)
                {
                    Content = content
                };

                var response = await Client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    //Show Success page
                    
                    WeakReferenceMessenger.Default.Send(new ProfilePicUpdate(ImageFileName));
                    if (isDelete == true)
                    {
                        await MopupService.Instance.PushAsync(new PopupPageHelper("Profile Picture Deleted") { });
                        Preferences.Set("profilepic", string.Empty);
                    }
                    else
                    {
                        await MopupService.Instance.PushAsync(new PopupPageHelper("Profile Picture Uploaded") { });
                        Preferences.Set("profilepic", ImageFileName);
                    }
                    await Task.Delay(1500);
                    await MopupService.Instance.PopAllAsync(false);
                    Navigation.RemovePage(this);
                }
            }

        }
        catch (Exception ex) when (
         ex is HttpRequestException ||
         ex is WebException ||
         ex is TaskCanceledException)
        {
            NotasyncMethod(ex);
        }
        catch (Exception ex)
        {
            NotasyncMethod(ex);
        }
    }


    private void Savebtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (ResizedImage != null)
            {
                    //update Both Blob Storage and UserProfile 
                    UploadtoBlobStorage();
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void Deletebtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            bool isDelete = true; 
            await UpdateProfilePic(isDelete); 
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}