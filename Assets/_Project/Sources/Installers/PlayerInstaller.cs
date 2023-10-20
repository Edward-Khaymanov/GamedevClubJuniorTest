using UnityEngine;
using Zenject;

namespace ClubTest
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private Player _playerTemplate;

        public override void InstallBindings()
        {
            BindPlayerFactory();
        }

        private void BindPlayerFactory()
        {
            Container
                .Bind<PlayerFactory>()
                .To<PlayerFactory>()
                .AsSingle()
                .WithArguments(_playerTemplate)
                .NonLazy();
        }
    }
}
