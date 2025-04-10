namespace PeopleWith;

public partial class HealthDataPage : ContentPage
{
	public HealthDataPage()
	{
		InitializeComponent();

		checkhealthdata();
	}


	async void checkhealthdata()
	{
		try
		{
            var healthService = Application.Current.Handler.MauiContext.Services.GetService<IHealthKitService>();
           // var healthService = MauiApp.Current.Services.GetService<IHealthKitService>();

            var isAuthorized = await healthService.RequestAuthorization();

            if (isAuthorized)
            {
                var steps = await healthService.GetStepCount(DateTime.Today.AddDays(-1), DateTime.Today);

				stepslbl.Text = steps.ToString();

                Console.WriteLine($"Steps: {steps}");
            }

        }
		catch(Exception ex)
		{

		}
	}

}