using UnityEngine;

namespace ClubTest
{
    public class CameraFollower : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float _smoothSpeed;

        private Transform _target;

        public void SetTarget(Transform target)
        {
            _target = target;
            transform.position = new Vector3(_target.position.x, _target.position.y, transform.position.z);
        }

        private void LateUpdate()
        {
            if (_target == null)
                return;

            var targetPosition = new Vector3(_target.position.x, _target.position.y, transform.position.z);
            var smoothedPosition = Vector3.Lerp(transform.position, targetPosition, _smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}