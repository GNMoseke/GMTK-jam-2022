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
            {"Patt Percer", 200},
            {"YOU", 3},
            {"Mulligan", 200},
            {"Arnac", 150},
            {"Tiamat", 30},
            {"Phil Pheaton", -20},
        };
    }

    public void UpdateLeaderboard(int playerFollowers) {
        leaderboard["YOU"] = playerFollowers;
        leaderboardUI.text = "";
        pauseLeaderboardUI.text = "";
        var sortedLeaderboard = from entry in leaderboard orderby entry.Value descending select entry;
        foreach(var item in sortedLeaderboard) {
            if (item.Key == "YOU") 
            {
                leaderboardUI.text += $"<color=#FF0000>YOU: {item.Value} Loyal Customers</color>\n";
                pauseLeaderboardUI.text += $"<color=#FF0000>YOU: {item.Value} Loyal Customers</color>\n";
            }
            else {
                leaderboardUI.text += $"{item.Key}: {item.Value + Random.Range(-20, 20)} Loyal Customers\n";
                pauseLeaderboardUI.text += $"{item.Key}: {item.Value + Random.Range(-20, 20)} Loyal Customers\n";
            }
        }
    }
}
