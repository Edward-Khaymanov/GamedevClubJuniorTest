using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClubTest
{
    public class InventoryItemView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TMP_Text _amountText;

        public event Action<int, Vector2> ItemClicked;

        public int ItemId { get; private set; }

        public void Render(int id, Sprite icon, int amount)
        {
            if (amount <= 0)
                return;

            ItemId = id;
            _itemIcon.sprite = icon;
            _amountText.text = amount == 1 ? string.Empty : amount.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ItemClicked?.Invoke(ItemId, eventData.position);
        }
    }
}