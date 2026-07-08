using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class LeaderBoardManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] boardEntries;


    [Header("Submission")]
    [SerializeField] private TextMeshProUGUI scoreField;
    [SerializeField] private TMP_InputField userNameInputField;

    private int newScore;
    private string username;

    private const string leaderboard_id = "Tassium_Board_jN6yYsPVVBbgQs";

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        //Start with default score
            //await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboard_id,0);
        AddScore(0);
        
        ScoreTracker.UpdateScoreUI += OnNewScore;
    }

    private void OnDisable()
    {
        ScoreTracker.UpdateScoreUI -= OnNewScore;
    }

    private async void OnNewScore(int arg1, int arg2, int arg3, int arg4, float arg5)
    {
        if (scoreField != null)
            scoreField.text = arg1.ToString();

        newScore = arg1;
    }

    public async void OnCallRefresh()
    {
        RefreshLeaderboard();
    }

    public void OnSubmitNameAndScore()
    {
        UpdateName();
        AddScore(newScore);
    }

    public void OnNameChanged(string name)
    {
        name = name.ToUpper().Trim();

        userNameInputField.SetTextWithoutNotify(name);
    }

    public void OnDeselect(string name)
    {
        if (name.Length < 3)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(name);

            int diff = 3 - name.Length;
            for (int i = 0; i < diff; i++)
            {
                sb.Append("A");
            }

            name = sb.ToString();
        }
        else if (name.Length > 3)
        {
            name = name.Substring(0, 3);
        }

        userNameInputField.SetTextWithoutNotify(name);
    }
    private void UpdateName()
    {
        if (userNameInputField != null)
        {
            username = userNameInputField.text;
        }

        if (username == string.Empty)
            username = "AAA";

        if (username.Length < 3)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(username);

            int diff = 3 - username.Length;
            for (int i = 0; i < diff; i++)
            {
                sb.Append("A");
            }

            username = sb.ToString();
        }
        else if (username.Length > 3)
        {
            username = username.Substring(0, 3);
        }

        AuthenticationService.Instance.UpdatePlayerNameAsync(username);
    }

    private async void AddScore(int score)
    {
        try
        {
            await LeaderboardsService.Instance.AddPlayerScoreAsync(leaderboard_id, score);

            RefreshLeaderboard();
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            throw;
        }
    }

    public async void RefreshLeaderboard()
    {
        LeaderboardScoresPage scoresPage = await LeaderboardsService.Instance.GetScoresAsync(leaderboard_id);

        //Empty out curr leaderboard
        for (int i = 0; i < boardEntries.Length; i++)
        {
            boardEntries[i].text = "";
        }

        int len = scoresPage.Total < boardEntries.Length ? scoresPage.Total : boardEntries.Length;

        //Add latest entries to leaderboard
        for (int i = 0; i < len; i++)
        {
            string formattedName = scoresPage.Results[i].PlayerName.ToUpper().Substring(0, 3);
            boardEntries[i].text = $"{formattedName} - {scoresPage.Results[i].Score.ToString()}";
        }
    }
}
