using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCameraBase alignCam, zoomCam, playerCam, restartCam;

    [SerializeField]
    private float maxTimer, maxFadeTimer;

    private float currentTimer;

    [SerializeField]
    private Image fadeImage;

    private float fadeAmout;

    private bool isPlayerCameraActive;

    void Awake()
    {
        maxFadeTimer = maxFadeTimer == 0 ? 0.5f : maxFadeTimer;
        currentTimer = maxTimer;
        SetActiveCameras(false, false);
        
        playerCam.gameObject.SetActive(false);
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
    }

    private void Update()
    {
        if (!alignCam.IsParticipatingInBlend() && alignCam.gameObject.activeInHierarchy && alignCam.IsLive)
        {
            WaitTime(true, false, 0);
        }

        if (!zoomCam.IsParticipatingInBlend() && zoomCam.gameObject.activeInHierarchy && zoomCam.IsLive)
        {
            WaitTime(false, false, 1);
        }

        SwitchToPlayerCamera();

    }


    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    //
    private void SwitchToPlayerCamera()
    {
        if (fadeImage.color.a > 0 && !zoomCam.IsLive)
        {
            if (!playerCam.IsLive && !isPlayerCameraActive)
            {
                playerCam.gameObject.SetActive(true);
                isPlayerCameraActive = true;
            }

            fadeAmout -= Time.deltaTime * maxFadeTimer;
            fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeAmout);
        }
    }

    private void SetPlayerActive()
    {
        playerCam.gameObject.SetActive(false);
        isPlayerCameraActive = false;
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
        }
    }

    private void SetActiveCameras(bool isZoomActive, bool isAlignActive)
    {
        zoomCam.gameObject.SetActive(isZoomActive);
        alignCam.gameObject.SetActive(isAlignActive);
    }
}
