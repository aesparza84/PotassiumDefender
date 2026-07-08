using System.Collections;
using System.Reflection;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private MenuMusicSO menuMusicSO;
    [SerializeField] private LayeredMusicSO gameplayLayersSO;
    [SerializeField] private AudioSource[] MenuSources;
    [SerializeField] private AudioSource[] GameplaySources;

    private double currClipDur;
    private double goalTime;
    private bool usePrimarySource;
    private bool menuMode;

    //Gameplay fields
    private float layerTargetVol;
    private float currlayerLerp;

    private void Start()
    {
        menuMode = true;

        PlayMenuMusic(menuMusicSO, true, false);

        for (int i = 0; i < GameplaySources.Length; i++)
        {
            if (GameplaySources[i] != null)
                GameplaySources[i].loop = true;
        }

        GameCurator.OnInitializeGame += OnGameplayMusic;
        GameCurator.OnCleanUpGame += OnMenuMusic;
        FoodSupply.OnSupplyBelowHalf += OnLayerB;
    }

    private void OnLayerB()
    {
        layerTargetVol = 1;
    }

    private void OnDisable()
    {
        GameCurator.OnInitializeGame -= OnGameplayMusic;
        GameCurator.OnCleanUpGame -= OnMenuMusic;
        FoodSupply.OnSupplyBelowHalf -= OnLayerB;
    }
    private void OnMenuMusic()
    {
        menuMode = true;
        for (int i = 0; i < GameplaySources.Length; i++)
        {
            if (GameplaySources[i] != null)
            {
                GameplaySources[i].Stop();
                GameplaySources[i].volume = 0;
            }
        }

        layerTargetVol = 0.0f;

        PlayMenuMusic(menuMusicSO, true, true);
    }

    private void OnGameplayMusic(Transform obj)
    {
        menuMode = false;
        currlayerLerp = 0.0f;

        for (int i = 0; i < MenuSources.Length; i++)
        {
            if (MenuSources[i] != null)
                MenuSources[i].Stop();
        }

        PlayGameplayClip(gameplayLayersSO);
    }

    private void Update()
    {


        if (menuMode)
        {
            if (AudioSettings.dspTime > goalTime)
            {
                PlayMenuMusic(menuMusicSO, false, false);
            }
        }
        else
        {
            if (layerTargetVol != 0)
            {
                currlayerLerp += Time.deltaTime * 3;
                GameplaySources[1].volume = Mathf.Lerp(0, layerTargetVol, currlayerLerp);
            }
        }

    }

    private void PlayMenuMusic(MenuMusicSO music, bool isIntro, bool comingFromGameOver)
    {
        goalTime = AudioSettings.dspTime;

        AudioSource s;
        AudioClip clip;

        if (usePrimarySource)
        {
            s = MenuSources[0];
        }
        else
        {
            s = MenuSources[1];
        }

        if (isIntro)
        {
            if (comingFromGameOver)
            {
                clip = music.introB;
            }
            else
            {
                clip = music.introA;
            }
        }
        else
        {
            clip = music.loop;
        }

        s.clip = clip;
        s.volume = music.volume;
        s.pitch = music.pitch;
        s.spatialBlend = music.Spatial_Blend;

        s.PlayScheduled(goalTime);

        currClipDur = (double)clip.samples / clip.frequency;
        goalTime += currClipDur;

        usePrimarySource = !usePrimarySource;
    }
    private void PlayGameplayClip(LayeredMusicSO music)
    {
        AudioClip layerA = music.getLayerA();
        AudioClip layerB = music.getLayerB();

        if (GameplaySources.Length < 2)
        {
            Debug.Log("Missing gameplay source audio");
            return;
        }

        if (GameplaySources[0] != null)
        {
            GameplaySources[0].volume = 1;
            GameplaySources[0].clip = layerA;
            GameplaySources[0].Play();
        }
        
        if (GameplaySources[1] != null)
        {
            GameplaySources[1].volume = 0;
            GameplaySources[1].clip = layerB;
            GameplaySources[1].Play();
        }
    }
}
