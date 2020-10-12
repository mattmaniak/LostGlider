using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuController : MonoBehaviour
{
    [SerializeField]
    GameObject[] gameOverMenuButtons;

    bool playerAliveBefore = Player.Alive;

    void Update()
    {
        if (Player.Alive != playerAliveBefore)
        {
            ToggleVisibilityOfGUI(!Player.Alive);
            playerAliveBefore = Player.Alive;
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
