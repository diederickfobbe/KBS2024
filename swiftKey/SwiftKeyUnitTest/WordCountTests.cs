using NUnit.Framework;
using Business_Logic;
using SwiftKey_Logic;

[TestFixture]
public class WordCountTests
{
    [Test]
    public void GetWordCount_WithNormalSentence_ReturnsCorrectCount()
    {
        // Arrange
        var text = "Hello, world! This is a test.";

        // Act
        var count = OefenschermMethods.GetWordCount(text);

        // Assert
        Assert.AreEqual(6, count);
    }

    [Test]
    public void GetWordCount_WithMultipleSpaces_ReturnsCorrectCount()
    {
        // Arrange
        var text = "Hello    world  !  This   is a test.";

        // Act
        var count = OefenschermMethods.GetWordCount(text);

        // Assert
        Assert.AreEqual(6, count);
    }

    [Test]
    public void GetWordCount_WithPunctuations_ReturnsCorrectCount()
    {
        // Arrange
        var text = "Hello, world! This, is a test.";

        // Act
        var count = OefenschermMethods.GetWordCount(text);

        // Assert
        Assert.AreEqual(6, count);
    }

    [Test]
    public void GetWordCount_WithEmptyString_ReturnsZero()
    {
        // Arrange
        var text = "";

        // Act
        var count = OefenschermMethods.GetWordCount(text);

        // Assert
        Assert.AreEqual(0, count);
    }

    [Test]
    public void GetWordCount_WithOnlyPunctuationsAndSpaces_ReturnsZero()
    {
        // Arrange
        var text = " , ! ? . ,";

        // Act
        var count = OefenschermMethods.GetWordCount(text);

        // Assert
        Assert.AreEqual(0, count);
    }

    [Test]
    public void GetWordCount_WithNull_ThrowsArgumentNullException()
    {
        // Arrange
        string text = null;

        // Act & Assert
        var ex = Assert.Throws<System.ArgumentNullException>(() => OefenschermMethods.GetWordCount(text));
        Assert.That(ex.ParamName, Is.EqualTo("text"));
    }
}

