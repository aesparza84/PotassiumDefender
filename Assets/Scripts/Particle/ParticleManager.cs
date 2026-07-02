using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;

    [SerializeField] private GameObject bannanaImpact;

    private Stack<GameObject> availableParticles;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            return;
        }
    }

    private void Start()
    {
        availableParticles = new Stack<GameObject>();
    }

    public void PlayBanannaImpact(Vector3 pos)
    {
        GameObject b;
        ParticleSystem particle;

        if (availableParticles.Count > 0)
        {
            b = availableParticles.Pop();
        }
        else
        {
            b = Instantiate(bannanaImpact);
        }

        particle = b.GetComponent<ParticleSystem>();
        particle.transform.position = pos;
        particle.gameObject.SetActive(true);
        particle.Play();
        StartCoroutine(poolParticle(particle, particle.main.duration));
    }

    private IEnumerator poolParticle(ParticleSystem particle, float time)
    {
        yield return new WaitForSeconds(time);
        particle.Stop();
        particle.gameObject.SetActive(false);
        availableParticles.Push(particle.gameObject);
        yield return null;
    }
}
