using UnityEngine;

namespace ClubTest
{
    public class DropedItemView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _iconSource;

        private DropedItem _drop;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player) == false)
                return;

            if (_drop != null)
                player.PickUpItem(_drop);

            Destroy(gameObject);
        }

        public void Init(DropedItem drop)
        {
            if (drop == null)
                return;

            _drop = drop;
            _iconSource.sprite = drop.Item.Icon;
        }
    }
}