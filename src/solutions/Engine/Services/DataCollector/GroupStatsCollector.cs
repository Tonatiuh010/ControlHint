using Engine.Interfaces.DataCollector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Services.DataCollector
{
    public class GroupStatsCollector : IGroupStatsCollector
    {
        public static IGroupStatsCollector.SearchGoupStats SearchGoupStats { get; set; } = () => throw new NotImplementedException();
        public static IGroupStatsCollector Instance => new GroupStatsCollector(SearchGoupStats);

        public IGroupStatsCollector.SearchGoupStats GetStats { get; }
        private GroupStatsCollector(IGroupStatsCollector.SearchGoupStats callback)
        {
            GetStats = callback;
        }
    }
}
