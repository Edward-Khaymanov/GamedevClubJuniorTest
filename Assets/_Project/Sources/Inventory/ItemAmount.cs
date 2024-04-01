using System;
using UnityEngine;

namespace ClubTest
{
    [Serializable]
    public class ItemAmount
    {
        [field: SerializeField] public ItemDefinition Item { get; set; }
        [field: SerializeField, Min(1)] public int Amount { get; set; }
    }
}