using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

public enum menuType { MainMenu, QuitMenu, LeaderboardMenu, SettingsMenu, GameplayMenu}

public class MenuCameraManager : MonoBehaviour
{
    public List<VirtualCameraMenu> menuVirtualCameras;

    void Start()
    {
        UpdateCurrentCamera(menuType.MainMenu);
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
                //return;
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