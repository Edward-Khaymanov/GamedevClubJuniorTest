using UnityEngine;
using Zenject;

namespace ClubTest
{
    public class LevelDataInstaller : MonoInstaller
    {
        [SerializeField] private LevelData _levelData;

        public override void InstallBindings()
        {
            Container
                .Bind<LevelData>()
                .FromInstance(_levelData);
        }
    }
}