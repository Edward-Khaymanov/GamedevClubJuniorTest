using Cysharp.Threading.Tasks;
using System.Linq;
using System.Threading;
using UnityEngine;
using Zenject;

namespace ClubTest
{
    public class Player : Unit
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private PlayerView _view;
        [SerializeField] private UnitStats _stats;

        private CancellationTokenSource _attackTokenSource;
        private CancellationTokenSource _searchTokenSource;
        private Inventory _inventory;
        private SharedInput _input;
        private Unit _target;
        private UnitDelector _unitDetector;
        private Weapon _equipedWeapon;
        private InventoryContextMenu _inventoryContextMenu;

        [Inject]
        private void Construct(InventoryView inventoryView, InventoryContextMenu inventoryContextMenu)
        {
            _inventoryContextMenu = inventoryContextMenu;
            _inventory = new Inventory(inventoryView, inventoryContextMenu);
            _unitDetector = new UnitDelector();
            _input = new SharedInput();
            _attackTokenSource = new CancellationTokenSource();
            _searchTokenSource = new CancellationTokenSource();
        }

        public void Init(PlayerData data)
        {
            _stats = data.Stats;
            _inventory.Init(data.InventoryItems);
            _stats.Healf.HealfChanged += OnHealfChanged;
            if (data.EquipedWeaponInventoryItemId.HasValue)
                EquipItem(data.EquipedWeaponInventoryItemId.Value);
        }

        private void OnEnable()
        {
            _input.Enable();
            _input.Player.Shoot.started += (ctx) => StartAttack(_attackTokenSource.Token).Forget();
            _input.Player.Shoot.canceled += (ctx) => StopAttack();
            _inventoryContextMenu.EquipRequested += EquipItem;
            _inventoryContextMenu.DeleteRequested += OnDeleteRequested;
            StartSearchEnemies(_searchTokenSource.Token).Forget();
        }

        private void OnDisable()
        {
            _input.Disable();
            _input.Player.Shoot.started -= (ctx) => StartAttack(_attackTokenSource.Token).Forget();
            _input.Player.Shoot.canceled -= (ctx) => StopAttack();
            _inventoryContextMenu.EquipRequested -= EquipItem;
            _inventoryContextMenu.DeleteRequested -= OnDeleteRequested;
            StopSearchEnemies();
        }

        private void OnDestroy()
        {
            _stats.Healf.HealfChanged -= OnHealfChanged;
            _inventory.Dispose();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _stats.FOVRadius);
            Gizmos.color = Color.white;
        }

        private void FixedUpdate()
        {
            var direction = _input.Player.Move.ReadValue<Vector2>();
            if (direction == Vector2.zero)
                return;

            Move(_stats.MoveSpeed * Time.fixedDeltaTime * direction);
            HandleRotation(direction);
            _rigidbody.velocity = Vector2.zero;
        }

        public PlayerSaveData GetSaveData()
        {
            return new PlayerSaveData(_equipedWeapon?.InventoryItemId, transform.position, _stats, _inventory.Items);
        }

        public void EquipItem(int invetoryItemId)
        {
            var item = _inventory.Items.FirstOrDefault(x => x.Id == invetoryItemId);
            if (item == null)
                return;

            if (item.Asset is WeaponAsset)
            {
                RemoveWeapon();
                EquipWeapon(item);
                return;
            }
        }

        public void PickUpItem(DropedItem dropedItem)
        {
            _inventory.Add(dropedItem.Item, dropedItem.Amount);
        }

        public override void TakeDamage(float damage)
        {
            _stats.Healf.Remove(damage);
        }

        protected override void Die()
        {
            Destroy(gameObject);
        }

        protected override void Move(Vector2 offsetPosition)
        {
            _rigidbody.MovePosition(_rigidbody.position + offsetPosition);
        }

        private void EquipWeapon(InventoryItem weaponItem)
        {
            var asset = weaponItem.Asset as WeaponAsset;
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

        private void HandleRotation(Vector2 moveDirection)
        {
            if (moveDirection.x == 0)
                return;

            var currentRotationYIsPositive = _view.transform.rotation.eulerAngles.y == 0;
            var directionIsPositive = moveDirection.x > 0;
            if (currentRotationYIsPositive == directionIsPositive)
                return;

            var targetAngleY = directionIsPositive ? 0f : 180f;
            var currentAngle = _view.transform.rotation.eulerAngles;
            _view.transform.rotation = Quaternion.Euler(currentAngle.x, targetAngleY, currentAngle.z);
        }

        private async UniTaskVoid StartSearchEnemies(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                var enemies = _unitDetector.GetComponentsAround<Enemy>(transform.position, _stats.FOVRadius, CONSTANTS.EnemyMask).ToList();
                _target = enemies.GetClosestToPoint(transform.position);
                await UniTask.Delay(CONSTANTS.UNIT_DETECTION_INTERVAL, cancellationToken: cancellationToken);
            }
        }

        private async UniTaskVoid StartAttack(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                if (_equipedWeapon == null || _target == null)
                {
                    await UniTask.NextFrame();
                    continue;
                }

                var inventoryBullet = _inventory.Items.FirstOrDefault(x =>
                    x.Asset is BulletAsset asset &&
                    asset.Caliber == _equipedWeapon.BulletCaliber &&
                    x.Amount > 0);

                if (inventoryBullet == null)
                {
                    await UniTask.NextFrame();
                    continue;
                }

                var isShooted = await _equipedWeapon.TryShoot(_target.transform.position);
                if (isShooted)
                {
                    _inventory.Remove(inventoryBullet.Id, 1);
                }

                await UniTask.NextFrame();
            }
        }

        private void StopSearchEnemies()
        {
            _searchTokenSource.Cancel();
            _searchTokenSource = new CancellationTokenSource();
        }

        private void StopAttack()
        {
            _attackTokenSource.Cancel();
            _attackTokenSource = new CancellationTokenSource();
        }

        private void OnDeleteRequested(int itemId, int amount)
        {
            if (_equipedWeapon?.InventoryItemId == itemId)
                RemoveWeapon();
        }
    }
}