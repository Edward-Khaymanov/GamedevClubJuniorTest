using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace ClubTest
{
    public class GameSceneHandler : MonoBehaviour
    {
        [SerializeField] private List<Enemy> _enemyToSpawn;
        [SerializeField] private Transform _minSpawnPoint;
        [SerializeField] private Transform _maxSpawnPoint;
        [SerializeField] private PlayerData _defaultPlayerData;
        [SerializeField] private Transform _defaultPlayerPosition;
        [SerializeField] private CameraFollower _playerCamera;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _reloadButton;
        [SerializeField] private Button _deleteSaveButton;


        private SaveLoadProvider _saveSystem;
        private EnemyFactory _enemyFactory;
        private PlayerFactory _playerFactory;
        private ItemProvider _itemProvider;

        private Player _player;


        [Inject]
        public void Construct(SaveLoadProvider saveSystem, EnemyFactory enemyFactory, PlayerFactory playerFactory, ItemProvider itemProvider)
        {
            _saveSystem = saveSystem;
            _enemyFactory = enemyFactory;
            _playerFactory = playerFactory;
            _itemProvider = itemProvider;
        }

        private void Start()
        {
            SpawnPlayer();
            SpawnEnemyAtRandomPosition();
        }

        private void OnEnable()
        {
            _saveButton.onClick.AddListener(SaveData);
            _reloadButton.onClick.AddListener(ReloadLevel);
            _deleteSaveButton.onClick.AddListener(DeleteSaveData);
        }

        private void OnDisable()
        {
            _saveButton.onClick.RemoveListener(SaveData);
            _reloadButton.onClick.RemoveListener(ReloadLevel);
            _deleteSaveButton.onClick.RemoveListener(DeleteSaveData);
        }

        private void OnApplicationQuit()
        {
            // здесь могло быть сохранение
        }

        private void SpawnPlayer()
        {
            var playerData = default(PlayerData);
            var spawnPosition = _defaultPlayerPosition.position;

            try
            {
                var playerDataSave = _saveSystem.LoadPlayer();
                playerData = new PlayerData()
                {
                    Stats = playerDataSave.Stats,
                    InventoryItems = new List<InventoryItem>(),
                    EquipedWeaponInventoryItemId = playerDataSave.EquipedWeaponInventoryItemId
                };

                foreach (var saveItem in playerDataSave.InventoryItems)
                {
                    var itemAsset = _itemProvider.GetItemById<ItemAsset>(saveItem.AssetId);
                    var item = new InventoryItem(saveItem.Id, saveItem.Amount, itemAsset);
                    playerData.InventoryItems.Add(item);
                }
            }
            catch (System.Exception)
            {
                playerData = _defaultPlayerData;
            }

            _player = _playerFactory.Spawn(playerData, spawnPosition);
            _playerCamera.SetTarget(_player.transform);
        }

        private void SpawnEnemyAtRandomPosition()
        {
            foreach (var enemyTemplate in _enemyToSpawn)
            {
                var position = new Vector2(Random.Range(_minSpawnPoint.position.x, _maxSpawnPoint.position.x), Random.Range(_minSpawnPoint.position.y, _maxSpawnPoint.position.y));
                _enemyFactory.Spawn(position, enemyTemplate);
            }
        }

        private void ReloadLevel()
        {
            SceneManager.LoadScene(CONSTANTS.SCENE_GAME_INDEX);
        }

        private void SaveData()
        {
            _saveSystem.SavePlayer(_player.GetSaveData());
        }

        private void DeleteSaveData()
        {
            _saveSystem.DeletePlayer();
        }

    }
}