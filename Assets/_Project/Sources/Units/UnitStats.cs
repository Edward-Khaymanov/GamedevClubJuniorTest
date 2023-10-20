using System;
using UnityEngine;

namespace ClubTest
{
    [Serializable]
    public class UnitStats
    {
        [field: SerializeField] public Healf Healf { get; set; }
        [field: SerializeField, Min(0)] public float FOVRadius { get; set; }
        [field: SerializeField, Min(0)] public float MoveSpeed { get; set; }
    }
}