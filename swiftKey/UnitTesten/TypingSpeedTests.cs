using NUnit.Framework;
using System;
using Business_Logic;
using SwiftKey_Logic;
using NUnit.Framework;
using Business_Logic;
using SwiftKey_Logic;
using NUnit.Framework;

namespace TypingTests
{
    [TestFixture]
    public class TypingSpeedTests
    {
        [Test]
        public void CalculateTypingSpeed_WithPositiveInputs_ReturnsCorrectSpeed()
        {
            // Arrange
            int wordCount = 240;
            double timeTakenInMinutes = 2;

            // Act
            int speed = OefenschermMethods.CalculateTypingSpeed(wordCount, timeTakenInMinutes);

            // ClassicAssert
            Assert.AreEqual(120, speed);
        }

        [Test]
        public void CalculateTypingSpeed_WithZeroWords_ReturnsZeroSpeed()
        {
            // Arrange
            int wordCount = 0;
            double timeTakenInMinutes = 5;

            // Act
            int speed = OefenschermMethods.CalculateTypingSpeed(wordCount, timeTakenInMinutes);

            // ClassicAssert
            Assert.AreEqual(0, speed);
        }

        [Test]
        public void CalculateTypingSpeed_WithZeroTime_ThrowsArgumentException()
        {
            // Arrange
            int wordCount = 100;
            double timeTakenInMinutes = 0;

            // Act & ClassicAssert
            var ex = Assert.Throws<ArgumentException>(() => OefenschermMethods.CalculateTypingSpeed(wordCount, timeTakenInMinutes));
            Assert.That(ex.Message, Does.Contain("Time taken must be greater than zero."));
            Assert.AreEqual("timeTakenInMinutes", ex.ParamName);
        }

        [Test]
        public void CalculateTypingSpeed_WithNegativeTime_ThrowsArgumentException()
        {
            // Arrange
            int wordCount = 100;
            double timeTakenInMinutes = -1;

            // Act & ClassicAssert
            var ex = Assert.Throws<ArgumentException>(() => OefenschermMethods.CalculateTypingSpeed(wordCount, timeTakenInMinutes));
            Assert.That(ex.Message, Does.Contain("Time taken must be greater than zero."));
            Assert.AreEqual("timeTakenInMinutes", ex.ParamName);
        }
    }
}
