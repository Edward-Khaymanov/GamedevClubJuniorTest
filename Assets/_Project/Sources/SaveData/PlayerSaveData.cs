using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClubTest
{
    [Serializable]
    [JsonObject(MemberSerialization.OptOut)]
    public struct PlayerSaveData
    {
        public PlayerSaveData(
            int? equipedWeaponInventoryItemId,
            Vector2 position,
            UnitStats stats,
            IEnumerable<InventoryItem> inventoryItems)
        {
            EquipedWeaponInventoryItemId = equipedWeaponInventoryItemId;
            PositionX = position.x;
            PositionY = position.y;
            Stats = stats;
            InventoryItems = new List<InventoryItemSaveData>();

            foreach (var inventoryItem in inventoryItems)
            {
                var saveItem = new InventoryItemSaveData()
                {
                    Id = inventoryItem.Id,
                    Amount = inventoryItem.Amount,
                    AssetId = inventoryItem.Asset.Id,
                };
                InventoryItems.Add(saveItem);
            }
        }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public int? EquipedWeaponInventoryItemId { get; set; }
        public UnitStats Stats { get; set; }
        public List<InventoryItemSaveData> InventoryItems { get; set; }
        [JsonIgnore] public Vector2 Position => new Vector2(PositionX, PositionY);
    }
}