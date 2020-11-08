using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class MainMenuController : MonoBehaviour
    {
        public void SwitchSceneToLevel()
        {
            // TODO: FIX
            FindObjectOfType<PauseMenuController>().Paused = false;
            SceneManager.LoadSceneAsync("Level");
        }

        public void QuitGame()
        {
            Utils.UnityQuit.Quit();
        }
    }    
}
