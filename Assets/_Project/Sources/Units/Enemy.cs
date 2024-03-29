using System;
using UnityEngine;

namespace ClubTest
{
    public abstract class Enemy : Unit
    {
        [field: SerializeField] protected EnemyView View { get; private set; }
        [field: SerializeField] protected Rigidbody2D Rigidbody { get; private set; }
        [field: SerializeField] protected EnemyData Data { get; private set; }

        public EnemyType Type { get; private set; }

        public event Action<Enemy> Died;

        public virtual void Init(EnemyType enemyType)
        {
            Type = enemyType;
        }

        protected override void Die()
        {
            Died?.Invoke(this);
        }
    }
}