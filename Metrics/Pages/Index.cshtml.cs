using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Metrics.Metrics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Metrics.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string Message;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public void OnPost(string text)
        {
            StringBuilder messageBuilder = new StringBuilder();
            if (string.IsNullOrEmpty(text))
            {
                messageBuilder.Append("There is empty text. Repeat enter a text");
                return;
            }

            messageBuilder.Append("Text:" + System.Environment.NewLine + text);
            messageBuilder.Append(System.Environment.NewLine);
            messageBuilder.Append(System.Environment.NewLine);
            messageBuilder.Append("Result:");
            messageBuilder.Append(System.Environment.NewLine);
            var s = new MostFreqMetric().GetMostFrequentChar(text);
            string frequentChars = string.Join(System.Environment.NewLine, s);
            if (frequentChars.EndsWith(",,"))
                frequentChars = frequentChars.Substring(0, frequentChars.Length-1);
            frequentChars = frequentChars.Replace(" ", "<whiteSpace>");
            messageBuilder.Append(("Most frequent characters are (each on new line): " + System.Environment.NewLine + frequentChars));
            messageBuilder.Append(System.Environment.NewLine);

            messageBuilder.Append("Number of exclamatory sentences: " + new SpecialMetric("exclamatory sentences").count(text));
            messageBuilder.Append(System.Environment.NewLine);

            messageBuilder.Append("Percent of nouns: " + new SpecialDictionaryMetric().dense(text)*100+"%");
            messageBuilder.Append(System.Environment.NewLine);

            messageBuilder.Append("Number of words: " + new EntityMetric("word").count(text));
            messageBuilder.Append(System.Environment.NewLine);

            messageBuilder.Append("Number of sentences: " + new EntityMetric("sentense").count(text));
            Message = messageBuilder.ToString();
        }

        private float NumberOfTypedSentence(string text, char type)
        {

            var list = IndexesChar(text, type).ToArray();
            int count=0;
            for (int i = 1; i < list.Length; i++)
            {
                if (IsValid(text.Substring(list[i-1], list[i] - list[i - 1]))) count++;
            }

            return count;
        }

        private float CountNounWords(string text)
        {
            var words = GetWords(text);
            var count = 0;
           foreach (var word in words)
           {
               if(Dictionary.Nouns.Any(a => a.Equals(word)))count++;
           }

           return count;
        }

        private static List<string> GetWords(string text)
        {
            var parsed = Regex.Replace(text, @"[^\w\s]", "");

            var words = parsed.Split(" ").ToList();
            return words;
        }

        private static bool IsValid(String str)
        {
            return str.Any(char.IsLetter);
        }

        private static List<int> IndexesChar(string s, char type)
        {
            var foundIndexes = new List<int>();
            foundIndexes.Add(0);
            for (int i = s.IndexOf(type); i > -1; i = s.IndexOf(type, i + 1))
            {
                foundIndexes.Add(i);
            }

            return foundIndexes;
        }

        public static char[] GetMostFrequentChar(string str)
        {
            Dictionary<char, int> result = str.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            return result.Where(x => x.Value == result.Values.Max()).Select(x => x.Key).ToArray();
        }
    }
}
