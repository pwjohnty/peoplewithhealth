using Mopups.Pages;
using Mopups.Services;

namespace PeopleWith;

public partial class imagePopUp : PopupPage
{
    public imagePopUp()
    {
        InitializeComponent();
    }

    public imagePopUp(string message)
    {
        try
        {
            InitializeComponent();
            mainimage.Source = ImageSource.FromUri(new Uri(message));

        }
        catch (Exception Ex)
        {

        }
    }

    public imagePopUp(string message, string rotate)
    {
        try
        {
            //just used to rotate the image 90°
            InitializeComponent();
            mainimage.Source = ImageSource.FromUri(new Uri(message));
            mainimage.Rotation = 90; 

        }
        catch (Exception Ex)
        {

        }
    }

    //async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    //{
    //    try
    //    {
    //        await MopupService.Instance.PopAsync();
    //    }
    //    catch (Exception Ex)
    //    {

    //    }
    //}

    async private void Closebtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            await MopupService.Instance.PopAsync();
        }
        catch (Exception Ex)
        {

        }
    }
}