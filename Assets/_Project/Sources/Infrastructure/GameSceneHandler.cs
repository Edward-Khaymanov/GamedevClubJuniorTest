using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace ClubTest
{
    public class GameSceneHandler : MonoBehaviour
    {
        [SerializeField] private LevelData _levelData;
        [SerializeField] private Transform _minSpawnPoint;
        [SerializeField] private Transform _maxSpawnPoint;
        [SerializeField] private PlayerData _defaultPlayerData;
        [SerializeField] private Transform _defaultPlayerPosition;
        [SerializeField] private CameraFollower _playerCamera;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _reloadButton;
        [SerializeField] private Button _deleteSaveButton;


        private SaveLoadService _saveLoadService;
        private EnemyFactory _enemyFactory;
        private PlayerFactory _playerFactory;
        private ItemService _itemService;

        private Player _player;

        [Inject]
        public void Construct(SaveLoadService saveLoadService, EnemyFactory enemyFactory, PlayerFactory playerFactory, ItemService itemService)
        {
            _saveLoadService = saveLoadService;
            _enemyFactory = enemyFactory;
            _playerFactory = playerFactory;
            _itemService = itemService;
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
                var playerDataSave = _saveLoadService.LoadPlayer();
                playerData = new PlayerData()
                {
                    Stats = playerDataSave.Stats,
                    InventoryItems = new List<InventoryItem>(),
                    EquipedWeaponInventoryItemId = playerDataSave.EquipedWeaponInventoryItemId
                };

                foreach (var saveItem in playerDataSave.InventoryItems)
                {
                    var itemAsset = _itemService.GetItemById<ItemAsset>(saveItem.AssetId);
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
            foreach (var enemyTypeAmount in _levelData.EnemyTypeAmountToSpawn)
            {
                for (var i = 0; i < enemyTypeAmount.Value; i++)
                {
                    var position = Helpers.GetRandomPointBeetween(_minSpawnPoint.position, _maxSpawnPoint.position);
                    _enemyFactory.Spawn(enemyTypeAmount.Key, position);
                }
            }
        }

        private void ReloadLevel()
        {
            SceneManager.LoadScene(CONSTANTS.SCENE_GAME_INDEX);
        }

        private void SaveData()
        {
            _saveLoadService.SavePlayer(_player.GetSaveData());
        }

        private void DeleteSaveData()
        {
            _saveLoadService.DeletePlayer();
        }
    }
}