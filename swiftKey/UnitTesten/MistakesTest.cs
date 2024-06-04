using NUnit.Framework;
using Business_Logic;
using SwiftKey_Logic;
using NUnit.Framework;

[TestFixture]
public class Mistakes
{
    [Test]
    public void GetMistakeCount_CapitalLetter_OneMistake()
    {
        // Arrange
        var text = "Hello, world! This is a test.";
        var wrong_text = "hello, world! This is a test.";

        // Act
        int count = ResultaatschermMethods.calculateMistakes(wrong_text, text);

        // ClassicClassicAssert
        Assert.AreEqual(1, count);
    }
    [Test]
    public void GetMistakeCount_ToManyLetters_ZeroMistakes()
    {
        // Arrange
        var text = "Hello, world! This is a test.";
        var wrong_text = "Hello, world! This is a test. I am not supposed to be here!";

        // Act
        int count = ResultaatschermMethods.calculateMistakes(wrong_text, text);

        // ClassicClassicAssert
        Assert.AreEqual(0, count);
    }
    [Test]
    public void GetMistakeCount_WrongWord_FourMistakes()
    {
        // Arrange
        var text = "Hello, world! This is a test.";
        var wrong_text = "Hello, earth! This is a test.";

        // Act
        int count = ResultaatschermMethods.calculateMistakes(wrong_text, text);

        // ClassicClassicAssert
        Assert.AreEqual(4, count);
    }
    [Test]
    public void GetMistakeCount_CompletelyWrongSentence_9Mistakes()
    {
        // Arrange
        var text = "Hello, world! This is a test.";
        var wrong_text = "123456789";

        // Act
        int count = ResultaatschermMethods.calculateMistakes(wrong_text, text);

        // ClassicClassicAssert
        Assert.AreEqual(9, count);
    }
    [Test]
    public void GetMistakeCount_OneLetter_0Mistakes()
    {
        // Arrange
        var text = "Hello, world! This is a test.";
        var wrong_text = "H";

        // Act
        int count = ResultaatschermMethods.calculateMistakes(wrong_text, text);

        // ClassicClassicAssert
        Assert.AreEqual(0, count);
    }
    [Test]
    public void GetMistakeCount_OneLetter_1Mistake()
    {
        // Arrange
        var text = "Hello, world! This is a test.";
        var wrong_text = "h";

        // Act
        int count = ResultaatschermMethods.calculateMistakes(wrong_text, text);

        // ClassicClassicAssert
        Assert.AreEqual(1, count);
    }
}