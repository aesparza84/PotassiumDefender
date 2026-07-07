using UnityEngine;

[CreateAssetMenu(fileName = "MusicSO", menuName = "Scriptable Objects/MusicSO")]
public class MenuMusicSO : ScriptableObject
{
    public AudioClip introA;
    public AudioClip introB;
    public AudioClip loop;

    [Range(0, 1)]
    public float volume = 1;

    [Range(-3, 3)]
    public float pitch = 1;

    [Range(0,1)]
    public float Spatial_Blend = 0;

    public AudioClip getIntroA()
    {
        return introA;
    }
    public AudioClip getIntroB()
    {
        return introB;
    }

    public AudioClip getLoop()
    {
        return loop;
    }
}
