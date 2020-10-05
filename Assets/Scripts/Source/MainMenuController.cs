using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void SwitchSceneToLevel()
    {
        PauseMenuController.Paused = false;
        SceneManager.LoadSceneAsync("Level");
    }
}
