using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ClubTest
{
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public class PlayerSaveData
    {
        public UnitStats Stats { get; set; }
        public List<InventoryCellData> Inventory { get; set; }
        public List<int> EquipedItemsInventoryId { get; set; }
    }
}