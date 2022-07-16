using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Leaderboard : MonoBehaviour
{
    Dictionary<string, int> leaderboard;
    public TMPro.TMP_Text leaderboardUI;
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
        var sortedLeaderboard = from entry in leaderboard orderby entry.Value descending select entry;
        foreach(var item in sortedLeaderboard) {
            leaderboardUI.text += $"{item.Key}: {item.Value + ((item.Key == "YOU") ? 0 : Random.Range(-20, 20))} Followers\n";
        }
    }
}
