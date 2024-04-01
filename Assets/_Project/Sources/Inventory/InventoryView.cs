using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

namespace ClubTest
{
    public class InventoryView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Transform _cellsContainer;

        private InventoryCellView _cellViewTemplate;
        private InventoryContextMenu _contextMenu;
        private List<InventoryCellView> _cellsViews;

        [Inject]
        private void Construct(InventoryContextMenu contextMenu, InventoryCellView cellViewTemplate)
        {
            _contextMenu = contextMenu;
            _cellViewTemplate = cellViewTemplate;
            _cellsViews = new List<InventoryCellView>();
        }

        private void OnEnable()
        {
            SubscribeContextMenu();
        }

        private void OnDisable()
        {
            UnsubscribeContextMenu();
        }

        public void AddView(InventoryCell cell)
        {
            var view = Instantiate(_cellViewTemplate, _cellsContainer);
            view.Render(cell);
            view.Clicked += ShowContextMenu;
            _cellsViews.Add(view);
        }

        public void UpdateView(InventoryCell cell)
        {
            var view = _cellsViews.FirstOrDefault(x => x.CellId == cell.Id);
            if (view != null)
                view.Render(cell);
        }

        public void RemoveView(int id)
        {
            var view = _cellsViews.FirstOrDefault(x => x.CellId == id);
            if (view == null)
                return;

            view.Clicked -= ShowContextMenu;
            _cellsViews.Remove(view);
            Destroy(view.gameObject);
        }

        public void Show()
        {
            _canvas.enabled = true;
        }

        public void Hide()
        {
            _canvas.enabled = false;
            _contextMenu.Hide();
        }

        private void SubscribeContextMenu()
        {
            foreach (var view in _cellsViews)
            {
                view.Clicked += ShowContextMenu;
            }
        }
        
        private void UnsubscribeContextMenu()
        {
            foreach (var view in _cellsViews)
            {
                view.Clicked -= ShowContextMenu;
            }
        }

        private void ShowContextMenu(int itemId, PointerEventData eventData)
        {
            var view = _cellsViews.FirstOrDefault(x => x.CellId == itemId);
            if (view == null)
                return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)transform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint);

            _contextMenu.Show(view.CellId, view.Amount, view.ContextMenuOptions, localPoint);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _contextMenu.Hide();
        }
    }
}