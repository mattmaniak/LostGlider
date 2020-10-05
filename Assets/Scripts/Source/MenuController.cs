using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    GameObject pauseButton;

    [SerializeField]
    GameObject[] pauseMenuButtons;

    static bool paused = false;

    static public bool Paused
    {
        get => paused;
    }

    void Update()
    {
        // TODO: IF STATEMENT INSTEAD OF LOOP EVERY UPDATE.
        foreach (var button in pauseMenuButtons)
        {
            button.SetActive(paused);
        }
        pauseButton.SetActive(!paused);
    }

    // Load, switch a next and unload a previous scene.
    public void SwitchToScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void Pause()
    {
        paused = true;
    }

    public void Unpause()
    {
        paused = false;
    }
}
