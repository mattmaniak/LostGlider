using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{    
    class PauseMenuController : MonoBehaviour
    {
        [SerializeField]
        GameObject pauseButton;

        [SerializeField]
        GameObject[] pauseMenuButtons;

        public bool Paused { get; set; } = false;
        bool PausedBefore { get; set; } = false;

        void Update()
        {
            if (Paused != PausedBefore)
            {
                foreach (var button in pauseMenuButtons)
                {
                    button.SetActive(Paused);
                }
                pauseButton.SetActive(!Paused);            
            }
            PausedBefore = Paused;
        }

        public void SwitchSceneToMainMenu()
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }

        public void Pause()
        {
            Paused = true;
        }

        public void Unpause()
        {
            Paused = false;
        }

        public void QuitGame()
        {
            Utils.UnityQuit.Quit();
        }
    }
}

