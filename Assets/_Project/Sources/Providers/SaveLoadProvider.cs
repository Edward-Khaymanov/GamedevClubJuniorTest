using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace ClubTest
{
    public class SaveLoadProvider
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