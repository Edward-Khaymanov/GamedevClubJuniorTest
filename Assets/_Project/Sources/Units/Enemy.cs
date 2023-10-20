using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using Zenject;

namespace ClubTest
{
    public class Enemy : Unit
    {
        [SerializeField] private EnemyView _view;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private EnemyData _data;

        private bool _isOnAttackDelay;
        private CancellationTokenSource _moveAndAttackTokenSource;
        private CancellationTokenSource _searchTokenSource;
        private DropedItemView _dropedItemViewTemplate;
        private Unit _target;
        private UnitDelector _unitDetector;

        [Inject]
        private void Construct(DropedItemView dropedItemViewTemplate)
        {
            _dropedItemViewTemplate = dropedItemViewTemplate;
            _moveAndAttackTokenSource = new CancellationTokenSource();
            _searchTokenSource = new CancellationTokenSource();
            _unitDetector = new UnitDelector();
        }

        private void OnEnable()
        {
            _data.UnitStats.Healf.HealfChanged += OnHealfChanged;
            StartSearchPlayer(_searchTokenSource.Token).Forget();
            StartMoveAndAttack(_moveAndAttackTokenSource.Token).Forget();
        }

        private void OnDisable()
        {
            _data.UnitStats.Healf.HealfChanged -= OnHealfChanged;
            StopMoveAndAttack();
            StopSearchPlayer();
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = Vector2.zero;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _data.AttackDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _data.UnitStats.FOVRadius);
            Gizmos.color = Color.white;
        }

        public override void TakeDamage(float damage)
        {
            _data.UnitStats.Healf.Remove(damage);
        }

        private async UniTask Attack(Unit target)
        {
            if (target == null || _isOnAttackDelay)
                return;

            await UniTask.Delay(TimeSpan.FromSeconds(_data.AttackTimeInSeconds), cancellationToken: this.GetCancellationTokenOnDestroy());
            target.TakeDamage(_data.Damage);
            WaitAttackDelay().Forget();
        }

        private async UniTaskVoid WaitAttackDelay()
        {
            _isOnAttackDelay = true;
            await UniTask.Delay(TimeSpan.FromSeconds(_data.AttackDelayInSeconds), cancellationToken: this.GetCancellationTokenOnDestroy());
            _isOnAttackDelay = false;
        }

        protected override void Die()
        {
            DropItems();
            Destroy(gameObject);
        }

        protected override void Move(Vector2 offsetPosition)
        {
            _rigidbody.MovePosition(_rigidbody.position + offsetPosition);
        }

        private void DropItems()
        {
            var view = Instantiate(_dropedItemViewTemplate, transform.position, Quaternion.identity);
            view.Init(_data.DropList.RandomSingle());
        }

        private async UniTaskVoid StartSearchPlayer(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                _target = _unitDetector.GetComponentAround<Unit>(transform.position, _data.UnitStats.FOVRadius, CONSTANTS.PlayerMask);
                await UniTask.Delay(CONSTANTS.UNIT_DETECTION_INTERVAL, cancellationToken: cancellationToken);
            }
        }

        private async UniTaskVoid StartMoveAndAttack(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                if (_target == null)
                {
                    await UniTask.NextFrame(cancellationToken);
                    continue;
                }

                var distance = Vector2.Distance(transform.position, _target.transform.position);
                if (distance <= _data.AttackDistance)
                {
                    await Attack(_target);
                    await UniTask.NextFrame(cancellationToken);
                    continue;
                }

                var moveDirection = (_target.transform.position - transform.position).normalized;
                var offsetPosition = _data.UnitStats.MoveSpeed * Time.fixedDeltaTime * moveDirection;
                HandleRotation(moveDirection);
                Move(offsetPosition);
                await UniTask.NextFrame(PlayerLoopTiming.FixedUpdate);
            }
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

        private void StopMoveAndAttack()
        {
            _moveAndAttackTokenSource.Cancel();
            _moveAndAttackTokenSource = new CancellationTokenSource();
        }

        private void StopSearchPlayer()
        {
            _searchTokenSource.Cancel();
            _searchTokenSource = new CancellationTokenSource();
        }
    }
}