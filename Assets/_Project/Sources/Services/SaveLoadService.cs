using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace ClubTest
{
    public class SaveLoadService
    {
        public string PlayerSavePath => Path.Combine(Application.persistentDataPath, CONSTANTS.PLAYER_SAVE_FILE_NAME);

        public PlayerSaveData LoadPlayer()
        {
            var playerJson = File.ReadAllText(PlayerSavePath);
            return DeserializePlayerJson(playerJson);
        }

        public PlayerSaveData LoadPlayerDefault()
        {
            var file = Resources.Load<TextAsset>(CONSTANTS.DEFAULT_PLAYER_PATH);
            return DeserializePlayerJson(file.text);
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

        private PlayerSaveData DeserializePlayerJson(string json)
        {
            var jsonSettings = new JsonSerializerSettings()
            {
                MissingMemberHandling = MissingMemberHandling.Error,
            };
            var player = JsonConvert.DeserializeObject<PlayerSaveData>(json, jsonSettings);
            return player;
        }
    }
}