using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            if (string.IsNullOrEmpty(text))
            {
                Message = "There is empty text. Repeat enter a text";
                return;
            }

            Message += "Text:"+ System.Environment.NewLine + text;
            Message += System.Environment.NewLine;
            Message += System.Environment.NewLine;
            Message += "Result:";
            Message += System.Environment.NewLine;
            var s = GetMostFrequentChar(text);
            string frequentChars = string.Join(",", s);
            if (frequentChars.EndsWith(",,"))
                frequentChars = frequentChars.Substring(0, frequentChars.Length-1);
            frequentChars = frequentChars.Replace(" ", "<whiteSpace>");
            Message += "Most frequent characters: " + frequentChars;
            Message += System.Environment.NewLine;

            Message += "Number of exclamatory sentences: " + NumberOfTypedSentence(text,'!');
            Message += System.Environment.NewLine;

            Message += "Percent of nouns: " + (CountNounWords(text)/ GetWords(text).Count)*100+"%";
            Message += System.Environment.NewLine;

            Message += "Number of words: " + GetWords(text).Count;
            Message += System.Environment.NewLine;

            Message += "Number of sentences: " + (NumberOfTypedSentence(text, '.')+ NumberOfTypedSentence(text, '?')+ NumberOfTypedSentence(text, '!'));
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
            return str.Any(x => char.IsLetter(x));
        }

        private static List<int> IndexesChar(string s, char type)
        {
            var foundIndexes = new List<int>();
            foundIndexes.Add(0);
            for (int i = s.IndexOf(type); i > -1; i = s.IndexOf(type, i + 1))
            {
                // for loop end when i=-1 ('a' not found)
                foundIndexes.Add(i);
            }

            return foundIndexes;
        }

        public static char[] GetMostFrequentChar(string str)
        {


           // str = new string(str.Where(c => (char.IsLetter(c))).ToArray());


            Dictionary<char, int> result =

                //new string(str.Where(c => (char.IsLetter(c))).ToArray())
                str.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            return result.Where(x => x.Value == result.Values.Max()).Select(x => x.Key).ToArray();
        }
    }
}
