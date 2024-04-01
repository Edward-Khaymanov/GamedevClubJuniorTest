using Zenject;

namespace ClubTest
{
    public class ServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindSaveLoadService();
            BindItemService();
        }

        private void BindItemService()
        {
            var service = new ItemService(CONSTANTS.ITEMS_PATH);
            Container
                .Bind<ItemService>()
                .FromInstance(service)
                .AsSingle()
                .NonLazy();
        }

        private void BindSaveLoadService()
        {
            var service = new SaveLoadService();
            Container
                .Bind<SaveLoadService>()
                .FromInstance(service)
                .AsSingle()
                .NonLazy();
        }
    }
}
