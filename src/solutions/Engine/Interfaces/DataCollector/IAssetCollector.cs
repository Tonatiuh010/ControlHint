using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.BO;

namespace Engine.Interfaces.DataCollector
{
    public interface IAssetCollector
    {
        public delegate Asset? SearchAsset(int assetId);

        public int AssetId { get; set; }
        public SearchAsset? GetAsset { get; }
        public Asset? Asset => GetAsset?.Invoke(AssetId);
    }
}
