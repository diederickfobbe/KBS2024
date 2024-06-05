
namespace SwiftKey_Logic;

public class OefenschermMethods {
    
    public static int GetWordCount(string text)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text), "Input text cannot be null.");
        }

        return text.Split(new char[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }
    
    public static int CalculateTypingSpeed(int wordCount, double timeTakenInMinutes)
    {
        if (timeTakenInMinutes <= 0)
        {
            throw new ArgumentException("Time taken must be greater than zero.", nameof(timeTakenInMinutes));
        }
        return (int)(wordCount / timeTakenInMinutes);
    }
    public static double CalculateAccuracy(string enteredText, int targetWordCount, string targetText)
    {
        if (string.IsNullOrEmpty(targetText))
        {
            if (targetWordCount != 0)
                throw new ArgumentException("Target word count does not match the actual number of words in target text.");
            return 0.0;
        }

        var targetWords = targetText.Split(new char[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        if (targetWordCount != targetWords.Length)
            throw new ArgumentException("Target word count does not match the actual number of words in target text.");

        if (string.IsNullOrEmpty(enteredText))
            return 0.0;

        var normalizedTargetWords = targetWords.Select(word => word.ToLowerInvariant()).ToList();
        var normalizedEnteredWords = enteredText.Split(new char[] { ' ', '.', ',', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(word => word.ToLowerInvariant()).ToList();

        int correctWords = normalizedTargetWords.Intersect(normalizedEnteredWords).Count();

        if (targetWordCount == 0)
            return 0.0;

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