using UnityEngine;
using Zenject;

namespace ClubTest
{
    public class EnemyFactory
    {
        private readonly DiContainer _diContainer;

        public EnemyFactory(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        public Enemy Spawn(Vector2 position, Enemy enemyTemplate)
        {
            var enemy = _diContainer.InstantiatePrefabForComponent<Enemy>(enemyTemplate, position, Quaternion.identity, null);
            return enemy;
        }
    }
}