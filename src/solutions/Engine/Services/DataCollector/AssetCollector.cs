using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.BO;
using Engine.Interfaces.DataCollector;

namespace Engine.Services.DataCollector
{
    public class AssetCollector : IAssetCollector
    {
        public static IAssetCollector.SearchAsset? SearchAsset { get; set; }
        public static IAssetCollector Instance(int assetId) => new AssetCollector(SearchAsset, assetId);

        public int AssetId { get; set; }
        public IAssetCollector.SearchAsset? GetAsset { get; }

        private AssetCollector(IAssetCollector.SearchAsset? callback, int assetId)
        {
            AssetId = assetId;
            GetAsset = callback;
        }
    }
}
