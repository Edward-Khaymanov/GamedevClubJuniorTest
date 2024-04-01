using System;
using UnityEngine;

namespace ClubTest
{
    public class DropedItemView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _iconSource;

        private ItemAmount _drop;

        public event Action<DropedItemView, ItemAmount> PickedUp;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player) == false)
                return;

            PickedUp?.Invoke(this, _drop);
        }

        public void Init(ItemAmount drop)
        {
            _drop = drop;
            _iconSource.sprite = drop.Item.Icon;
        }
    }
}