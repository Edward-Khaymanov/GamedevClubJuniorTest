using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ClubTest
{
    public class ItemService
    {
        private IEnumerable<ItemDefinition> _items;

        public ItemService(string itemsPath)
        {
            _items = Resources.LoadAll<ItemDefinition>(itemsPath);
        }

        public T GetItemById<T>(int itemId) where T : ItemDefinition
        {
            var result = default(T);
            var itemAsset = (T)_items.FirstOrDefault(x => x.Id == itemId);

            if (itemAsset != null)
                result = Object.Instantiate(itemAsset);

            return result;
        }

        public IEnumerable<ItemDefinition> GetItemsById(IEnumerable<int> itemsId)
        {
            return _items.Where(x => itemsId.Contains(x.Id));
        }
    }
}