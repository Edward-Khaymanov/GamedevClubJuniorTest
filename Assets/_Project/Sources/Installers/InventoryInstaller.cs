using UnityEngine;
using Zenject;

namespace ClubTest
{
    public class InventoryInstaller : MonoInstaller
    {
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private InventoryItemView _inventoryItemViewTemplate;
        [SerializeField] private InventoryContextMenu _inventoryContextMenu;

        public override void InstallBindings()
        {
            BindInventoryCell();
            BindInventoryView();
            BindContextMenu();
        }

        private void BindInventoryCell()
        {
            Container
                .Bind<InventoryItemView>()
                .FromInstance(_inventoryItemViewTemplate)
                .WhenInjectedInto<InventoryView>()
                .NonLazy();
        }

        private void BindInventoryView()
        {
            Container
                .Bind<InventoryView>()
                .FromInstance(_inventoryView)
                .WhenInjectedInto<Player>();
        }

        private void BindContextMenu()
        {
            Container
                .Bind<InventoryContextMenu>()
                .FromInstance(_inventoryContextMenu);
        }
    }
}
