using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;
using Unity.Cinemachine;

public enum menuType { MainMenu, QuitMenu, LeaderboardMenu, SettingsMenu, GameplayMenu, RestartMenu}

public class MenuCameraManager : MonoBehaviour
{
    public List<VirtualCameraMenu> menuVirtualCameras;

    
    int playerCamPosition;

    void Start()
    {
        UpdateCurrentCamera(menuType.MainMenu);

        playerCamPosition = menuVirtualCameras.FindIndex(x => x.currentMenu == menuType.GameplayMenu);
    }

    private void OnEnable()
    {
        MainMenu.UpdateCamera += UpdateCurrentCamera;
    }

    private void OnDisable()
    {
        MainMenu.UpdateCamera -= UpdateCurrentCamera;
    }


    private void UpdateCurrentCamera(menuType menu)
    {
        

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