using UnityEngine;
using Zenject;

namespace ClubTest
{
    public class InventoryInstaller : MonoInstaller
    {
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private InventoryContextMenu _inventoryContextMenu;
        [SerializeField] private InventoryCellView _inventoryCellViewTemplate;

        public override void InstallBindings()
        {
            BindInventoryCell();
            BindInventoryView();
            BindContextMenu();
        }

        private void BindInventoryCell()
        {
            Container
                .Bind<InventoryCellView>()
                .FromInstance(_inventoryCellViewTemplate)
                .WhenInjectedInto<InventoryView>();
        }

        private void BindInventoryView()
        {
            Container
                .Bind<InventoryView>()
                .FromInstance(_inventoryView);
        }

        private void BindContextMenu()
        {
            Container
                .Bind<InventoryContextMenu>()
                .FromInstance(_inventoryContextMenu);
        }
    }
}
