using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Metrics.Metrics
{
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
}