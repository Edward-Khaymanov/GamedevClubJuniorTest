using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace ClubTest
{
    public class InventoryView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Transform _cellsContainer;

        private Canvas _canvas;
        private InventoryItemView _cellTemplate;
        private List<InventoryItemView> _itemViews;
        private SharedInput _input;
        private InventoryContextMenu _itemContextMenu;

        public IReadOnlyList<InventoryItemView> ItemViews => _itemViews;

        [Inject]
        private void Construct(InventoryItemView ItemViewTemplate, InventoryContextMenu inventoryContextMenu)
        {
            _cellTemplate = ItemViewTemplate;
            _itemContextMenu = inventoryContextMenu;
            _canvas = GetComponent<Canvas>();
            _input = new SharedInput();
            _itemViews = new List<InventoryItemView>();
        }

        private void OnEnable()
        {
            _input.Enable();
            _input.UI.Inventory.performed += (ctx) => SwitchState();
        }

        private void OnDisable()
        {
            _input.Disable();
            _input.UI.Inventory.performed -= (ctx) => SwitchState();
        }

        public InventoryItemView Add(int itemId, Sprite icon, int amount)
        {
            var cell = Instantiate(_cellTemplate, _cellsContainer);
            cell.Render(itemId, icon, amount);
            _itemViews.Add(cell);
            return cell;
        }

        public void UpdateView(InventoryItem item)
        {
            var cell = _itemViews.FirstOrDefault(x => x.ItemId == item.Id);
            if (cell != null)
                cell.Render(item.Id, item.Asset.Icon, item.Amount);
        }

        public void Remove(int id)
        {
            var cell = _itemViews.FirstOrDefault(x => x.ItemId == id);
            if (cell == null)
                return;

            _itemViews.Remove(cell);
            Destroy(cell.gameObject);
        }

        private void SwitchState()
        {
            if (_canvas.enabled)
                Hide();
            else
                Show();
        }

        private void Show()
        {
            _canvas.enabled = true;
        }

        private void Hide()
        {
            _canvas.enabled = false;
            _itemContextMenu.Hide();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _itemContextMenu.Hide();
        }
    }
}