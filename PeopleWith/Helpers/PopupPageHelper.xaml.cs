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

        detaillbl.Text = message;
    }
}