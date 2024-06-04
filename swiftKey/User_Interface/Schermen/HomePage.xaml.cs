using Business_Logic;
using SwiftKey_Logic;

namespace User_Interface.Schermen;

public partial class HomePage : ContentPage
{
    private User user;

    public HomePage(User user)
	{
		InitializeComponent();
		this.user = user;
	}

    private async void onOefeningenButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SelecterenOefening(user));
    }

    private async void onLeaderboardButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LeaderboardScherm(user));
    }

    private async void onAccountButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProfielScherm(user));
    }

    private async void onLogoutButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.Navigation.PopToRootAsync();
    }
}