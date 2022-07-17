using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject tutorialCanvas;
    public void OnClickSettingsButton()
    {
        SceneManager.LoadSceneAsync("SettingsMenu");
    }

    public void OnClickStartButton()
    {
        SceneManager.LoadSceneAsync("Main");
    }

    public void OnClickQuitButton()
    {
        Application.Quit();
    }

    public void OnClickTutorialButton() {
        tutorialCanvas.SetActive(true);
    }
}
