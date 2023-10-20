using UnityEngine;

namespace ClubTest
{
    public class WeaponView : MonoBehaviour
    {
        [SerializeField] private Transform _gunPoint;
        [SerializeField] private Bullet _bulletTemplate;

        public void Shoot(Vector3 targetPosition, float damage, float speed)
        {
            var direction = targetPosition - _gunPoint.position;
            var bullet = Instantiate(_bulletTemplate, _gunPoint.position, Quaternion.identity);
            bullet.Init(damage, speed);
            bullet.Move(direction.normalized).Forget();
        }
    }
}