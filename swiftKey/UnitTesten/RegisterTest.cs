using NUnit.Framework;
using Business_Logic;

namespace Business_Logic.Tests
{
    [TestFixture]
    public class CheckComplexityTests
    {
        [Test]
        public void EmailValidCheck_ValidEmail_ReturnsTrue()
        {
            // Arrange
            string email = "test@example.com";

            // Act
            bool result = CheckComplexity.EmailValidCheck(email);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void EmailValidCheck_InvalidEmail_ReturnsFalse()
        {
            // Arrange
            string email = "invalid-email";

            // Act
            bool result = CheckComplexity.EmailValidCheck(email);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void PasswordValidCheck_ValidPassword_ReturnsTrue()
        {
            // Arrange
            string password = "Valid1Password!";

            // Act
            bool result = CheckComplexity.PasswordValidCheck(password);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void PasswordValidCheck_TooShort_ReturnsFalse()
        {
            // Arrange
            string password = "Short1!";

            // Act
            bool result = CheckComplexity.PasswordValidCheck(password);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void PasswordValidCheck_NoUpperCase_ReturnsFalse()
        {
            // Arrange
            string password = "validpassword1!";

            // Act
            bool result = CheckComplexity.PasswordValidCheck(password);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void PasswordValidCheck_NoLowerCase_ReturnsFalse()
        {
            // Arrange
            string password = "VALIDPASSWORD1!";

            // Act
            bool result = CheckComplexity.PasswordValidCheck(password);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void PasswordValidCheck_NoDigit_ReturnsFalse()
        {
            // Arrange
            string password = "ValidPassword!";

            // Act
            bool result = CheckComplexity.PasswordValidCheck(password);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void PasswordValidCheck_NoSpecialCharacter_ReturnsFalse()
        {
            // Arrange
            string password = "ValidPassword1";

            // Act
            bool result = CheckComplexity.PasswordValidCheck(password);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
