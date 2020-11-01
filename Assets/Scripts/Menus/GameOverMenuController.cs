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
            PlayerAliveBefore = FindObjectOfType<Player>().Alive;
        }

        void Update()
        {
            var player = FindObjectOfType<Player>();

            if (player.Alive != PlayerAliveBefore)
            {
                ToggleVisibilityOfGUI(!player.Alive);
                PlayerAliveBefore = player.Alive;
            }
        }

        public void RestartCurrentLevel()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            FindObjectOfType<Player>().Alive = true;
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
