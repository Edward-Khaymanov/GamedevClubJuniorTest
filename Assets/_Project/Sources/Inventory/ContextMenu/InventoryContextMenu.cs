using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ClubTest
{
    public class InventoryContextMenu : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private SelectableOption _equipOption;
        [SerializeField] private SelectableOption _deleteOption;
        [SerializeField] private SelectableOption _deleteAllOption;

        private int _currentCellId;
        private int _currentCellAmount;

        private Dictionary<ContextMenuOptionType, BaseContextMenuOption> _optionsByType;

        public event Action<int, int> DeleteRequested;
        public event Action<int> EquipRequested;

        private void Awake()
        {
            _optionsByType = new Dictionary<ContextMenuOptionType, BaseContextMenuOption>()
            {
                [ContextMenuOptionType.Equip] = _equipOption,
                [ContextMenuOptionType.Delete] = _deleteOption,
                [ContextMenuOptionType.DeleteAll] = _deleteAllOption,
            };
        }

        private void OnEnable()
        {
            _equipOption.Selected += Equip;
            _deleteOption.Selected += Delete;
            _deleteAllOption.Selected += DeleteAll;
        }

        private void OnDisable()
        {
            _equipOption.Selected -= Equip;
            _deleteOption.Selected -= Delete;
            _deleteAllOption.Selected -= DeleteAll;
        }

        public void Show(int cellId, int amountInCell, IEnumerable<ContextMenuOptionType> options, Vector2 position)
        {
            _currentCellId = cellId;
            _currentCellAmount = amountInCell;
            ((RectTransform)transform).localPosition = position;

            foreach (var optionType in options)
            {
                _optionsByType[optionType].Show();
            }

            var otherOptions = _optionsByType.Keys.Except(options);
            foreach (var optionType in otherOptions)
            {
                _optionsByType[optionType].Hide();
            }

            _canvasGroup.alpha = 1;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
            foreach (var option in _optionsByType.Values)
            {
                option.Hide();
            }
        }

        private void Delete()
        {
            DeleteMany(1);
        }

        private void DeleteMany(int amount)
        {
            DeleteRequested?.Invoke(_currentCellId, amount);
            Hide();
        }

        private void DeleteAll()
        {
            DeleteMany(_currentCellAmount);
        }

        private void Equip()
        {
            EquipRequested?.Invoke(_currentCellId);
            Hide();
        }
    }
}