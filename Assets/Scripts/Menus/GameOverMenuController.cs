using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{
    public class GameOverMenuController : MonoBehaviour
    {
        [SerializeField]
        GameObject[] gameOverMenuButtons;

        bool PlayerAliveBefore { get; set; }

        void Start()
        {
            PlayerAliveBefore = Player.Alive;
        }

        void Update()
        {
            if (Player.Alive != PlayerAliveBefore)
            {
                ToggleVisibilityOfGUI(!Player.Alive);
                PlayerAliveBefore = Player.Alive;
            }
        }

        public void RestartCurrentLevel()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            Player.Alive = true;
        }

        void ToggleVisibilityOfGUI(bool toggled)
        {
            foreach (var button in gameOverMenuButtons)
            {
                button.SetActive(toggled);
            }
        }
    }
    
}
