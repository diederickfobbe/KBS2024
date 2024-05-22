using Business_Logic;

namespace User_Interface.Schermen
{
    public partial class ProfielScherm : ContentPage
    {
        private User user;

        public ProfielScherm(User user)
        {
            InitializeComponent();
            this.user = user;
            SetUpUserProfile();
        }

        private void SetUpUserProfile()
        {

            UserNameLabel.Text = "Welkom! " + user.Username;

        }
    }
}
