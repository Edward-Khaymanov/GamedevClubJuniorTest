using UnityEngine;

namespace ClubTest
{
    public class DrawPivot : MonoBehaviour
    {
        [SerializeField] private Color _color = Color.red;
        [SerializeField] private float _radius = 0.1f;

        private void OnDrawGizmos()
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(transform.position, _radius);
            Gizmos.color = Color.white;
        }
    }
}
