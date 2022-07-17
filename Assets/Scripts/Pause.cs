using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject Leaderboard;
    [SerializeField] GameModel gameManager;
    [SerializeField] TMPro.TMP_Text pauseLeaderboardText;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameManager.betweenRounds)
        {
            if (Time.timeScale == 0.0f)
                ResumeGame();
            else
                PauseGame();
        }
    }

    private void PauseGame()
    {
        Leaderboard.SetActive(false);
        gameManager.GetComponent<AudioSource>().Pause();
        Camera.main.GetComponent<CameraRotate>().StartRotation(false, false);
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        gameManager.GetComponent<AudioSource>().Play();
        Camera.main.GetComponent<CameraRotate>().StartRotation(true, false);
        Time.timeScale = 1.0f;
    }

    public void QuitGame()
    {
        print("quit");
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }
}
