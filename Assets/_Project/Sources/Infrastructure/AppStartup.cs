using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace ClubTest
{
    public class AppStartup : MonoBehaviour
    {
        private SaveLoadService _saveLoadService;

        [Inject]
        private void Construct(SaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        private void Start()
        {
            var saveExist = File.Exists(_saveLoadService.PlayerSavePath);
            if (saveExist)
            {
                try
                {
                    var playerDataSave = _saveLoadService.LoadPlayer();
                }
                catch (System.Exception ex)
                {
                    //говорим игроку что с сохранением проблема. предлагаем варианты решения.
                    return;
                }
                SceneManager.LoadScene(CONSTANTS.SCENE_GAME_INDEX);
                return;
            }

            var firstLauchFilePath = Path.Combine(Application.persistentDataPath, CONSTANTS.APP_FIRST_LAUCH_FILE_NAME);
            var IsFirstLaunch = File.Exists(firstLauchFilePath) == false;

            if (IsFirstLaunch)
            {
                var playerDataSave = _saveLoadService.LoadPlayerDefault();
                _saveLoadService.SavePlayer(playerDataSave);
                File.WriteAllText(firstLauchFilePath, string.Empty);
                SceneManager.LoadScene(CONSTANTS.SCENE_GAME_INDEX);
                return;
            }
            else
            {
                //говорим игроку что с сейв пропал. предлагаем варианты решения.
            }
        }
    }
}