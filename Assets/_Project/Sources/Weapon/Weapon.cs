using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace ClubTest
{
    public class Weapon
    {
        private readonly int _inventoryCellId;
        private readonly WeaponStats _weaponStats;
        private readonly WeaponView _weaponView;
        private bool _isOnAttackDelay;

        public Weapon(int inventoryCellId, WeaponStats weaponStats, WeaponView weaponView)
        {
            _inventoryCellId = inventoryCellId;
            _weaponStats = weaponStats;
            _weaponView = weaponView;
        }

        public int InventoryCellId => _inventoryCellId;

        public BulletCaliber BulletCaliber => _weaponStats.Caliber;
        public int BulletsPerShoot => _weaponStats.BulletsPerShoot;

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