using UnityEngine;
using Zenject;

namespace ClubTest
{
    public class EnemyInstaller : MonoInstaller
    {
        [SerializeField] private DropedItemView _dropedItemViewTemplate;

        public override void InstallBindings()
        {
            BindDropedItemView();
            BindEnemyFactory();
        }

        private void BindDropedItemView()
        {
            Container
                .Bind<DropedItemView>()
                .FromInstance(_dropedItemViewTemplate)
                .AsSingle()
                .NonLazy();
        }

        private void BindEnemyFactory()
        {
            Container
                .Bind<EnemyFactory>()
                .To<EnemyFactory>()
                .AsSingle()
                .NonLazy();
        }
    }
}