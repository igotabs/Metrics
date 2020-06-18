using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Metrics.Metrics
{
    public class SpecialDictionaryMetric : SpecialMetric
    {
        private List<string> Nouns = global::Metrics.Dictionary.Nouns;

        public override float count(string text)
        {
            return CountNounWords(text);
        }

        private float CountNounWords(string text)
        {
            var words = GetWords(text);
            var count = 0;
            foreach (var word in words)
            {
                if (Dictionary.Nouns.Any(a => a.Equals(word))) count++;
            }

            return count;
        }

        private static List<string> GetWords(string text)
        {
            var parsed = Regex.Replace(text, @"[^\w\s]", "");

            var words = parsed.Split(" ").ToList();
            return words;
        }
    }
}