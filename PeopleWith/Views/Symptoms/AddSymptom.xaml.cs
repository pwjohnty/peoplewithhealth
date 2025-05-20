using CommunityToolkit.Mvvm.Messaging;
using Mopups.PreBaked.AbstractClasses;
using Mopups.Services;
using Newtonsoft.Json;
using Syncfusion.Maui.Core;
using Syncfusion.Maui.ListView;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Networking;
using SkiaSharp;
using Azure.Storage.Blobs;
namespace PeopleWith;
public partial class AddSymptom : ContentPage
{
    public ObservableCollection<symptom> FilterResults = new ObservableCollection<symptom>();
    public ObservableCollection<symptom> FilterTabsList = new ObservableCollection<symptom>();
    public ObservableCollection<usersymptom> AddNewSymptom = new ObservableCollection<usersymptom>();
    public ObservableCollection<symptomfeedback> AddNewFeedback = new ObservableCollection<symptomfeedback>();
    public ObservableCollection<symptom> itemstoremove = new ObservableCollection<symptom>();
    public ObservableCollection<usersymptom> SymptomsPassed = new ObservableCollection<usersymptom>();
    public ObservableCollection<usersymptom> UpdatedAllUserSymptoms = new ObservableCollection<usersymptom>();
    public ObservableCollection<symptom> AlreadyAdded = new ObservableCollection<symptom>();
    private bool FilterTabClicked = false;
    bool ShowHideSymptomIMG = false;
    private byte[] ResizedImage;
    symptom SelectedSymptom = new symptom();
    string ImageFileName = string.Empty;
    // private PopupViewModel viewModel;
    private const string StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=peoplewithappiamges;AccountKey=9maBMGnjWp6KfOnOuXWHqveV4LPKyOnlCgtkiKQOeA+d+cr/trKApvPTdQ+piyQJlicOE6dpeAWA56uD39YJhg==;EndpointSuffix=core.windows.net";
    private const string Container = "symptomimages";
    string previous;
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();
    userfeedback userfeedbacklistpassed = new userfeedback();
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

    public AddSymptom(ObservableCollection<usersymptom> ItemsPassed, userfeedback userfeedbacklist)
    {
        try
        {
            InitializeComponent();
            SymptomsPassed = ItemsPassed;
            userfeedbacklistpassed = userfeedbacklist;
            GetBCCall();
            //viewModel = BindingContext as PopupViewModel;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public AddSymptom(ObservableCollection<usersymptom> ItemsPassed)
    {
        try
        {
            InitializeComponent();
            SymptomsPassed = ItemsPassed;
            GetBCCall();
            //viewModel = BindingContext as PopupViewModel;
        }
        catch(Exception Ex) 
        {
            NotasyncMethod(Ex);
        }
    }
    async public void GetBCCall()
    {
        try
        {
            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();  
            Symptomloading.IsVisible = true;
            //Database call for All Symptoms 
            APICalls database = new APICalls();
            ObservableCollection<symptom> userSymptoms = await database.GetSymptomSearchAsync();
            //stopWatch.Stop();
            //TimeSpan ts = stopWatch.Elapsed;
            //await DisplayAlert("Time", ts.ToString(), "Close");
            //Remove Pending and inactive symptoms 
            foreach (var item in userSymptoms)
            {
                if (item.status != "active")
                {
                    itemstoremove.Add(item);
                }
                //Symptom Classification = null, Change to 'Other' 
                if (item.classification == null)
                {
                    item.classification = "Other";
                }
            }
            //Get Symptoms user already added to remove from listview
            var SymptomAdded = new ObservableCollection<symptom>(userSymptoms.Where(s => SymptomsPassed.Any(x => x.symptomtitle == s.title)));
            foreach (var item in SymptomAdded)
            {
                itemstoremove.Add(item);
            }
            //Remove items from list
            foreach (var item in itemstoremove)
            {
                userSymptoms.Remove(item);
            }
            SymptomsListview.ItemsSource = userSymptoms.OrderBy(s => s.title);
            var count = userSymptoms.Count.ToString();
            //Results inital count
            Results.Text = "Results" + " (" + count + ")";
            FilterResults = userSymptoms;


            //Add Classiciation Filters 
            var distinctSymptoms = userSymptoms
                .GroupBy(s => s.classification)
                .Select(g => g.First())
                .ToList().OrderBy(g => g.classification);

            FilterTabsList = new ObservableCollection<symptom>(distinctSymptoms);

            // Insert "All" at the beginning of the list
            var AddAll = new symptom
            {
                classification = "All"
            };
            FilterTabsList.Insert(0, AddAll);
            FilterTabs.ItemsSource = FilterTabsList;
            FilterTabs.DisplayMemberPath = "classification";
            Filterstack.IsVisible = true;
            Symptomloading.IsVisible = false;
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void CheckInput(string symptomID)
    {
        try
        {
            APICalls databse = new APICalls();
            if (symptomID != null)
            {
                ShowHideSymptomIMG = await databse.GetSymptonsInput(symptomID);
            }        
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    //Open Camera to take Photo
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

    private async Task<byte[]> ResizeImageAsync(Stream imageStream, int maxWidth, int maxHeight, int quality)
    {
        try
        {
            using var memoryStream = new MemoryStream();
            await imageStream.CopyToAsync(memoryStream);
            byte[] imageData = memoryStream.ToArray();

            using var original = SKBitmap.Decode(imageData);

            float scale = Math.Min((float)maxWidth / original.Width, (float)maxHeight / original.Height);
            int newWidth = (int)(original.Width * scale);
            int newHeight = (int)(original.Height * scale);

            using var resized = original.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.High);
            using var image = SKImage.FromBitmap(resized);
            using var outputStream = new MemoryStream();

            image.Encode(SKEncodedImageFormat.Png, quality).SaveTo(outputStream);

            return outputStream.ToArray();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
            return null;
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

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void SymptomsListview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var symptom = e.DataItem as symptom;
            SelectedSymptom = symptom;
            CheckInput(symptom.symptomid);
        
            var result = await DisplayAlert("Confirm Symptom", "Are you sure you want to add " + symptom.title + "?", "Ok", "Cancel");
            if (result)
            {
                //OK Clicked
                if (ShowHideSymptomIMG)
                {
                    var Question = await DisplayAlert("Add Symptom Image", "Would you like to add a photo of the affected area?", "Ok", "Cancel");
                    if (Question)
                    {
                        //Ok Clicked (Add Image)
                        var photo = await CapturePhotoAsync();
                        if (photo != null)
                        {
                            using var stream = await photo.OpenReadAsync();
                            var resizedImage = await ResizeImageAsync(stream, 1024, 1024, 80);

                            if (resizedImage != null)
                            {
                                // Save the resized image bytes for later use
                                ResizedImage = resizedImage;
                                //Update FIleName
                                var backrandom = new Random();
                                var backrandomnum = backrandom.Next(1000, 10000000);
                                var Filename = Helpers.Settings.UserKey + "-" + DateTime.Now.ToString("H-m-s-yyyyy-MM-dd") + "-" + backrandomnum + ".Jpeg";
                                ImageFileName = Filename;
                                AddSymptomToDB();

                            }
                        }
                        else
                        {
                            //Cancel Clicked (Add Image)
                            AddSymptomToDB();
                        }
                    }
                    else
                    {
                        //Show Symptom Image False (Add Symptom)
                        AddSymptomToDB();
                    }
                }
                else
                {
                    AddSymptomToDB();
                }
            }
            else
            {
                //Cancel Clicked
                SymptomsListview.SelectedItems.Clear();
                return;
            }
             
         }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void AddSymptomToDB()
    {
        try
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {

                var usersymptom = new symptomfeedback();
                Guid newUUID = Guid.NewGuid();
                usersymptom.symptomfeedbackid = newUUID.ToString().ToUpper();
                usersymptom.timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                usersymptom.intensity = "50";
                usersymptom.notes = null;
                usersymptom.triggers = null;
                usersymptom.interventions = null;
                usersymptom.duration = null;
                if (!String.IsNullOrEmpty(ImageFileName))
                {
                    usersymptom.symptomimage = ImageFileName;
                }            
                AddNewFeedback.Add(usersymptom);
                var NewSymptom = new usersymptom();
                NewSymptom.userid = Helpers.Settings.UserKey;
                NewSymptom.symptomtitle = SelectedSymptom.title;
                NewSymptom.symptomid = SelectedSymptom.symptomid;
                NewSymptom.feedback = AddNewFeedback;


                APICalls database = new APICalls();
                //insert to db
                var returnedsymptom = await database.PostSymptomAsync(NewSymptom);

                if (!String.IsNullOrEmpty(ImageFileName))
                {
                    UploadtoBlobStorage();
                }

                SymptomsPassed.Add(returnedsymptom);

                var newsym = new feedbackdata();
                newsym.value = "50";
                newsym.datetime = AddNewFeedback[0].timestamp;
                newsym.action = "add";
                newsym.label = NewSymptom.symptomtitle;
                newsym.id = usersymptom.symptomfeedbackid;

                if (userfeedbacklistpassed.symptomfeedbacklist == null)
                {
                    userfeedbacklistpassed.symptomfeedbacklist = new ObservableCollection<feedbackdata>();
                }

                userfeedbacklistpassed.symptomfeedbacklist.Add(newsym);

                string newsymJson = System.Text.Json.JsonSerializer.Serialize(userfeedbacklistpassed.symptomfeedbacklist);
                userfeedbacklistpassed.symptomfeedback = newsymJson;


                await database.UserfeedbackUpdateSymptomData(userfeedbacklistpassed);


                await MopupService.Instance.PushAsync(new PopupPageHelper("Symptom Added") { });
                await Task.Delay(1500);

                await Navigation.PushAsync(new AllSymptoms(SymptomsPassed, userfeedbacklistpassed));

                await MopupService.Instance.PopAllAsync(false);

                var pageToRemoves = Navigation.NavigationStack.ToList();

                int i = 0;

                foreach (var page in pageToRemoves)
                {
                    if (i == 0)
                    {
                    }
                    else if (i == 1 || i == 2 || i == 3)
                    {
                        Navigation.RemovePage(page);
                    }
                    else
                    {
                        //Navigation.RemovePage(page);
                    }
                    i++;
                }
            }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }       
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (FilterTabClicked) return;

            var Characters = searchbar.Text.ToString();
            var filteredSymptoms = new ObservableCollection<symptom>(FilterResults.Where(s => s.title.Contains(Characters, StringComparison.OrdinalIgnoreCase))).OrderBy(m => m.title);
            SymptomsListview.ItemsSource = filteredSymptoms;
            var count = filteredSymptoms.Count().ToString();
            Results.Text = "Results" + " (" + count + ")";

            if (count == "0")
            {
                NoResultslbl.IsVisible = true;
                SymptomsListview.IsVisible = false;
            }
            else
            {
                SymptomsListview.IsVisible = true;
                NoResultslbl.IsVisible = false;
            }

            //If FilterTabs item is Selected - UnSelect it 
            if (string.IsNullOrEmpty(searchbar.Text) || searchbar.Text == "")
            {
                FilterTabs.SelectedItem = FilterTabsList[0];
            }
            else
            {
                if (FilterTabs.SelectedItem != null)
                {
                    FilterTabs.SelectedItem = null;
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            if (Filterstack.IsVisible == true)
            {
                Filterstack.IsVisible = false;
                if(DeviceInfo.Platform == DevicePlatform.Android)
                {
                    ListViewStack.Margin = new Thickness(0, 0, 0, 0);
                }
                else if(DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    ListViewStack.Margin = new Thickness(0, -50, 0, 0);
                }
                
            }
            else
            {
                Filterstack.IsVisible = true;
                ListViewStack.Margin = new Thickness(0, 0, 0, 0);
            }
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void FilterTabs_ChipClicked(object sender, EventArgs e)
    {
        try
        {
            var tappedFrame = sender as SfChip;
            var item = tappedFrame.Text;
            FilterTabClicked = true;


            if (item == "All")
            {
                var count = FilterResults.Count().ToString();
                Results.Text = "Results" + " (" + count + ")";
                SymptomsListview.ItemsSource = FilterResults;
                SymptomsListview.IsVisible = true;
                NoResultslbl.IsVisible = false;
                searchbar.Text = String.Empty;
            }
            else
            {
                var filteredSymptoms = new ObservableCollection<symptom>(FilterResults.Where(s => s.classification.Contains(item, StringComparison.OrdinalIgnoreCase))).OrderBy(m => m.title);
                var count = filteredSymptoms.Count().ToString();
                Results.Text = "Results" + " (" + count + ")";
                SymptomsListview.ItemsSource = filteredSymptoms;
                SymptomsListview.IsVisible = true;
                NoResultslbl.IsVisible = false;
                searchbar.Text = String.Empty;

            }

            Device.BeginInvokeOnMainThread(() =>
            {
                FilterTabClicked = false;
            });

        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    async private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        try
        {
            await Navigation.PopAsync();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }    
    }
    //private void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
    //{
    //}
}