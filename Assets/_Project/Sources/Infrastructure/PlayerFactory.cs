using UnityEngine;
using Zenject;

namespace ClubTest
{
    public class PlayerFactory
    {
        private readonly DiContainer _diContainer;
        private readonly Player _playerTemplate;

        [Inject]
        public PlayerFactory(DiContainer diContainer, Player playerTemplate)
        {
            _diContainer = diContainer;
            _playerTemplate = playerTemplate;
        }

        public Player Spawn(PlayerData playerData, Vector2 position)
        {
            var player = _diContainer.InstantiatePrefabForComponent<Player>(_playerTemplate, position, Quaternion.identity, null);
            player.Init(playerData);
            return player;
        }
    }
}