using UnityEngine;
using Zenject;

namespace ClubTest
{
    public class PlayerFactoryInstaller : MonoInstaller
    {
        [SerializeField] private Player _playerTemplate;

        public override void InstallBindings()
        {
            BindPlayerFactory();
        }

        private void BindPlayerFactory()
        {
            var factory = new PlayerFactory(_playerTemplate);
            Container
                .Bind<PlayerFactory>()
                .FromInstance(factory)
                .AsSingle()
                .NonLazy();
        }
    }
}
