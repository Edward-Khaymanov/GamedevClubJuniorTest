using System;
using UnityEngine;

namespace ClubTest
{
    [Serializable]
    public class WeaponStats
    {
        [field: SerializeField] public float AttackDelayInSeconds { get; set; }
        [field: SerializeField] public float BulletSpeed { get; set; }
        [field: SerializeField] public float Damage { get; set; }
        [field: SerializeField] public int BulletsPerShoot { get; set; }
        [field: SerializeField] public BulletCaliber Caliber { get; set; }
    }
}