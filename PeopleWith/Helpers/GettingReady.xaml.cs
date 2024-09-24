using Mopups.Pages;

namespace PeopleWith;

public partial class GettingReady : PopupPage
{
	public GettingReady()
	{
		InitializeComponent();
	}

    public GettingReady(string message)
    {
        InitializeComponent();

        detaillbl.Text = message;
    }
}