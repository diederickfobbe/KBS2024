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

    
}