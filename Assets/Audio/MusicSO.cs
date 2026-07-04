using UnityEngine;

[CreateAssetMenu(fileName = "MusicSO", menuName = "Scriptable Objects/MusicSO")]
public class MusicSO : ScriptableObject
{
    public AudioClip intro;
    public AudioClip loop;

    [Range(0, 1)]
    public float volume = 1;

    [Range(-3, 3)]
    public float pitch = 1;

    [Range(0,1)]
    public float Spatial_Blend = 0;

    public AudioClip getIntro()
    {
        return intro;
    }

    public AudioClip getLoop()
    {
        return loop;
    }
}
