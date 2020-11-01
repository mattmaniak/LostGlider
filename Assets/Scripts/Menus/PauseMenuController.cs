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

        internal bool Paused { get; set; }
        bool PausedBefore { get; set; }

        void Start()
        {
            PausedBefore = Paused = false;
        }

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

