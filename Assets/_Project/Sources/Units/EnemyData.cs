using System;
using UnityEngine;

namespace ClubTest
{
    [Serializable]
    public class EnemyData
    {
        [field: SerializeField, Min(0)] public float AttackTimeInSeconds { get; set; }
        [field: SerializeField, Min(0)] public float AttackCooldownInSeconds { get; set; }
        [field: SerializeField, Min(0)] public float AttackDistance { get; set; }
        [field: SerializeField, Min(0)] public float Damage { get; set; }
        [field: SerializeField] public UnitStats UnitStats { get; set; }
    }
}