using System.Collections.Generic;
using UnityEngine;

namespace ClubTest
{
    public class InventoryCell
    {
        private readonly InventoryCellData _cellData;

        public InventoryCell(int id, int amount, ItemDefinition itemDefinition)
        {
            _cellData = new InventoryCellData()
            {
                Id = id,
                Amount = amount,
                ItemDefinitionId = itemDefinition.Id,
            };
            ItemDefinition = itemDefinition;
        }

        public int Id => _cellData.Id;
        public int Amount => _cellData.Amount;
        public ItemDefinition ItemDefinition { get; private set; }

        public void Add(int amount)
        {
            var tempAmount = Amount + amount;
            _cellData.Amount = Mathf.Clamp(tempAmount, 0, ItemDefinition.MaxInStack);
        }

        public void Remove(int amount)
        {
            var tempAmount = Amount - amount;
            _cellData.Amount = Mathf.Clamp(tempAmount, 0, ItemDefinition.MaxInStack);
        }

        public IEnumerable<ContextMenuOptionType> GetContextMenuOptions()
        {
            var options = new List<ContextMenuOptionType>()
            {
                ContextMenuOptionType.Delete
            };

            if (ItemDefinition.MaxInStack > 1)
                options.Add(ContextMenuOptionType.DeleteAll);

            switch (ItemDefinition)
            {
                case WeaponDefinition:
                    options.Add(ContextMenuOptionType.Equip);
                    break;
                default:
                    break;
            }

            return options;
        }
    }
}