using UnityEngine;

namespace ClubTest
{
    [SelectionBase]
    public abstract class Unit : MonoBehaviour, IDamageable
    {
        [field: SerializeField] protected ProgressBar HealfBar { get; private set; }
        [field: SerializeField] protected Rigidbody2D Rigidbody { get; private set; }

        public Vector2 Position => Rigidbody.position;

        public abstract void TakeDamage(float damage);
        protected virtual void Move(Vector2 direction)
        {
            Rigidbody.MovePosition(Rigidbody.position + direction);
        }
        protected abstract void Die();

        protected virtual void OnHealfChanged(float current, float max)
        {
            HealfBar.Fill(current, max);
            if (current <= 0)
                Die();
        }
    }
}