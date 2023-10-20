using Zenject;

namespace ClubTest
{
    public class ServiceInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindSaveSystem();
            BindItemProvider();
        }

        private void BindItemProvider()
        {
            var provider = new ItemProvider();
            provider.Initialize();
            Container
                .Bind<ItemProvider>()
                .FromInstance(provider)
                .AsSingle()
                .NonLazy();
        }

        private void BindSaveSystem()
        {
            Container
                .Bind<SaveLoadProvider>()
                .FromInstance(new SaveLoadProvider())
                .AsSingle()
                .NonLazy();
        }
    }
}
