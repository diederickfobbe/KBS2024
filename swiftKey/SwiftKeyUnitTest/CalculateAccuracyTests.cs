using NUnit.Framework;
using NUnit.Framework;
using Business_Logic;
using SwiftKey_Logic;
using NUnit.Framework.Legacy;
using SwiftKey_Logic;


[TestFixture]
public class AccuracyTests
{
    
    
    [Test]
    public void CalculateAccuracy_WithPerfectMatch_Returns100Percent()
    {
        // Arrange
        string enteredText = "Hello, world! This is a test.";
        string targetText = "Hello, world! This is a test.";
        int targetWordCount = 6;

        // Act
        double accuracy = OefenschermMethods.CalculateAccuracy(enteredText, targetWordCount, targetText);

        // ClassicAssert
        ClassicAssert.AreEqual(100.0, accuracy);
    }

    [Test]
    public void CalculateAccuracy_WithNoCorrectWords_ReturnsZeroPercent()
    {
        // Arrange
        string enteredText = "Some random words";
        string targetText = "Hello, world! This is a test.";
        int targetWordCount = 6;

        // Act
        double accuracy = OefenschermMethods.CalculateAccuracy(enteredText, targetWordCount, targetText);

        // ClassicAssert
        ClassicAssert.AreEqual(0.0, accuracy);
    }

    [Test]
    public void CalculateAccuracy_WithSomeCorrectWords_ReturnsCorrectPercentage()
    {
        // Arrange
        string enteredText = "Hello, this is a test.";
        string targetText = "Hello, world! This is a test.";
        int targetWordCount = 6;

        // Act
        double accuracy = OefenschermMethods.CalculateAccuracy(enteredText, targetWordCount, targetText);

        // ClassicAssert
        ClassicAssert.AreEqual(83.33, Math.Round(accuracy, 2));
    }

    [Test]
    public void CalculateAccuracy_WithEmptyEnteredText_ReturnsZeroPercent()
    {
        // Arrange
        string enteredText = "";
        string targetText = "Hello, world! This is a test.";
        int targetWordCount = 6;

        // Act
        double accuracy = OefenschermMethods.CalculateAccuracy(enteredText, targetWordCount, targetText);

        // ClassicAssert
        ClassicAssert.AreEqual(0.0, accuracy);
    }

    [Test]
    public void CalculateAccuracy_WithEmptyTargetText_ReturnsZeroPercent()
    {
        // Arrange
        string enteredText = "Hello, world! This is a test.";
        string targetText = "";
        int targetWordCount = 0; // Target word count should ideally match target text words count

        // Act
        double accuracy = OefenschermMethods.CalculateAccuracy(enteredText, targetWordCount, targetText);

        // ClassicAssert
        ClassicAssert.AreEqual(0.0, accuracy);
    }

    [Test]
    public void CalculateAccuracy_WithMismatchedWordCounts_ThrowsArgumentException()
    {
        // Arrange
        string enteredText = "Hello, world! This is a test.";
        string targetText = "Hello, world! This is a test.";
        int targetWordCount = 7;  // Mismatch with actual words in target text

        // Act & ClassicAssert
        var ex = ClassicAssert.Throws<System.ArgumentException>(() => OefenschermMethods.CalculateAccuracy(enteredText, targetWordCount, targetText));
        ClassicAssert.That(ex.Message, Is.EqualTo("Target word count does not match the actual number of words in target text."));
    }
}



