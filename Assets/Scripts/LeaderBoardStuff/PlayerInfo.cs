using UnityEngine;

public class PlayerInfo
{
    private string name;
    private int score;

    public string Name { get { return name; } }
    public int Score { get { return score; } }

    public PlayerInfo(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}
