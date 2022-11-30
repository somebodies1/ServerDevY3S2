using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    public int currentScore, highScore;
    [SerializeField] TextMeshProUGUI leaderboardText;

    private void Start()
    {
        GetHighScoreFromLeaderboard();
    }

    //to display in console
    void UpdateMsg(string msg)
    {
        Debug.Log(msg);
    }

    //Report any errors
    void OnError(PlayFabError e)
    {
        UpdateMsg("Error" + e.GenerateErrorReport());
    }

    public void OnButtonGetLeaderboard()
    {
        var lbreq = new GetLeaderboardRequest
        {
            StatisticName = "Highscore", //Playfab leaderboard name
            StartPosition = 0,
            MaxResultsCount = 10
        };

        PlayFabClientAPI.GetLeaderboard(lbreq, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult r)
    {
        string LeaderboardStr = "Leaderboard\n";

        foreach (var item in r.Leaderboard)
        {
            string onerow = (item.Position + 1) + " - " + item.DisplayName + " - " + item.StatValue + "\n";
            LeaderboardStr += onerow; //Combine all into one string
        }

        leaderboardText.text = LeaderboardStr;
    }

    public void OnButtonSendLeaderboard()
    {
        var req = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "Highscore",
                    Value = currentScore
                }
            }
        };

        UpdateMsg("Submitting score:" + currentScore);
        PlayFabClientAPI.UpdatePlayerStatistics(req, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult r)
    {
        UpdateMsg("Successful leaderboard sent:" + r.ToString());
    }

    void GetHighScoreFromLeaderboard()
    {
        var lbreq = new GetLeaderboardRequest
        {
            StatisticName = "Highscore", //Playfab leaderboard name
            StartPosition = 0,
            MaxResultsCount = 10
        };

        PlayFabClientAPI.GetLeaderboard(lbreq, OnLeaderboardGetHighScore, OnError);
    }

    void OnLeaderboardGetHighScore(GetLeaderboardResult r)
    {
        foreach (var item in r.Leaderboard)
        {
            if (item.Position == 0)
            {
                highScore = item.StatValue;
                Debug.Log("Highscore" + highScore);
                break;
            }
        }
    }
}
