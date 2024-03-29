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
            var templates = Resources.Load<EnemyTemplates>(CONSTANTS.ENEMY_TEMPLATES_PATH);
            var factory = new EnemyFactory(templates.Templates);
            Container
                .Bind<EnemyFactory>()
                .FromInstance(factory)
                .AsSingle()
                .NonLazy();
        }
    }
}