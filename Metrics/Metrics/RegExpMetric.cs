using System.Text.RegularExpressions;

namespace Metrics.Metrics
{
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
}