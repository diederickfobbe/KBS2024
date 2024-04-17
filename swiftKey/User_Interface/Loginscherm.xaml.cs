namespace User_Interface
{
    public partial class Loginscherm : ContentPage {

        public Loginscherm()
        {
            InitializeComponent();
        }
        

        private void Button_OnClicked(object? sender, EventArgs e)
        {
            Navigation.PushAsync(new Oefenscherm());
        }
    }

}
