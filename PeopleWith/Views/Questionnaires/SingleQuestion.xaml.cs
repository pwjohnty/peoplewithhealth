using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class SingleQuestion : ContentPage
{
	public SingleQuestion()
	{
		InitializeComponent();
	}

    public SingleQuestion(List<registryDataInputs> questionspassed)
    {
        InitializeComponent();


        mainquestionnaire.ItemsSource = questionspassed;


    }


}