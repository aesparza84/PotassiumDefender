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
                UpdateCamera?.Invoke(menuType.GameplayMenu);
            break;
            
            case "MainMenu":
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


        }

    }

    
    
}

