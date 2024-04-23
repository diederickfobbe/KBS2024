
namespace SwiftKey_Logic;

public class OefenschermMethods {
    
    public static int GetWordCount(string text)
    {
        return text.Split(new char[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }
    
    public static int CalculateTypingSpeed(int wordCount, double timeTakenInMinutes)
    {
        return (int)(wordCount / timeTakenInMinutes);
    }
    
    public static double CalculateAccuracy(string enteredText, int targetWordCount, string targetText)
    {
        int correctWords = targetText.Split(new char[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
            .Intersect(enteredText.Split(new char[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries))
            .Count();
        return (double)correctWords / targetWordCount * 100;
    }
    
    public static string GenerateNewTargetText()
    {
        string[] exampleTexts = new string[]
        {
            "The quick brown fox jumps over the lazy dog.",
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            "To be or not to be, that is the question.",
            "A journey of a thousand miles begins with a single step."
        };

        Random random = new Random();
        return exampleTexts[random.Next(exampleTexts.Length)];
    }
}