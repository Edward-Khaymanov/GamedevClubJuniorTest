using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClubTest
{
    public class InventoryContextMenu : MonoBehaviour
    {
        [SerializeField] private SelectableOption _equipOption;
        [SerializeField] private SelectableOption _deleteOption;
        [SerializeField] private SelectableOption _deleteAllOption;

        private int _currentItemId;
        private int _currentItemMaxStack;

        private Dictionary<ContextMenuOptionType, BaseContextMenuOption> _optionsByType;

        public event Action<int, int> DeleteRequested;
        public event Action<int> EquipRequested;

        [Inject]
        private void Constructor()
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

        public void Show(int itemId, int maxInStack, IEnumerable<ContextMenuOptionType> optionTypes, Vector2 position)
        {
            Hide();
            _currentItemId = itemId;
            _currentItemMaxStack = maxInStack;
            ((RectTransform)transform).anchoredPosition = position;

            foreach (var optionType in optionTypes)
            {
                _optionsByType[optionType].Show();
            }

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
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
            DeleteRequested?.Invoke(_currentItemId, amount);
            Hide();
        }

        private void DeleteAll()
        {
            DeleteMany(_currentItemMaxStack);
        }

        private void Equip()
        {
            EquipRequested?.Invoke(_currentItemId);
            Hide();
        }
    }
}