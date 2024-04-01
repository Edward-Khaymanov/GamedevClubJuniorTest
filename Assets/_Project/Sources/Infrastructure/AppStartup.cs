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
            try
            {
                var playerDataSave = _saveLoadService.LoadPlayer();
            }
            catch (System.Exception)
            {
                var playerDataSave = _saveLoadService.LoadPlayerDefault();
                _saveLoadService.SavePlayer(playerDataSave);

            }
            finally
            {
                SceneManager.LoadScene(CONSTANTS.SCENE_GAME_INDEX);
            }
        }
    }
}