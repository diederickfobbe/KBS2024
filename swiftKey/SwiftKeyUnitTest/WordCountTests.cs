using NUnit.Framework;
using Business_Logic;
using SwiftKey_Logic;
using NUnit.Framework.Legacy;

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

        // ClassicClassicAssert
        ClassicAssert.AreEqual(6, count);
    }

    [Test]
    public void GetWordCount_WithMultipleSpaces_ReturnsCorrectCount()
    {
        // Arrange
        var text = "Hello    world  !  This   is a test.";

        // Act
        var count = OefenschermMethods.GetWordCount(text);

        // ClassicClassicAssert
        ClassicAssert.AreEqual(6, count);
    }

    [Test]
    public void GetWordCount_WithPunctuations_ReturnsCorrectCount()
    {
        // Arrange
        var text = "Hello, world! This, is a test.";

        // Act
        var count = OefenschermMethods.GetWordCount(text);

        // ClassicClassicAssert
        ClassicAssert.AreEqual(6, count);
    }

    [Test]
    public void GetWordCount_WithEmptyString_ReturnsZero()
    {
        // Arrange
        var text = "";

        // Act
        var count = OefenschermMethods.GetWordCount(text);

        // ClassicClassicAssert
        ClassicAssert.AreEqual(0, count);
    }

    [Test]
    public void GetWordCount_WithOnlyPunctuationsAndSpaces_ReturnsZero()
    {
        // Arrange
        var text = " , ! ? . ,";

        // Act
        var count = OefenschermMethods.GetWordCount(text);

        // ClassicAssert
        ClassicAssert.AreEqual(0, count);
        
    }

    [Test]
    public void GetWordCount_WithNull_ThrowsArgumentNullException()
    {
        // Arrange
        string text = null;

        // Act & ClassicAssert
        var ex = ClassicAssert.Throws<System.ArgumentNullException>(() => OefenschermMethods.GetWordCount(text));
        ClassicAssert.That(ex.ParamName, Is.EqualTo("text"));
    }
}

