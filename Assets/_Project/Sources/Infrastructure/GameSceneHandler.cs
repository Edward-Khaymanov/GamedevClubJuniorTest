using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace ClubTest
{
    public class GameSceneHandler : MonoBehaviour
    {
        [SerializeField] private Transform _minSpawnPoint;
        [SerializeField] private Transform _maxSpawnPoint;
        [SerializeField] private Transform _defaultPlayerPosition;
        [SerializeField] private CameraFollower _playerCamera;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _reloadButton;
        [SerializeField] private Button _deleteSaveButton;

        private SaveLoadService _saveLoadService;
        private EnemyFactory _enemyFactory;
        private PlayerFactory _playerFactory;
        private ItemService _itemService;
        private DropedItemView _dropedItemTemplate;
        private Player _player;
        private LevelData _levelData;
        private InventoryContextMenu _inventoryContextMenu;
        private InventoryView _inventoryView;
        private Inventory _inventory;
        private IPlayerInput _input;

        [Inject]
        public void Construct(
            SaveLoadService saveLoadService,
            EnemyFactory enemyFactory,
            PlayerFactory playerFactory,
            ItemService itemService,
            LevelData levelData,
            DropedItemView dropedItemView,
            InventoryView inventoryView,
            InventoryContextMenu inventoryContextMenu,
            IPlayerInput playerInput)
        {
            _saveLoadService = saveLoadService;
            _enemyFactory = enemyFactory;
            _playerFactory = playerFactory;
            _itemService = itemService;
            _levelData = levelData;
            _dropedItemTemplate = dropedItemView;
            _inventoryContextMenu = inventoryContextMenu;
            _inventoryView = inventoryView;
            _input = playerInput;
        }

        private void Start()
        {
            var playerSaveData = _saveLoadService.LoadPlayer();
            InitInventory(playerSaveData.Inventory);
            SpawnPlayer(playerSaveData);
            SpawnEnemyAtRandomPosition();

            _saveButton.onClick.AddListener(SaveData);
            _reloadButton.onClick.AddListener(ReloadLevel);
            _deleteSaveButton.onClick.AddListener(DeleteSaveData);

            _inventory.CellAdded += _inventoryView.AddView;
            _inventory.CellUpdated += _inventoryView.UpdateView;
            _inventory.CellRemoved += _inventoryView.RemoveView;

            _inventoryContextMenu.EquipRequested += _player.EquipItem;
            _inventoryContextMenu.DeleteRequested += _inventory.Remove;
            _input.InventoryOpen += _inventoryView.Show;
            _input.InventoryClose += _inventoryView.Hide;
            _input.Enable();
        }

        private void OnDestroy()
        {
            _saveButton.onClick.RemoveListener(SaveData);
            _reloadButton.onClick.RemoveListener(ReloadLevel);
            _deleteSaveButton.onClick.RemoveListener(DeleteSaveData);

            _inventory.CellAdded -= _inventoryView.AddView;
            _inventory.CellUpdated -= _inventoryView.UpdateView;
            _inventory.CellRemoved -= _inventoryView.RemoveView;

            _inventoryContextMenu.EquipRequested -= _player.EquipItem;
            _inventoryContextMenu.DeleteRequested -= _inventory.Remove;
            _input.InventoryOpen += _inventoryView.Show;
            _input.InventoryClose += _inventoryView.Hide;
            _input.Disable();
        }

        private void InitInventory(List<InventoryCellData> savedInventory)
        {
            var inventoryCells = new List<InventoryCell>();

            foreach (var savedInventoryCell in savedInventory)
            {
                var itemDefinition = _itemService.GetItemById<ItemDefinition>(savedInventoryCell.ItemDefinitionId);
                var cell = new InventoryCell(savedInventoryCell.Id, savedInventoryCell.Amount, itemDefinition);
                inventoryCells.Add(cell);
            }
            _inventory = new Inventory(inventoryCells);

            foreach (var cell in _inventory.Cells)
            {
                _inventoryView.AddView(cell);
            }
        }

        private void SpawnPlayer(PlayerSaveData playerData)
        {
            _player = _playerFactory.Spawn(playerData, _inventory, _input, _defaultPlayerPosition.position);
            _player.Died += OnPlayerDeath;
            _playerCamera.SetTarget(_player.transform);
        }

        private void SpawnEnemyAtRandomPosition()
        {
            foreach (var enemyTypeAmount in _levelData.EnemyTypeAmountToSpawn)
            {
                for (var i = 0; i < enemyTypeAmount.Value; i++)
                {
                    var position = Helpers.GetRandomPointBeetween(_minSpawnPoint.position, _maxSpawnPoint.position);
                    var enemy = _enemyFactory.Spawn(enemyTypeAmount.Key, position);
                    enemy.Died += OnEnemyDeath;
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
            var defaultSave = _saveLoadService.LoadPlayerDefault();
            _saveLoadService.SavePlayer(defaultSave);
        }

        private void ShowDeathScreen()
        {

        }


        private void OnEnemyDeath(Enemy enemy)
        {
            enemy.Died -= OnEnemyDeath;
            var dropItem = _levelData.EnemyDropList[enemy.Type].RandomSingle();
            var dropItemView = GameObject.Instantiate(_dropedItemTemplate, enemy.transform.position, Quaternion.identity);
            dropItemView.Init(dropItem);
            dropItemView.PickedUp += OnPlayerPickItem;
            _enemyFactory.Despawn(enemy);
        }

        private void OnPlayerPickItem(DropedItemView itemView, ItemAmount drop)
        {
            itemView.PickedUp -= OnPlayerPickItem;
            _inventory.Add(drop.Item, drop.Amount);
            GameObject.Destroy(itemView.gameObject);
        }

        private void OnPlayerDeath(Player player)
        {
            player.Died -= OnPlayerDeath;
            _input.Disable();
            _inventoryView.Hide();
            GameObject.Destroy(player.gameObject);
            ShowDeathScreen();
        }
    }
}