using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonNavigator : MonoBehaviour
{
    // Load, switch a next and unload a previous scene.
    public void SwitchToScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
