using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ClubTest
{
    public class Bullet : MonoBehaviour
    {
        private float _damage;
        private float _speed;
        private readonly float _lifeTime = CONSTANTS.BULLET_LIFE_TIME_IN_SECONDS;

        public void Init(float damage, float speed)
        {
            _damage = damage;
            _speed = speed;
        }

        public async UniTaskVoid Move(Vector2 direction)
        {
            var leftLifeTime = _lifeTime;
            while (leftLifeTime > 0)
            {
                transform.Translate(direction * _speed * Time.fixedDeltaTime);
                leftLifeTime -= Time.fixedDeltaTime;
                await UniTask.NextFrame(PlayerLoopTiming.FixedUpdate, this.GetCancellationTokenOnDestroy());
            }
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Enemy target) == false)
            {
                Destroy(gameObject);
                return;
            }

            target.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}