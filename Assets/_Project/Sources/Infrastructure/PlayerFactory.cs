using UnityEngine;

namespace ClubTest
{
    public class PlayerFactory
    {
        private readonly Player _playerTemplate;

        public PlayerFactory(Player playerTemplate)
        {
            _playerTemplate = playerTemplate;
        }

        public Player Spawn(PlayerSaveData playerData, Inventory inventory, IPlayerInput input, Vector2 position)
        {
            var player = GameObject.Instantiate(_playerTemplate, position, Quaternion.identity);
            player.Init(playerData, inventory, input);
            return player;
        }
    }
}