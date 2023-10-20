using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace ClubTest
{
    public class Weapon
    {
        private readonly int _inventoryItemId;
        private readonly WeaponStats _weaponStats;
        private readonly WeaponView _weaponView;
        private bool _isOnAttackDelay;

        public Weapon(int itemId, WeaponStats weaponStats, WeaponView weaponView)
        {
            _inventoryItemId = itemId;
            _weaponStats = weaponStats;
            _weaponView = weaponView;
        }

        public int InventoryItemId => _inventoryItemId;

        public BulletCaliber BulletCaliber => _weaponStats.Caliber;

        public async UniTask<bool> TryShoot(Vector3 targetPosition)
        {
            if (_isOnAttackDelay)
                return false;

            _weaponView.Shoot(targetPosition, _weaponStats.Damage, _weaponStats.BulletSpeed);
            _isOnAttackDelay = true;
            await UniTask.Delay(TimeSpan.FromSeconds(_weaponStats.AttackDelayInSeconds));
            _isOnAttackDelay = false;
            return true;
        }
    }
}