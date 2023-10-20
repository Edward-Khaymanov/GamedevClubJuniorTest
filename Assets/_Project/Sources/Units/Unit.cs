using UnityEngine;

namespace ClubTest
{
    [SelectionBase]
    public abstract class Unit : MonoBehaviour, IDamageable
    {
        [SerializeField] protected ProgressBar HealfBar;

        public abstract void TakeDamage(float damage);
        protected abstract void Move(Vector2 position);
        protected abstract void Die();

        protected virtual void OnHealfChanged(float current, float max)
        {
            HealfBar.Fill(current, max);
            if (current <= 0)
                Die();
        }
    }
}