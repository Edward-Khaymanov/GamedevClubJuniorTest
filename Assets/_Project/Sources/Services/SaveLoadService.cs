using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ClubTest
{
    public class SaveLoadService
    {
        public string PlayerSavePath => Path.Combine(Application.persistentDataPath, CONSTANTS.PLAYER_SAVE_FILE_NAME);

        public PlayerSaveData LoadPlayer()
        {
            var jsonSettings = new JsonSerializerSettings()
            {
                MissingMemberHandling = MissingMemberHandling.Error,
            };
            var playerJson = File.ReadAllText(PlayerSavePath);
            var player = JsonConvert.DeserializeObject<PlayerSaveData>(playerJson, jsonSettings);
            return player;
        }

        public PlayerSaveData LoadPlayerDefault()
        {
            return new PlayerSaveData()
            {
                Stats = new UnitStats() 
                { 
                    FOVDistance = 3, 
                    Healf = new Healf(500), 
                    MoveSpeed = 6 
                },
                Inventory = new List<InventoryCellData>()
                {
                    new InventoryCellData() { Id = 1, Amount = 1, ItemDefinitionId = 2 },
                    new InventoryCellData() { Id = 2, Amount = 30, ItemDefinitionId = 3 }
                },
                EquipedItemsInventoryId = new List<int>()
                {
                    1
                }
            };
        }

        public void SavePlayer(PlayerSaveData playerData)
        {
            var playerJson = JsonConvert.SerializeObject(playerData);
            File.WriteAllText(PlayerSavePath, playerJson);
        }

        public void DeletePlayer()
        {
            if (File.Exists(PlayerSavePath))
                File.Delete(PlayerSavePath);
        }
    }
}