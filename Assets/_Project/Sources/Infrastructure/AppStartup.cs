using UnityEngine;
using UnityEngine.SceneManagement;

namespace ClubTest
{
    public class AppStartup : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene(CONSTANTS.SCENE_GAME_INDEX);
        }
    }
}