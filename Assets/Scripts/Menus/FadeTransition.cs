using NUnit.Framework;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class FadeTransition : MonoBehaviour
{
    public List<CinemachineVirtualCameraBase> vCams;

    [SerializeField]
    private float currentTimer, maxTimer;

    bool isCompleted;

    void Awake()
    {
        currentTimer = maxTimer;

        foreach(var cam in vCams)
        {
            cam.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!vCams[0].IsParticipatingInBlend() && vCams[0].gameObject.activeInHierarchy && !isCompleted)
        {
            WaitTime();
        }
        
    }

    private void WaitTime()
    {
        if (currentTimer > 0) 
        {
            currentTimer -= Time.deltaTime;
        }
        else
        {
            vCams[1].gameObject.SetActive(true);
            currentTimer = maxTimer;
            isCompleted = true;
            /*foreach (var cam in vCams)
            {
                cam.gameObject.SetActive(false);
            }*/
        }

            
    }
    



}
