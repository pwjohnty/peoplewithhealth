using Mopups.Pages;

namespace PeopleWith;

public partial class PopupPageHelper : PopupPage
{
	public PopupPageHelper()
	{
		InitializeComponent();
	}

    public PopupPageHelper(string message)
    {
        InitializeComponent();


        if (message != null)
        {
            if (message == "Updating...")
            {
                listloader.IsVisible = true;
                detaillbl2.IsVisible = true;
                img.IsVisible = false;
            }
        }

        detaillbl.Text = message;
    }


    public void UpdateMessage(string newMessage)
    {
        if (newMessage != null)
        {
            img.IsVisible = true;
            listloader.IsVisible = false;
            detaillbl2.IsVisible = false; 
            detaillbl.Text = newMessage;
        }
    }
}