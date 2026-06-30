using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCameraBase alignCam, zoomCam, playerCam, menuCam; //Align and Zoom virtual cameras allow for the transition to occur.
                                                                                //Player and Menu Camera are determining who is going to be active.

    [SerializeField] private GameObject directionalLight;

    [SerializeField] private List<Transform> dirLightPositions; //The purpose is to move the directional light object based
                                                                //on where the main camera is going to be located (Menus or Gameplay) 

    [SerializeField]
    private float maxTimer, maxFadeTimer; //Transition Timer & Fade timer

    private float currentTimer; 

    [SerializeField]
    private Image fadeImage;

    private float fadeAmout;

    private bool isCameraActive;

    private string nextCameraName;

    [SerializeField]
    private MainMenu mainMenu; //Better to call the ChangeView method since it's public 

    void Awake()
    {
        maxFadeTimer = maxFadeTimer == 0 ? 0.5f : maxFadeTimer;
        currentTimer = maxTimer;
        SetActiveCameras(false, false);
        SetPlayerActive();

        playerCam.gameObject.SetActive(false);
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
        directionalLight.transform.position = dirLightPositions[0].position;
    }

    private void Update()
    {
        //Start Aligning the camera to face the sun
        if (!alignCam.IsParticipatingInBlend() && alignCam.gameObject.activeInHierarchy && alignCam.IsLive)
        {
            WaitTime(true, false, 0);
        }

        //Start zooming into the sun
        if (!zoomCam.IsParticipatingInBlend() && zoomCam.gameObject.activeInHierarchy && zoomCam.IsLive)
        {
            WaitTime(false, false, 1);
            
        }

        SwitchToNextMajorCamera(nextCameraName);
    }

    //This is assigned to the OnClick() button in the inspector
    //To either play or exit, give it the correct camera name
    public void CameraName(string name)
    {
        nextCameraName = name;
    }

   /// <summary>
   /// This gets the next main camera and first assign which virtual camera will be active.
   /// Afterwards, once the fade image is 1, start decreasing so the player can see again once the zoom camera finishes.
   /// </summary>
   /// <param name="cameraName"></param>
    private void SwitchToNextMajorCamera(string cameraName)
    {
        CinemachineVirtualCameraBase vc = null;

        switch (cameraName)
        {
            case "PlayerCamera":
                vc = playerCam;
                directionalLight.transform.position = dirLightPositions[0].position; 
                break;

            case "MenuCamera":
                vc = menuCam;
                directionalLight.transform.position = dirLightPositions[1].position;
                break;
        }

        if (fadeImage.color.a > 0 && !zoomCam.IsLive)
        {
            if (!vc.IsLive && !isCameraActive) 
            {
                mainMenu.ChangeView(nextCameraName);
                isCameraActive = true;
                
            }

            fadeAmout -= Time.deltaTime * maxFadeTimer;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeAmout);
        }
    }

    private void SetPlayerActive()
    {
        playerCam.gameObject.SetActive(false);
    }

    //Waiting to transition from the aim camera to the zoom camera
    private void WaitTime(bool activateZoom, bool activateAlign, int num)
    {
        if (currentTimer > 0) 
        {
            currentTimer -= Time.deltaTime;
        }
        else
        {
            currentTimer = maxTimer;
            SetActiveCameras(activateZoom, activateAlign);
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, num);
            fadeAmout = num;
            isCameraActive = false;
        }
    }

    private void SetActiveCameras(bool isZoomActive, bool isAlignActive)
    {
        zoomCam.gameObject.SetActive(isZoomActive);
        alignCam.gameObject.SetActive(isAlignActive);
    }
}
