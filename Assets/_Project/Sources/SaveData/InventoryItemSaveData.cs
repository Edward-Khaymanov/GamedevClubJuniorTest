using Newtonsoft.Json;
using System;

namespace ClubTest
{
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public struct InventoryItemSaveData
    {
        public int Id { get; set; }
        public int AssetId { get; set; }
        public int Amount { get; set; }
    }
}