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
            var service = new ItemService();
            service.Initialize();
            Container
                .Bind<ItemService>()
                .FromInstance(service)
                .AsSingle()
                .NonLazy();
        }

        private void BindSaveLoadService()
        {
            Container
                .Bind<SaveLoadService>()
                .FromInstance(new SaveLoadService())
                .AsSingle()
                .NonLazy();
        }
    }
}
