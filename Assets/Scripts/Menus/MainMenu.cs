using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static event Action<menuType> UpdateCamera;

    public void ChangeView(string menu)
    {
        switch (menu) 
        {
            case "GameplayMenu":
            case "PlayerCamera":
                UpdateCamera?.Invoke(menuType.GameplayMenu);
            break;
            
            case "MainMenu":
            case "MenuCamera":
                UpdateCamera?.Invoke(menuType.MainMenu);
            break;

            case "QuitMenu":
                UpdateCamera?.Invoke(menuType.QuitMenu);
            break;

            case "LeaderboardMenu":
                UpdateCamera?.Invoke(menuType.LeaderboardMenu);
            break;

            case "SettingsMenu":
                UpdateCamera?.Invoke(menuType.SettingsMenu);
            break;
            
            case "TransitionMenu":
                UpdateCamera?.Invoke(menuType.TransitionMenu);
            break;

            case "RestartMenu":
                UpdateCamera?.Invoke(menuType.RestartMenu);
                break;
        }
    }
}

