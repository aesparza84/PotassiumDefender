using UnityEngine;

[CreateAssetMenu(fileName = "WaveDifficulty", menuName = "Scriptable Objects/WaveDifficulty")]
public class WaveDifficulty : ScriptableObject
{
    [Range(0, 1)] public float mouseChance;
    [Range(0, 1)] public float batChance;
    [Range(0, 1)] public float pigChance;
}
