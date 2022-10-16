using Engine.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Interfaces.DataCollector
{
    public interface IGroupStatsCollector
    {
        public delegate List<ItemStats> SearchGoupStats();        
        public SearchGoupStats GetStats { get; }
        public List<ItemStats> Stats => GetStats();
    }
}
