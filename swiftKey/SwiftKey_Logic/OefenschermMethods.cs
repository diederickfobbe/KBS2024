namespace SwiftKey_Logic;

public class OefenschermMethods {

    public OefenschermMethods()
    {
        
    }
    
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
        // You can add multiple example texts or sentences here and select one randomly
        string[] exampleTexts = new string[]
        {
            "The quick brown fox jumps over the lazy dog.",
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            "To be or not to be, that is the question.",
            "A journey of a thousand miles begins with a single step."
            // Add more example texts as needed
        };

        // Select a random example text
        Random random = new Random();
        return exampleTexts[random.Next(exampleTexts.Length)];
    }
}