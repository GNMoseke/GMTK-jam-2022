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

    void OnGUI()
    {
        Event e = Event.current;
        if (e.keyCode == KeyCode.Escape)
        {
            PauseGame();
        }
    }

    private void Update()
    {
        //Hold off changing board
    }

    private void PauseGame()
    {
        Camera.main.GetComponent<CameraRotate>().StartRotation(false, false);
        Time.timeScale = 0.0f;
        pauseMenu.SetActive(true);
        Leaderboard.SetActive(false);
    }

    public void ResumeGame()
    {
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
