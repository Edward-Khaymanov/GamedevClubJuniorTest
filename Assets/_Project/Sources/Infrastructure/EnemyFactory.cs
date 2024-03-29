using System.Collections.Generic;
using UnityEngine;

namespace ClubTest
{
    public class EnemyFactory
    {
        private readonly IDictionary<EnemyType, Enemy> _templates;

        public EnemyFactory(IDictionary<EnemyType, Enemy> enemyTemplates)
        {
            _templates = enemyTemplates;
        }

        public Enemy Spawn(EnemyType enemyType, Vector2 position)
        {
            var template = _templates[enemyType];
            var enemy = GameObject.Instantiate(template, position, Quaternion.identity);
            enemy.Init(enemyType);
            return enemy;
        }

        public void Despawn(Enemy enemy)
        {
            GameObject.Destroy(enemy.gameObject);
        }
    }
}