using System.Collections;
using System.Reflection;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private MusicSO menuMusicSO;
    [SerializeField] private AudioSource[] sources;

    private double currClipDur;
    private double goalTime;
    private bool usePrimarySource;
    
    private void Start()
    {
        PlayClip(menuMusicSO, true);
    }

    private void Update()
    {
        if (AudioSettings.dspTime > goalTime)
        {
            PlayClip(menuMusicSO, false);
        }
    }


    private void PlayClip(MusicSO music, bool isIntro)
    {
        goalTime = AudioSettings.dspTime;

        AudioSource s;
        AudioClip clip;

        if (usePrimarySource)
        {
            s = sources[0];
        }
        else
        {
            s = sources[1];
        }

        if (isIntro)
        {
            clip = music.intro;
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
}
