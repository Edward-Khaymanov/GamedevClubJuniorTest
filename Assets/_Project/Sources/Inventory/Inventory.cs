using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ClubTest
{
    [Serializable]
    public class Inventory : IDisposable
    {
        private readonly InventoryView _view;
        private readonly List<InventoryItem> _items;
        private readonly InventoryContextMenu _itemContextMenu;

        public Inventory(InventoryView view, InventoryContextMenu inventoryContextMenu)
        {
            _view = view;
            _items = new List<InventoryItem>();
            _itemContextMenu = inventoryContextMenu;
            _itemContextMenu.DeleteRequested += Remove;
        }

        private int NewId => _items.Count == 0 ? 0 : _items.Max(x => x.Id) + 1;
        public IReadOnlyList<InventoryItem> Items => _items;


        public void Dispose()
        {
            _itemContextMenu.DeleteRequested -= Remove;
            foreach (var item in _items)
            {
                _view.ItemViews.FirstOrDefault(x => x.ItemId == item.Id).ItemClicked -= ShowContextMenu;
            }
        }

        public void Init(List<InventoryItem> items)
        {
            var sortedItems = items.OrderBy(x => x.Id);
            foreach (var item in sortedItems)
            {
                Add(item.Asset, item.Amount);
            }
        }

        public void Add(ItemAsset asset, int amount)
        {
            if (amount < 1)
                return;

            var amountLeft = amount;
            var existedItems = _items
                .Where(x => x.Asset.Id == asset.Id && x.Asset.MaxInStack >= x.Amount + 1);

            if (existedItems.Any())
            {
                foreach (var existedItem in existedItems)
                {
                    var amountToAdd = existedItem.Asset.MaxInStack - existedItem.Amount;
                    amountToAdd = Math.Clamp(amountToAdd, 0, amountLeft);
                    existedItem.Add(amountToAdd);
                    amountLeft -= amountToAdd;
                    _view.UpdateView(existedItem);

                    if (amountLeft == 0)
                        return;
                }
            }

            var newItems = CreateItemsByStackSize(asset, amountLeft);
            _items.AddRange(newItems);

            foreach (var item in newItems)
            {
                var itemView = _view.Add(item.Id, item.Asset.Icon, item.Amount);
                itemView.ItemClicked += ShowContextMenu;
            }
        }

        public void Remove(int itemId, int amount)
        {
            if (amount < 0)
                return;

            var item = _items.FirstOrDefault(x => x.Id == itemId);
            if (item == null)
                return;

            item.Remove(amount);

            if (item.Amount == 0)
            {
                _items.Remove(item);
                _view.Remove(item.Id);
            }
            else
            {
                _view.UpdateView(item);
            }
        }

        private void ShowContextMenu(int itemId, PointerEventData eventData)
        {
            var item = _items.FirstOrDefault(x => x.Id == itemId);
            if (item == null)
                return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)_view.transform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint);

            var options = item.GetContextMenuOptions();
            _itemContextMenu.Show(item.Id, item.Asset.MaxInStack, options, localPoint);
        }

        private List<InventoryItem> CreateItemsByStackSize(ItemAsset asset, int totalAmount)
        {
            var result = new List<InventoryItem>();
            var stackSize = asset.MaxInStack;
            var amountLeft = totalAmount;
            var newCellId = NewId;

            while (amountLeft > 0)
            {
                var amount = Mathf.Clamp(amountLeft, 0, stackSize);
                var newItem = new InventoryItem(newCellId, amount, asset);

                result.Add(newItem);
                newCellId++;
                amountLeft -= amount;
            }

            return result;
        }
    }
}