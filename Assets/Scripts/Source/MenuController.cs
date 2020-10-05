using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    GameObject pauseButton;

    [SerializeField]
    GameObject[] pauseMenuButtons;

    static bool paused = false;

    static bool pausedBefore = paused;

    static public bool Paused
    {
        get => paused;
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
