using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClubTest
{
    [Serializable]
    public class InventoryItem
    {
        public InventoryItem(int id, int amount, ItemAsset asset)
        {
            Id = id;
            Amount = amount;
            Asset = asset;
        }

        [field: SerializeField, Min(1)] public int Id { get; private set; }
        [field: SerializeField, Min(1)] public int Amount { get; private set; }
        [field: SerializeField] public ItemAsset Asset { get; private set; }


        public void Add(int amount)
        {
            var tempAmount = Amount + amount;
            Amount = Mathf.Clamp(tempAmount, 0, Asset.MaxInStack);
        }

        public void Remove(int amount)
        {
            var tempAmount = Amount - amount;
            Amount = Mathf.Clamp(tempAmount, 0, Asset.MaxInStack);
        }

        public IEnumerable<ContextMenuOptionType> GetContextMenuOptions()
        {
            yield return ContextMenuOptionType.Delete;
            if (Asset.MaxInStack > 1)
                yield return ContextMenuOptionType.DeleteAll;

            switch (Asset)
            {
                case WeaponAsset:
                    yield return ContextMenuOptionType.Equip;
                    break;
                default:
                    break;
            }
        }
    }
}