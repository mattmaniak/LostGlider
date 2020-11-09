﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuButtons : MonoBehaviour
{
    public void SwitchSceneToMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void QuitGame()
    {
        Utils.UnityQuit.Quit();
    }
}
