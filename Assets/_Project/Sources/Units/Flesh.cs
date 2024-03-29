using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace ClubTest
{
    public class Flesh : Enemy
    {
        private bool _isOnAttackDelay;
        private CancellationTokenSource _moveAndAttackTokenSource;
        private CancellationTokenSource _searchTokenSource;
        private Unit _target;
        private UnitDelector _unitDetector;

        public override void Init(EnemyType enemyType)
        {
            base.Init(enemyType);
            _moveAndAttackTokenSource = new CancellationTokenSource();
            _searchTokenSource = new CancellationTokenSource();
            _unitDetector = new UnitDelector();

            Data.UnitStats.Healf.HealfChanged += OnHealfChanged;
            StartSearchPlayer(_searchTokenSource.Token).Forget();
            StartMoveAndAttack(_moveAndAttackTokenSource.Token).Forget();
        }

        private void OnDestroy()
        {
            Data.UnitStats.Healf.HealfChanged -= OnHealfChanged;
            StopMoveAndAttack();
            StopSearchPlayer();
        }

        private void FixedUpdate()
        {
            Rigidbody.velocity = Vector2.zero;
        }

        public override void TakeDamage(float damage)
        {
            Data.UnitStats.Healf.Remove(damage);
        }

        private async UniTask Attack(Unit target)
        {
            if (target == null || _isOnAttackDelay)
                return;

            await UniTask.Delay(TimeSpan.FromSeconds(Data.AttackTimeInSeconds), cancellationToken: this.GetCancellationTokenOnDestroy());
            target.TakeDamage(Data.Damage);
            WaitAttackDelay().Forget();
        }

        private async UniTaskVoid WaitAttackDelay()
        {
            _isOnAttackDelay = true;
            await UniTask.Delay(TimeSpan.FromSeconds(Data.AttackDelayInSeconds), cancellationToken: this.GetCancellationTokenOnDestroy());
            _isOnAttackDelay = false;
        }

        protected override void Move(Vector2 offsetPosition)
        {
            Rigidbody.MovePosition(Rigidbody.position + offsetPosition);
        }

        private async UniTaskVoid StartSearchPlayer(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                _target = _unitDetector.GetComponentAround<Unit>(transform.position, Data.UnitStats.FOVRadius, CONSTANTS.PlayerMask);
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
                if (distance <= Data.AttackDistance)
                {
                    await Attack(_target);
                    await UniTask.NextFrame(cancellationToken);
                    continue;
                }

                var moveDirection = (_target.transform.position - transform.position).normalized;
                var offsetPosition = Data.UnitStats.MoveSpeed * Time.fixedDeltaTime * moveDirection;
                HandleRotation(moveDirection);
                Move(offsetPosition);
                await UniTask.NextFrame(PlayerLoopTiming.FixedUpdate);
            }
        }

        private void HandleRotation(Vector2 moveDirection)
        {
            if (moveDirection.x == 0)
                return;

            var currentRotationYIsPositive = View.transform.rotation.eulerAngles.y == 0;
            var directionIsPositive = moveDirection.x > 0;
            if (currentRotationYIsPositive == directionIsPositive)
                return;

            var targetAngleY = directionIsPositive ? 0f : 180f;
            var currentAngle = View.transform.rotation.eulerAngles;
            View.transform.rotation = Quaternion.Euler(currentAngle.x, targetAngleY, currentAngle.z);
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
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Data.AttackDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, Data.UnitStats.FOVRadius);
            Gizmos.color = Color.white;
        }
    }
}