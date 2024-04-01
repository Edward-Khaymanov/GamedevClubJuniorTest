using Newtonsoft.Json;
using System;

namespace ClubTest
{
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class InventoryCellData
    {
        public int Id { get; set; }
        public int ItemDefinitionId { get; set; }
        public int Amount { get; set; }
    }
}