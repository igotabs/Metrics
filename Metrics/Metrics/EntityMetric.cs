using System.Collections.Generic;

namespace Metrics.Metrics
{
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
}