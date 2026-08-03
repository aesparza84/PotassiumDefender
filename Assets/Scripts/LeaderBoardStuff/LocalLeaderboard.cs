using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

public class LocalLeaderboard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] boardEntries;

    [Header("Submission")]
    [SerializeField] private TextMeshProUGUI scoreField;
    [SerializeField] private TMP_InputField userNameInputField;

    private List<PlayerInfo> entries;
    private int newScore;
    private string username;

    private const string LB_Key = "Leaderboard";
    private void Start()
    {
        entries = new List<PlayerInfo>();
        ScoreTracker.UpdateScoreUI += OnNewScore;
    }

    private void OnDisable()
    {
        ScoreTracker.UpdateScoreUI -= OnNewScore;
    }

    private void OnNewScore(int arg1, int arg2, int arg3, int arg4, float arg5)
    {
        if (scoreField != null)
            scoreField.text = arg1.ToString();


        newScore = arg1;
    }

    public void OnSubmitNameAndScore()
    {
        UpdateName();
        AddScore(newScore);

        PlayerInfo entry = new PlayerInfo(username, newScore);
        entries.Add(entry);

        //Playerprefs saving
        StringBuilder sb = new StringBuilder();

        foreach (PlayerInfo i in entries)
        {
            sb.Append(i.Name+",");
            sb.Append(i.Score+ ",");
        }

        //SAVE leaderboard state
        PlayerPrefs.SetString(LB_Key, sb.ToString());

        SortLeaderBoard();
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

        
    }

    private void AddScore(int score)
    {
        newScore = score;
    }

    public void RefreshLeaderboard()
    {
        entries.Clear();

        //Clear entry fields
        for (int i = 0; i < boardEntries.Length; i++)
        {
            boardEntries[i].text = "";
        }

        string stringStats = PlayerPrefs.GetString(LB_Key,"");

        string[] parsed = stringStats.Split(',');

        for (int i = 0; i < parsed.Length - 2; i += 2)
        {
            PlayerInfo loaded = new PlayerInfo(parsed[i], int.Parse(parsed[i + 1]));
            entries.Add(loaded);
        }

        SortLeaderBoard();

        //Add latest entries to leaderboard
        int len = entries.Count < boardEntries.Length ? entries.Count : boardEntries.Length;

        for (int i = 0; i < len; i++)
        {
            string formattedName = entries[i].Name.ToUpper().Substring(0, 3);
            boardEntries[i].text = $"{formattedName} - {entries[i].Score.ToString()}";
        }
    }

    private void SortLeaderBoard()
    {
        if (entries == null)
            return;

        entries = entries.OrderByDescending(x => x.Score).ToList();
    }

    public void ClearLeaderBoard()
    {
        PlayerPrefs.DeleteAll();

        RefreshLeaderboard();
    }
}
