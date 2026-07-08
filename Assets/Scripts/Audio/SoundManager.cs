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
        WarmMusicSources();
    }

    private void WarmMusicSources()
    {
        for (int i = 0; i < 4; i++)
        {
            AudioSource s = Instantiate(soundPrefab, transform.position, Quaternion.identity);
            s.playOnAwake = false;
            s.gameObject.SetActive(false);
            availableSoundPrefabs.Push(s);
        }
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

        s.transform.position = transform.position;
        s.clip = clipSO.getClip();
        s.pitch = clipSO.pitch;
        s.volume = clipSO.volume;
        s.spatialBlend = clipSO.Spatial_Blend;
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
