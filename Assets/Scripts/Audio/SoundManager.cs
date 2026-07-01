using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource soundPrefab;

    private Stack<AudioSource> availableSoundPrefabs;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        availableSoundPrefabs = new Stack<AudioSource>();
    }

    public void PlaySound(SoundSO clipSO, Transform transform)
    {
        AudioSource s;
        
        if (availableSoundPrefabs.Count > 0)
        {
            s = availableSoundPrefabs.Pop();
        }
        else
        {
            s = Instantiate(soundPrefab, transform.position, Quaternion.identity);
            s.playOnAwake = false;
        }

        if (!s.isActiveAndEnabled)
            s.gameObject.SetActive(true);

        s.clip = clipSO.getClip();
        s.volume = clipSO.volume;
        s.Play();

        float len = s.clip.length;

        StartCoroutine(DisableClip(s, len));
    }

    private IEnumerator DisableClip(AudioSource s, float time)
    {
        yield return new WaitForSeconds(time);
        s.Stop();
        s.gameObject.SetActive(false);
        availableSoundPrefabs.Push(s);
        yield return null;
    }
}
