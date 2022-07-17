using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class ButtonManager : MonoBehaviour
{
    // TODO: remove
    public GameObject tutorialCanvas;
    public AudioMixer mixer;
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

    public void SetVolume(float volume) {
        mixer.SetFloat("MasterVolume", volume);
    }
}
