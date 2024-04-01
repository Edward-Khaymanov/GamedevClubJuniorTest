using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

namespace ClubTest
{

    public class Flesh : Enemy
    {
        private Player _target;
        private UnitDelector _unitDetector;
        private EnemyState _currentState;
        private Vector2 _targetDirectionNormalized;
        private float _currentAttackCooldown;
        private float _currentAttackTime;

        public override void Init(EnemyType enemyType)
        {
            base.Init(enemyType);
            _unitDetector = new UnitDelector();
            Data.UnitStats.Healf.HealfChanged += OnHealfChanged;
        }

        private void OnDestroy()
        {
            Data.UnitStats.Healf.HealfChanged -= OnHealfChanged;
        }

        private void Update()
        {
            if (_currentAttackCooldown > 0)
            {
                _currentAttackCooldown -= Time.deltaTime;
            }

            if (_currentAttackTime > 0)
            {
                _currentAttackTime -= Time.deltaTime;
            }

            switch (_currentState)
            {
                case EnemyState.Idle:
                    IdleStateUpdate();
                    break;
                case EnemyState.Follow:
                    FollowStateUpdate();
                    break;
                case EnemyState.Attack:
                    AttackStateUpdate();
                    break;
            }
        }

        private void FixedUpdate()
        {
            if (_currentState == EnemyState.Follow && _targetDirectionNormalized != Vector2.zero)
            {
                var moveDirection = Data.UnitStats.MoveSpeed * Time.fixedDeltaTime * _targetDirectionNormalized;
                HandleRotation(moveDirection);
                Move(moveDirection);
            }
        }

        private void IdleStateUpdate()
        {
            _target = _unitDetector.GetComponentAround<Player>(transform.position, Data.UnitStats.FOVDistance, CONSTANTS.PlayerMask);
            if (_target != null)
            {
                _currentState = EnemyState.Follow;
                View.ChangeColor(Color.yellow);
            }
        }

        private void FollowStateUpdate()
        {
            if (_target == null)
            {
                _currentState = EnemyState.Idle;
                return;
            }

            var distanceToTarget = Vector2.Distance(_target.transform.position, transform.position);
            if (distanceToTarget > Data.UnitStats.FOVDistance)
            {
                _currentState = EnemyState.Idle;
                return;
            }

            if (distanceToTarget > Data.AttackDistance)
            {
                _targetDirectionNormalized = (_target.transform.position - transform.position).normalized;
            }
            else
            {
                if (_currentAttackCooldown > 0)
                {
                    _targetDirectionNormalized = Vector2.zero;
                    return;
                }

                _currentState = EnemyState.Attack;
                View.ChangeColor(Color.red);
            }
        }

        private void AttackStateUpdate()
        {
            if (_currentAttackTime > 0)
            {
                _currentAttackTime -= Time.deltaTime;
            }
            else
            {
                Attack(_target);
                View.ChangeColor(Color.white);
                _currentState = EnemyState.Idle;
            }
        }


        private void Attack(IDamageable target)
        {
            target.TakeDamage(Data.Damage);
            _currentAttackCooldown = Data.AttackCooldownInSeconds;
        }

        public override void TakeDamage(float damage)
        {
            Data.UnitStats.Healf.Remove(damage);
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Data.AttackDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, Data.UnitStats.FOVDistance);
            Gizmos.color = Color.white;
        }
    }
}