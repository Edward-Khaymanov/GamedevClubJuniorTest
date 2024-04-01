using Zenject;

namespace ClubTest
{
    public class InputInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container
                .Bind<IPlayerInput>()
                .To<DefaultPlayerInput>()
                .AsSingle();
        }
    }
}