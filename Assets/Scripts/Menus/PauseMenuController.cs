using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{    
    public class PauseMenuController : MonoBehaviour
    {
        [SerializeField]
        GameObject pauseButton;

        [SerializeField]
        GameObject[] pauseMenuButtons;

        [SerializeField]
        Transform playerTransform;

        static bool paused = false;
        static bool pausedBefore = paused;

        static public bool Paused
        {
            get => paused;
            set => paused = value;
        }

        void Update()
        {
            if (paused != pausedBefore)
            {
                foreach (var button in pauseMenuButtons)
                {
                    button.SetActive(paused);
                }
                pauseButton.SetActive(!paused);            
            }
            pausedBefore = paused;
        }

        public void SwitchSceneToMainMenu()
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }

        public void Pause()
        {
            paused = true;
        }

        public void Unpause()
        {
            paused = false;
        }

        public void QuitGame()
        {
            Utils.UnityQuit.Quit();
        }
    }
}

