using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Metrics
{
    public abstract class BaseMetric
    {
    }


    public class RegExpMetric : BaseMetric
    {
        public RegExpMetric()
        {
        }

        public string pattern;
        public Regex regex;

        public Regex Regexpr
        {
            get { return regex; }
            set { regex = new Regex(pattern); }
        }

        public RegExpMetric(string queryMes)
        {
            pattern = queryMes;
            regex = new Regex(pattern);
        }

        public virtual bool IsExist(string text)
        {
            return Regexpr.IsMatch(text);
        }

        public virtual float count(string text)
        {
            return Regexpr.Matches(text).Count;
        }
    }


    public class EntityMetric : RegExpMetric
    {
        public EntityMetric()
        {
        }

        protected Dictionary<string, string> entDict = new Dictionary<string, string>
        {
            {"character", "RegExpPattern1"},
            {"word", "RegExpPattern2"},
            {"sentence", "RegExpPattern2"}
        };

        public EntityMetric(string queryMes)
        {
            if (entDict.ContainsKey(queryMes))
                this.pattern = entDict[queryMes];
            else pattern = queryMes;
        }
    }

    public class SpecialMetric : EntityMetric
    {
        Dictionary<string, string> specDict = new Dictionary<string, string>
        {
            {"exclamatory Sen", "RegExpPattern111"},
            {"quest Sen", "RegExpPattern112"},
            {"ord Sent", "RegExpPattern113"},
            {"number Char", "RegExpPattern131"},
            {"letter Char", "RegExpPattern132"},
            {"spec Char", "RegExpPattern133"}
        };

        public SpecialMetric()
        {
        }

        public SpecialMetric(string queryMes)
        {
            if (specDict.ContainsKey(queryMes))
                pattern = specDict[queryMes];
            else if (entDict.ContainsKey(queryMes))
                this.pattern = entDict[queryMes];
            else pattern = queryMes;
        }

        private string GetBaseEnity(string query)
        {
            return query.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1];
        }


        public Regex getBaseRegexpr(string queryMes)
        {
            string baseEntity = GetBaseEnity(queryMes);
            string basePattern = specDict[baseEntity];
            return new Regex(basePattern);
        }


        public float dense(string text)
        {
            return count(text) / getBaseRegexpr(pattern).Matches(text).Count;
        }
    }


    public class SpecialDictionaryMetric : SpecialMetric
    {
        private List<string> Nouns = Metrics.Dictionary.Nouns;

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


    public class MostFreqMetric : BaseMetric
    {
        public  char[] GetMostFrequentChar(string str)
        {
            Dictionary<char, int> result = str.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
            return result.Where(x => x.Value == result.Values.Max()).Select(x => x.Key).ToArray();
        }
    }









}
