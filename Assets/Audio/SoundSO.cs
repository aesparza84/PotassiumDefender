using UnityEngine;

[CreateAssetMenu(fileName = "SoundSO", menuName = "Scriptable Objects/SoundSO")]
public class SoundSO : ScriptableObject
{
    public AudioClip[] clips;

    [Range(0, 1)]
    public float volume = 1; 
    
    [Range(-3, 3)]
    public float pitch = 1;

    [Range(0, 1)]
    public float Spatial_Blend = 1;
    public AudioClip getClip()
    {
        if (clips.Length == 1)
            return clips[0];

        int choice = Random.Range(0, clips.Length);
        return clips[choice];
    }
}
