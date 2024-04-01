using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ClubTest
{
    [Serializable]
    public class Inventory
    {
        private readonly List<InventoryCell> _cells;

        public event Action<InventoryCell> CellAdded;
        public event Action<InventoryCell> CellUpdated;
        public event Action<int> CellRemoved;

        public Inventory(List<InventoryCell> items)
        {
            _cells = new List<InventoryCell>(items);
        }

        public IReadOnlyList<InventoryCell> Cells => _cells;
        private int NewId => _cells.Count == 0 ? 0 : _cells.Max(x => x.Id) + 1;

        public void Add(ItemDefinition itemDefinition, int amount)
        {
            if (amount < 1)
                return;

            var amountLeft = amount;
            var existedCells = _cells.Where(x =>
                x.ItemDefinition.Id == itemDefinition.Id &&
                x.ItemDefinition.MaxInStack >= x.Amount + 1)
                .OrderByDescending(x => x.Amount);

            if (existedCells.Any())
            {
                foreach (var existedCell in existedCells)
                {
                    var amountToAdd = existedCell.ItemDefinition.MaxInStack - existedCell.Amount;
                    amountToAdd = Math.Clamp(amountToAdd, 0, amountLeft);
                    existedCell.Add(amountToAdd);
                    amountLeft -= amountToAdd;
                    CellUpdated?.Invoke(existedCell);

                    if (amountLeft == 0)
                        return;
                }
            }

            var newCells = CreateCellsByStackSize(itemDefinition, amountLeft);
            _cells.AddRange(newCells);
            
            foreach (var cell in newCells)
            {
                CellAdded?.Invoke(cell);
            }
        }

        public void Remove(int cellId, int amount)
        {
            if (amount < 0)
                return;

            var cell = _cells.FirstOrDefault(x => x.Id == cellId);
            if (cell == null)
                return;

            cell.Remove(amount);

            if (cell.Amount == 0)
            {
                _cells.Remove(cell);
                CellRemoved?.Invoke(cell.Id);
            }
            else
            {
                CellUpdated?.Invoke(cell);
            }
        }

        public void Remove(Func<InventoryCell, bool> predicate, int amount)
        {
            if (amount < 0)
                return;

            var itemCells = _cells.Where(predicate).OrderBy(x => x.Amount).ToList();
            var totalAmount = itemCells.Sum(x => x.Amount);
            if (totalAmount < amount)
                return;

            var amountLeft = amount;

            while (amountLeft > 0)
            {
                var cell = itemCells.FirstOrDefault();
                var removeAmount = Mathf.Clamp(amountLeft, 0, cell.Amount);
                cell.Remove(removeAmount);
                amountLeft -= removeAmount;

                if (cell.Amount == 0)
                {
                    itemCells.Remove(cell);
                    _cells.Remove(cell);
                    CellRemoved?.Invoke(cell.Id);
                }
                else
                {
                    CellUpdated?.Invoke(cell);
                }
            }
        }

        private List<InventoryCell> CreateCellsByStackSize(ItemDefinition itemDefinition, int totalAmount)
        {
            var result = new List<InventoryCell>();
            var amountLeft = totalAmount;
            var newCellId = NewId;

            while (amountLeft > 0)
            {
                var amount = Mathf.Clamp(amountLeft, 0, itemDefinition.MaxInStack);
                var newItem = new InventoryCell(newCellId, amount, itemDefinition);

                result.Add(newItem);
                newCellId++;
                amountLeft -= amount;
            }

            return result;
        }
    }
}