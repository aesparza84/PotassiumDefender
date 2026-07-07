using UnityEngine;

[CreateAssetMenu(fileName = "LayeredMusic", menuName = "Scriptable Objects/LayeredMusic")]
public class LayeredMusicSO : ScriptableObject
{
    public AudioClip loopLayerA;
    public AudioClip loopLayerB;

    [Range(0, 1)]
    public float volume = 1;

    [Range(-3, 3)]
    public float pitch = 1;

    [Range(0, 1)]
    public float Spatial_Blend = 0;

    public AudioClip getLayerA()
    {
        return loopLayerA;
    }
    public AudioClip getLayerB()
    {
        return loopLayerB;
    }
}
