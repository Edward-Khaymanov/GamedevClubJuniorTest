using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ClubTest
{
    public class ItemService
    {
        private IEnumerable<ItemAsset> _items;

        public void Initialize()
        {
            _items = Resources.LoadAll<ItemAsset>(CONSTANTS.ITEMS_PATH);
        }

        public T GetItemById<T>(int itemId) where T : ItemAsset
        {
            var result = default(T);
            var itemAsset = (T)_items.FirstOrDefault(x => x.Id == itemId);

            if (itemAsset != null)
                result = Object.Instantiate(itemAsset);

            return result;
        }

        public IEnumerable<ItemAsset> GetItemsById(IEnumerable<int> itemsId)
        {
            return _items.Where(x => itemsId.Contains(x.Id));
        }
    }
}