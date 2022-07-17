using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Leaderboard : MonoBehaviour
{
    Dictionary<string, int> leaderboard;
    // this is so dumb but it's kinda all screwy now and no time
    public TMPro.TMP_Text leaderboardUI;
    public TMPro.TMP_Text pauseLeaderboardUI;
    // Start is called before the first frame update
    void Start()
    {
        leaderboard = new Dictionary<string, int>() {
            {"Patt Percer", 99},
            {"YOU", 3},
            {"Mulligan", 99},
            {"Arnac", 77},
            {"Tiamat", 12},
            {"Phil Pheaton", 0},
        };
    }

    public void UpdateLeaderboard(int playerFollowers) {
        Dictionary<string, int> tempLeaderboard = new Dictionary<string, int>();
        leaderboard["YOU"] = playerFollowers;
        tempLeaderboard["YOU"] = playerFollowers;
        leaderboardUI.text = "";
        pauseLeaderboardUI.text = "";
        var sortedLeaderboard = from entry in leaderboard orderby entry.Value descending select entry;
        foreach(var item in sortedLeaderboard) {
            if (item.Key == "YOU") 
            {
                leaderboardUI.text += $"<color=#FF0000>YOU: {item.Value} Loyal Customers</color>\n\n";
                pauseLeaderboardUI.text += $"<color=#FF0000>YOU: {item.Value} Loyal Customers</color>\n\n";
            }
            else {
                leaderboardUI.text += $"{item.Key}: {item.Value} Loyal Customers\n\n";
                pauseLeaderboardUI.text += $"{item.Key}: {item.Value} Loyal Customers\n\n";
                // Add some random change for next go round as well
                int randomAddition = Random.Range(-10, 10);
                int newVal = Mathf.Clamp(item.Value + randomAddition, 0, 99);
                tempLeaderboard[item.Key] = newVal;
            }
        }
        leaderboard = tempLeaderboard;
    }
}
