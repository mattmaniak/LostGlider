using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menus
{
    class GameOverMenuController : MonoBehaviour
    {
        [SerializeField]
        GameObject[] gameOverMenuButtons;

        [SerializeField]
        Transform playerTransform;

        bool PlayerAlive
        {
            get
            {
                return playerTransform.gameObject.GetComponent<Player>().Alive;
            }
            set 
            {
                playerTransform.gameObject.GetComponent<Player>().Alive = value;
            }
        }

        bool PlayerAliveBefore { get; set; }

        void Start()
        {
            PlayerAliveBefore = PlayerAlive;
        }

        void Update()
        {
            if (PlayerAlive != PlayerAliveBefore)
            {
                ToggleVisibilityOfGUI(!PlayerAlive);
                PlayerAliveBefore = PlayerAlive;
            }
        }

        public void RestartCurrentLevel()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            PlayerAlive = true;
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
