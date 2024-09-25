using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class AddSupplements : ContentPage
{
	public AddSupplements(ObservableCollection<usersupplement> AllUserSupplements)
	{
		InitializeComponent();
	}
}