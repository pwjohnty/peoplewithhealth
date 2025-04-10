using Maui.FreakyControls;
using Microsoft.Maui.Layouts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith;
public partial class SymptomGallery : ContentPage
{
    ObservableCollection<symptomfeedback> FeedbackPassed = new ObservableCollection<symptomfeedback>();
    ObservableCollection<GalleryItem> ListviewItems = new ObservableCollection<GalleryItem>();
    public int currentIndex = 0;
    private double currentScale = 1.0;

    //Commented Out Code
    //private double startScale = 1.0;
    //private double xOffset = 0;
    //private double yOffset = 0;
    //private bool isPanEnabled = false;
    //private bool isZoomed => currentScale > 1.1;
    //private PanGestureRecognizer panGesture;
    //private SwipeGestureRecognizer swipeLeft;
    //private SwipeGestureRecognizer swipeRight;
    //private Stopwatch TimeBetween = new Stopwatch();
    //private bool DoubleTap = false;
    //private DateTime lastTapTime = DateTime.MinValue;

    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();
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

    public class GalleryItem
    {
        public string Image { get; set; }
        public string TimeDate { get; set; }
        public int index { get; set; }
    }

    public SymptomGallery(ObservableCollection<symptomfeedback> Feedback, string Title)
    {
        InitializeComponent();
        FeedbackPassed = Feedback;

        int NewIndex = 0;
        foreach (var item in FeedbackPassed)
        {
            if (item.ImageAttached == true)
            {
                var imagestring = $"https://peoplewithappiamges.blob.core.windows.net/symptomimages/{item.symptomimage}?t={DateTime.UtcNow.Ticks}";

                ListviewItems.Add(new GalleryItem() { index = NewIndex, Image = imagestring, TimeDate = item.formattedDateTime});
                NewIndex++;
            }
        }

        //Set items Intially
        CurrentImage.Source = ListviewItems[0].Image;
        NextImage.Source = ListviewItems[0].Image;
        MainText.Text = ListviewItems[0].TimeDate;

        ThumbNail.ItemsSource = ListviewItems;
        ThumbNail.SelectedItem = ListviewItems[0];
        ImageCount.Text = "1 of " + ListviewItems.Count.ToString();
        GalleryTitle.Text = Title;

        //Initalize Pinch Guesture 
        //InitializeGestureRecognizers();

        //Check When Zoom Updated to Show/Hide Button
        SetupZoomChangedListener();
    }

    // Initialize Gesture Recognizers
    private void InitializeGestureRecognizers()
    {
        try
        {
            //Not used (Add Commented Out)
            //var pinchGesture = new PinchGestureRecognizer();
            //pinchGesture.PinchUpdated += OnPinchUpdatedAsync;
            //Container.GestureRecognizers.Add(pinchGesture);

            //panGesture = new PanGestureRecognizer();
            //panGesture.PanUpdated += OnPanUpdatedAsync;
            //Container.GestureRecognizers.Add(panGesture);

            //var TaptoZoom = new TapGestureRecognizer
            //{
            //    NumberOfTapsRequired = 2,
            //};

            //TaptoZoom.Tapped += DoubleTappedAsync;
            //Container.GestureRecognizers.Add(TaptoZoom);

            //Swipe Guesture Recognizer 

            //var swipeLeft = new SwipeGestureRecognizer
            //{
            //    Direction = SwipeDirection.Left,
            //    Threshold = 12, 
          
            //};
            //swipeLeft.Swiped += OnSwipeUpdatedAsync;

            //var swipeRight = new SwipeGestureRecognizer
            //{
            //    Direction = SwipeDirection.Right,
            //    Threshold = 12
            //};
            //swipeRight.Swiped += OnSwipeUpdatedAsync;

            //Container.GestureRecognizers.Add(swipeLeft);
            //Container.GestureRecognizers.Add(swipeRight);
            //Container.GestureRecognizers.Add(swipeLeft);
            //Container.GestureRecognizers.Add(swipeRight);

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private async void ThumbNail_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            bool IsZoomed = await IsFreakyZoomed();
            if (IsZoomed)
            {
                ResetFreakyZoom();
            }

            var SelectedItem = e.DataItem as GalleryItem;
            //Check Current Index
            int CheckIndex = ListviewItems.IndexOf(SelectedItem);
            if (currentIndex == CheckIndex)
            {
                //If Image Currently Set is Same as Index (Do Nothing)
                return;
            }
            else
            {
                currentIndex = CheckIndex;
                if (SelectedItem != null)
                {
                    await UpdateImageAndUI();
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    //Check if Freaky Is Zoomed
    public async Task<bool> IsFreakyZoomed()
    {
        try
        {
            var zoomField = typeof(FreakyZoomableView).GetField("_currentScale", BindingFlags.NonPublic | BindingFlags.Instance);

            if (zoomField != null)
            {
                var currentScale = (double)zoomField.GetValue(ZoomControl);
                bool isZoomed = currentScale > ZoomControl.MinScale;
                return isZoomed;
            }
            else
            {
                return false;
            }
        }
        catch(Exception Ex)
        {
            return false;
        }
    }

    async private void SetupZoomChangedListener()
    {
        ZoomControl.PropertyChanged += (sender, e) =>
        {
            // Check if the scale property has changed
            if (e.PropertyName == "Scale")
            {
                HandleZoomChangedAsync();
            }
        };
    }

    private async void HandleZoomChangedAsync()
    {
        bool isZoomed = await IsFreakyZoomed();
        if (isZoomed)
        {
            ResetBtn.IsVisible = true; 
        }
        else
        {
            ResetBtn.IsVisible = false; 
        }
    }

    // Reset internal gesture state
    private void ResetFreakyZoom()
    {
        try
        {
            //Reverts Image to Original Size 
            var contentProperty = typeof(FreakyZoomableView).GetProperty("Content", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (contentProperty?.GetValue(ZoomControl) is View GetView)
            {
                GetView.Scale = ZoomControl.MinScale;
                GetView.TranslationX = 0;
                GetView.TranslationY = 0;
            }

            ResetField("_xOffset", 0.0);
            ResetField("_yOffset", 0.0);
            ResetField("_startScale", 1.0);
            ResetField("_currentScale", 1.0);
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }


    void ResetField(string fieldName, object value)
    {
        var field = typeof(FreakyZoomableView).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (field != null)
        {
            field.SetValue(ZoomControl, value);
        }
    }
    private async void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            bool IsZoomed = await IsFreakyZoomed();
            if (IsZoomed)
            {
                ResetFreakyZoom();
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    //Swipe Handler Method
    private async void OnSwipeUpdatedAsync(object? sender, SwipedEventArgs e)
    {
        try
        {
            //Stops Swipe Guesture When Zoomed in 
            bool IsZoomed = await IsFreakyZoomed();
            if (IsZoomed)
            {
                return;
            }

            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    await NavigateToNextImage();
                    break;


                case SwipeDirection.Right:
                    await NavigateToPreviousImage();
                    break;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    // Navigation to previous image
    private async Task NavigateToPreviousImage()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            await UpdateImageAndUI();
        }
        else
        {
            Vibration.Vibrate();
        }
    }

    // Navigation to next image
    private async Task NavigateToNextImage()
    {
        if (currentIndex < ListviewItems.Count - 1)
        {
            currentIndex++;
            await UpdateImageAndUI();
        }
        else
        {
            Vibration.Vibrate();
        }
    }

    // Update image and UI (crossfade, thumbnail, etc.)
    private async Task UpdateImageAndUI()
    {
        await CrossfadeImage(ListviewItems[currentIndex].Image);
        MainText.Text = ListviewItems[currentIndex].TimeDate;
        ThumbNail.ScrollTo(ListviewItems[currentIndex], ScrollToPosition.Center, true);
        ImageCount.Text = $"{currentIndex + 1} of {ListviewItems.Count}";
        ThumbNail.SelectedItem = ListviewItems[currentIndex];
    }

    // Stop Flashing of Image
    private async Task CrossfadeImage(string newSource)
    {
        NextImage.Source = newSource;
        await Task.WhenAll(
            CurrentImage.FadeTo(0, 250),
            NextImage.FadeTo(1, 250)
        );

        // Swap current and next image
        (CurrentImage, NextImage) = (NextImage, CurrentImage);
        NextImage.Opacity = 0;
    }





    ///////////////////////////////////////////////////////////////
    ////////// Old Code Not used //////////////////////////////////
    ///////////////////////////////////////////////////////////////

    //private async Task ClampTranslationAsync(double transX, double transY, bool animate = false)
    //{
    //    if (DeviceInfo.Platform == DevicePlatform.iOS)
    //    {
    //        Container.AnchorX = 0;
    //        Container.AnchorY = 0;
    //    }
    //    else
    //    {
    //        Container.AnchorX = 0;  // Default center anchor for Android
    //        Container.AnchorY = 0;
    //    }

    //    double contentWidth = Container.Width * currentScale;
    //    double contentHeight = Container.Height * currentScale;

    //    if (contentWidth <= Width)
    //    {
    //        transX = -(contentWidth - Container.Width) / 2;
    //    }
    //    else
    //    {
    //        double minBoundX = ((Width - Container.Width) / 2) + contentWidth - Width;
    //        double maxBoundX = (Width - Container.Width) / 2;
    //        transX = Math.Clamp(transX, -minBoundX, -maxBoundX);
    //    }

    //    if (contentHeight <= Height)
    //    {
    //        transY = -(contentHeight - Container.Height) / 2;
    //    }
    //    else
    //    {
    //        double minBoundY = ((Height - Container.Height) / 2) + contentHeight - Height;
    //        double maxBoundY = (Height - Container.Height) / 2;
    //        transY = Math.Clamp(transY, -minBoundY, -maxBoundY);
    //    }

    //    if (animate)
    //    {
    //        await TranslateToAsync(transX, transY);
    //    }
    //    else
    //    {
    //        Container.TranslationX = transX;
    //        Container.TranslationY = transY;
    //    }
    //}

    //private async Task ClampTranslationFromScaleOriginAsync(double originX, double originY, bool animate = false)
    //{
    //    // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
    //    // so get the X pixel coordinate.
    //    double renderedX = Container.X + xOffset;
    //    double deltaX = renderedX / Width;
    //    double deltaWidth = Width / (Container.Width * startScale);
    //    originX = (originX - deltaX) * deltaWidth;

    //    // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
    //    // so get the Y pixel coordinate.
    //    double renderedY = Container.Y + yOffset;
    //    double deltaY = renderedY / Height;
    //    double deltaHeight = Height / (Container.Height * startScale);
    //    originY = (originY - deltaY) * deltaHeight;

    //    // Calculate the transformed element pixel coordinates.
    //    double targetX = xOffset - (originX * Container.Width * (currentScale - startScale));
    //    double targetY = yOffset - (originY * Container.Height * (currentScale - startScale));

    //    // Apply translation based on the change in origin.
    //    if (currentScale > 1)
    //    {
    //        targetX = Math.Clamp(targetX, -Container.Width * (currentScale - 1), 0);
    //        targetY = Math.Clamp(targetY, -Container.Height * (currentScale - 1), 0);
    //    }
    //    else
    //    {
    //        targetX = (Width - (Container.Width * currentScale)) / 2;
    //        targetY = Container.Height * (1 - currentScale) / 2;
    //    }

    //    await ClampTranslationAsync(targetX, targetY, animate);
    //}

    //private async void DoubleTappedAsync(object? sender, TappedEventArgs e)
    //{
    //    //var currentTime = DateTime.UtcNow;
    //    //var timeSinceLastTap = currentTime - lastTapTime;

    //    //if (timeSinceLastTap.TotalMilliseconds >= 300 && timeSinceLastTap.TotalMilliseconds <= 500)
    //    //{
    //    //    // Double-tap detected within 300-500ms
    //    //    await DoubleTapped(sender, e);
    //    //}

    //    //lastTapTime = currentTime;

    //    startScale = Container.Scale;
    //    currentScale = startScale;
    //    xOffset = Container.TranslationX;
    //    yOffset = Container.TranslationY;

    //    if (currentScale < 2)
    //    {
    //        currentScale = 2;
    //        EnablePan();
    //    }
    //    else
    //    {
    //        currentScale = 1;
    //        EnableSwipe();
    //    }

    //    var point = e.GetPosition(sender as View);

    //    var translateTask = Task.CompletedTask;

    //    if (point is not null)
    //    {
    //        translateTask = ClampTranslationFromScaleOriginAsync(point.Value.X / Width, point.Value.Y / Height, true);
    //    }

    //    var scaleTask = ScaleToAsync(currentScale);

    //    await Task.WhenAll(translateTask, scaleTask);

    //    xOffset = Container.TranslationX;
    //    yOffset = Container.TranslationY;
    //}

    //private async void OnPanUpdatedAsync(object? sender, PanUpdatedEventArgs e)
    //{
    //    if (!isPanEnabled)
    //    {
    //        return;
    //    }

    //    if (Container.Scale <= 1)
    //    {
    //        EnableSwipe();
    //        return;       
    //    }

    //    if (e.StatusType == GestureStatus.Started)
    //    {
    //        xOffset = Container.TranslationX;
    //        yOffset = Container.TranslationY;

    //        if (DeviceInfo.Platform == DevicePlatform.iOS)
    //        {
    //            Container.AnchorX = 0;  
    //            Container.AnchorY = 0;
    //        }
    //        else
    //        {
    //            Container.AnchorX = 0;  // Default center anchor for Android
    //            Container.AnchorY = 0;
    //        }
    //    }
    //    else if (e.StatusType == GestureStatus.Running)
    //    {
    //        // Optional: Adjust the speed multiplier based on scale
    //        double speedMultiplier = Math.Max(1, currentScale); // E.g., if zoomed in 3x, move 3x faster

    //        double adjustedX = xOffset + e.TotalX * speedMultiplier;
    //        double adjustedY = yOffset + e.TotalY * speedMultiplier;
    //        await ClampTranslationAsync(adjustedX, adjustedY);
    //        // Translate and pan.
    //        //await ClampTranslationAsync(xOffset + e.TotalX, yOffset + e.TotalY);
    //    }
    //    else if (e.StatusType == GestureStatus.Completed)
    //    {
    //        // Store the translation applied during the pan
    //        xOffset = Container.TranslationX;
    //        yOffset = Container.TranslationY;
    //    }
    //    else if (e.StatusType == GestureStatus.Canceled)
    //    {
    //        Container.TranslationX = xOffset;
    //        Container.TranslationY = yOffset;
    //    }
    //}

   

    //private async void OnPinchUpdatedAsync(object? sender, PinchGestureUpdatedEventArgs e)
    //{
    //    if (e.Status == GestureStatus.Started)
    //    {
    //        isPanEnabled = false;
    //        //isSwipeEnabled = false; 

    //        xOffset = Container.TranslationX;
    //        yOffset = Container.TranslationY;

    //        // Store the current scale factor applied to the wrapped user interface element,
    //        // and zero the components for the center point of the translate transform.
    //        startScale = Container.Scale;

    //        if (DeviceInfo.Platform == DevicePlatform.iOS)
    //        {
    //            Container.AnchorX = 0;
    //            Container.AnchorY = 0;
    //        }
    //        else
    //        {
    //            Container.AnchorX = 0;  // Default center anchor for Android
    //            Container.AnchorY = 0;
    //        }
    //    }

    //    if (e.Status == GestureStatus.Running)
    //    {
    //        // Calculate the scale factor to be applied.
    //        currentScale += (e.Scale - 1) * startScale;
    //        currentScale = Math.Clamp(currentScale, 0.5, 10);

    //        await ClampTranslationFromScaleOriginAsync(e.ScaleOrigin.X, e.ScaleOrigin.Y);

    //        // Apply scale factor
    //        Container.Scale = currentScale;
    //    }

    //    if (e.Status == GestureStatus.Completed)
    //    {
    //        if (currentScale < 1)
    //        {
    //            var translateTask = TranslateToAsync(0, 0);
    //            var scaleTask = ScaleToAsync(1);

    //            await Task.WhenAll(translateTask, scaleTask);
    //        }

    //        xOffset = Container.TranslationX;
    //        yOffset = Container.TranslationY;

    //        isPanEnabled = true;
    //    }
    //    else if (e.Status == GestureStatus.Canceled)
    //    {
    //        Container.TranslationX = xOffset;
    //        Container.TranslationY = yOffset;
    //        Container.Scale = startScale;

    //        isPanEnabled = true;
    //    }
    //}
    //private async Task ScaleToAsync(double scale)
    //{
    //    await Container.ScaleTo(scale, 250, Easing.Linear);
    //    currentScale = scale;
    //}

    //private async Task TranslateToAsync(double x, double y)
    //{
    //    await Container.TranslateTo(x, y, 250, Easing.Linear);
    //    xOffset = x;
    //    yOffset = y;
    //}

    //private void EnablePan()
    //{
    //    if (!Container.GestureRecognizers.Contains(panGesture))
    //    {
    //        Container.GestureRecognizers.Add(panGesture);
    //        Container.GestureRecognizers.Remove(swipeLeft);
    //        Container.GestureRecognizers.Remove(swipeRight);
    //        ResetBtn.IsVisible = true;
    //    }
    //}

    //private void EnableSwipe()
    //{
    //    if (!Container.GestureRecognizers.Contains(swipeLeft))
    //    {
    //        Container.GestureRecognizers.Add(swipeLeft);
    //        ResetBtn.IsVisible = false;
    //    }
           
    //    if (!Container.GestureRecognizers.Contains(swipeRight))
    //    {
    //        Container.GestureRecognizers.Add(swipeRight);
    //    }
            
    //    Container.GestureRecognizers.Remove(panGesture);
    //}

}