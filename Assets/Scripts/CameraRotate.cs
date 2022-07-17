using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{

    private Vector3 currentCameraAngle;
    private Vector3 targetCameraAngle;
    public GameModel gameManager;
    private bool rotating = false;
    private bool towardsTable;
    private bool isEndOfDay;
    public GameObject leaderboard;
    public GameObject pauseMenu;

    void Start()
    {
        currentCameraAngle = Camera.main.transform.eulerAngles;
        targetCameraAngle = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (rotating)
        {
            // Rotate until we come to a good resting spot
            if (Mathf.Abs((currentCameraAngle - targetCameraAngle).x) > 1f) {
                RotateCamera(towardsTable);
            }
            else {
                // once we're done rotation, check if we were going towards the table and trigger the next day if so.
                rotating = false;
                // This should really be an event system, but time constraint so jank it is
                if (this.towardsTable && isEndOfDay)
                {
                    gameManager.NextDay();
                }
            }
        }
    }

    public void RotateNewDay() {
        StartRotation(true, true);
    }

    public void StartRotation(bool towardsTable, bool isEndOfDay)
    {
        leaderboard.SetActive(isEndOfDay);
        pauseMenu.SetActive(!isEndOfDay);
        rotating = true;
        this.towardsTable = towardsTable;
        this.isEndOfDay = isEndOfDay;
        if (towardsTable)
        {
            targetCameraAngle = new Vector3(56f, 0f, 0f);
        }
        else
        {
            targetCameraAngle = Vector3.zero;
        }
    }

    public void RotateCamera(bool towardsTable)
    {
        currentCameraAngle = Camera.main.transform.eulerAngles;

        currentCameraAngle = new Vector3(
            Mathf.LerpAngle(currentCameraAngle.x, targetCameraAngle.x, Time.unscaledDeltaTime),
            Mathf.LerpAngle(currentCameraAngle.y, targetCameraAngle.y, Time.unscaledDeltaTime),
            Mathf.LerpAngle(currentCameraAngle.z, targetCameraAngle.z, Time.unscaledDeltaTime)

        );
        Camera.main.transform.eulerAngles = currentCameraAngle;
    }

}
