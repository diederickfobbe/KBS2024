using SwiftKey_Logic;
using NUnit.Framework;
using Business_Logic;
using SwiftKey_Logic;
using NUnit.Framework.Legacy;

namespace SwiftKeyUnitTest
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void Button_OnRegisterClicked_EmptyFields_DisplayAlert()
        {
            // Arrange
            var registratiescherm = new Registratiescherm();
            var mockPage = new Mock<Page>();
            registratiescherm.DisplayAlertMethod = mockPage.Object.DisplayAlert;

            // Act
            registratiescherm.Button_OnRegisterClicked(null, null);

            // Assert
            mockPage.Verify(x => x.DisplayAlert("Fout", "Vul alle velden in", "OK"), Times.Once);
        }


    }
}