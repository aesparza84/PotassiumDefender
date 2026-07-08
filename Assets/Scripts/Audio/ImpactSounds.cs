using UnityEngine;

public class ImpactSounds : MonoBehaviour
{
    [SerializeField] private SoundSO impactSO;

    public void OnImpact(Transform t)
    {
        if (impactSO != null)
            SoundManager.instance.PlaySound(impactSO, t);
    }
}
