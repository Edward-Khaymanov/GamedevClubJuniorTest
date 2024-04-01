using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ClubTest
{
    public class InventoryCellView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _itemIcon;
        [SerializeField] private TMP_Text _amountText;

        public event Action<int, PointerEventData> Clicked;

        public int CellId { get; private set; }
        public int Amount { get; private set; }
        public IEnumerable<ContextMenuOptionType> ContextMenuOptions { get; private set; }

        public void Render(InventoryCell cell)
        {
            if (cell.Amount <= 0)
                return;

            CellId = cell.Id;
            Amount = cell.Amount;
            ContextMenuOptions = cell.GetContextMenuOptions();
            _itemIcon.sprite = cell.ItemDefinition.Icon;
            _amountText.text = cell.Amount == 1 ? string.Empty : cell.Amount.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke(CellId, eventData);
        }
    }
}