using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClubTest
{
    [Serializable]
    public class PlayerData
    {
        [field: SerializeField] public UnitStats Stats { get; set; }
        [field: SerializeField] public List<InventoryItem> InventoryItems { get; set; }
        public int? EquipedWeaponInventoryItemId { get; set; }
    }
}