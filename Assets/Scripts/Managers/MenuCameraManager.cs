using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
using Unity.Cinemachine;

public enum menuType { MainMenu, QuitMenu, 
    LeaderboardMenu, SettingsMenu, 
    GameplayMenu, RestartMenu, TransitionMenu}

public class MenuCameraManager : MonoBehaviour
{
    public List<VirtualCameraMenu> menuVirtualCameras;

    [SerializeField]
    private bool didPlayerLose; //Temp

    [SerializeField]
    private ScoreTracker tracker; //temp placement

    void Awake()
    {
        UpdateCurrentCamera(menuType.MainMenu);
    }

    private void Update()
    {
        if (didPlayerLose)
        {
            tracker.CalculateTotalResult();
            UpdateCurrentCamera(menuType.RestartMenu);
        }
        
    }

    private void OnEnable()
    {
        MainMenu.UpdateCamera += UpdateCurrentCamera;
    }

    private void OnDisable()
    {
        MainMenu.UpdateCamera -= UpdateCurrentCamera;
    }


    /// <summary>
    /// First, ensure the cursor is unlocked when viewing the menus. This was added
    /// since the players script would lock the cursor, even the scripts were deactivated
    /// 
    /// Second, go through the list and activate the selected camera and deactivate the rest.
    /// </summary>
    /// <param name="menu"></param>
    private void UpdateCurrentCamera(menuType menu)
    {
        switch (menu)
        { 
            case menuType.MainMenu:
            case menuType.QuitMenu:
            case menuType.LeaderboardMenu:
            case menuType.SettingsMenu:
            case menuType.RestartMenu:
                Cursor.lockState = CursorLockMode.None;
                didPlayerLose = false;
                break;
            
            case menuType.GameplayMenu:
            case menuType.TransitionMenu:
                Cursor.lockState = CursorLockMode.Locked;
                break;

        }

        foreach (var menuItem in menuVirtualCameras) 
        {
            if (menuItem.currentMenu == menu) 
            {
                menuItem.menuCamera.SetActive(true);
                
            }
            else
            {
                menuItem.menuCamera.SetActive(false);

            }     
        }
    }
    
    
}

[Serializable]
public class VirtualCameraMenu
{
    public GameObject menuCamera;
    public menuType currentMenu;

}