using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ClubTest
{
    public class Player : Unit
    {
        [SerializeField] private PlayerView _view;

        private UnitStats _stats;
        private CancellationTokenSource _attackTokenSource;
        private IPlayerInput _input;
        private Unit _target;
        private UnitDelector _unitDetector;
        private Weapon _equipedWeapon;
        private Inventory _inventory;

        public event Action<Player> Died;

        private void Awake()
        {
            _unitDetector = new UnitDelector();
            _attackTokenSource = new CancellationTokenSource();
        }

        public void Init(PlayerSaveData playerData, Inventory inventory, IPlayerInput input)
        {
            _stats = playerData.Stats;
            _inventory = inventory;
            _input = input;

            if (playerData.EquipedItemsInventoryId.Count > 0)
            {
                foreach (var cellId in playerData.EquipedItemsInventoryId)
                {
                    EquipItem(cellId);
                }
            }

            _input.Shoot += OnShoot;
            _stats.Healf.HealfChanged += OnHealfChanged;
            _inventory.CellRemoved += RemoveItem;
        }

        private void OnDestroy()
        {
            _input.Shoot -= OnShoot;
            _stats.Healf.HealfChanged -= OnHealfChanged;
            _inventory.CellRemoved -= RemoveItem;
        }

        private void Update()
        {
            var enemies = _unitDetector.GetComponentsAround<Enemy>(transform.position, _stats.FOVDistance, CONSTANTS.EnemyMask).ToList();
            _target = enemies.GetClosestToPoint(transform.position);

            if (_target != null)
                RotateToDirection(_target.Position - Position);
        }

        private void FixedUpdate()
        {
            var moveDirection = _input.MoveDirection;
            if (moveDirection == Vector2.zero)
                return;

            Move(_stats.MoveSpeed * Time.fixedDeltaTime * moveDirection);

            if (_target == null)
                RotateToDirection(moveDirection);
        }

        public void EquipItem(int invetoryCellId)
        {
            var item = _inventory.Cells.FirstOrDefault(x => x.Id == invetoryCellId);
            if (item == null)
                return;

            if (item.ItemDefinition is WeaponDefinition)
            {
                RemoveWeapon();
                EquipWeapon(item);
                return;
            }
        }

        public void RemoveItem(int inventoryCellId)
        {
            if (_equipedWeapon?.InventoryCellId == inventoryCellId)
                RemoveWeapon();
        }

        public override void TakeDamage(float damage)
        {
            _stats.Healf.Remove(damage);
        }

        public PlayerSaveData GetSaveData()
        {
            var result = new PlayerSaveData()
            {
                Stats = _stats,
                Inventory = _inventory.Cells.Select(x =>
                    new InventoryCellData()
                    {
                        Id = x.Id,
                        Amount = x.Amount,
                        ItemDefinitionId = x.ItemDefinition.Id
                    })
                .ToList(),
                EquipedItemsInventoryId = new List<int>()
            };

            if (_equipedWeapon != null)
                result.EquipedItemsInventoryId.Add(_equipedWeapon.InventoryCellId);

            return result;
        }

        protected override void Die()
        {
            RemoveWeapon();
            Died?.Invoke(this);
        }

        private void EquipWeapon(InventoryCell weaponItem)
        {
            var asset = weaponItem.ItemDefinition as WeaponDefinition;
            var view = Instantiate(asset.View);
            _view.EquipWeapon(view);
            _equipedWeapon = new Weapon(weaponItem.Id, asset.Stats, view);
        }

        private void RemoveWeapon()
        {
            StopAttack();
            _equipedWeapon = null;
            _view.RemoveEquipedWeapon();
        }

        private void RotateToDirection(Vector2 direction)
        {
            if (direction.x == 0)
                return;

            var currentRotationYIsPositive = _view.transform.rotation.eulerAngles.y == 0;
            var directionIsPositive = direction.x > 0;
            if (currentRotationYIsPositive == directionIsPositive)
                return;

            var targetAngleY = directionIsPositive ? 0f : 180f;
            var currentAngle = _view.transform.rotation.eulerAngles;
            _view.transform.rotation = Quaternion.Euler(currentAngle.x, targetAngleY, currentAngle.z);
        }

        private async UniTaskVoid StartAttack(CancellationToken cancellationToken)
        {
            Func<InventoryCell, bool> predicate = (cell) =>
                cell.ItemDefinition is BulletDefinition bullet &&
                bullet.Caliber == _equipedWeapon.BulletCaliber;

            while (cancellationToken.IsCancellationRequested == false)
            {
                if (_equipedWeapon == null || _target == null)
                {
                    await UniTask.NextFrame();
                    continue;
                }

                var bulletCells = _inventory.Cells.Where(predicate);

                if (bulletCells.Any() == false || bulletCells.Sum(x => x.Amount) < _equipedWeapon.BulletsPerShoot)
                {
                    await UniTask.NextFrame();
                    continue;
                }

                var isShooted = await _equipedWeapon.TryShoot(_target.transform.position);
                if (isShooted)
                {
                    _inventory.Remove(predicate, _equipedWeapon.BulletsPerShoot);
                }

                await UniTask.NextFrame();
            }
        }

        private void StopAttack()
        {
            _attackTokenSource.Cancel();
            _attackTokenSource = new CancellationTokenSource();
        }

        private void OnShoot(InputAction.CallbackContext context)
        {
            if (context.started)
                StartAttack(_attackTokenSource.Token).Forget();
            if (context.canceled)
                StopAttack();
        }

        private void OnDrawGizmos()
        {
            if (_stats == null) 
                return;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _stats.FOVDistance);
            Gizmos.color = Color.white;
        }
    }
}