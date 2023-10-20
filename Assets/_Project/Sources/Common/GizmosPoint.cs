using UnityEngine;

namespace ClubTest
{
    public class GizmosPoint : MonoBehaviour
    {
        [SerializeField] private float _radius = 0.5f;
        [SerializeField] private Color _color = Color.red;

        private void OnDrawGizmos()
        {
            Gizmos.color = _color;
            Gizmos.DrawWireSphere(transform.position, _radius);
            Gizmos.color = Color.white;
        }
    }
}
