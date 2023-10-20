using System;
using UnityEngine;

namespace ClubTest
{
    [Serializable]
    public class DropedItem
    {
        [field: SerializeField] public ItemAsset Item { get; set; }
        [field: SerializeField, Min(1)] public int Amount { get; set; }
    }
}