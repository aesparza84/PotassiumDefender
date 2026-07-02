using UnityEngine;

public class AnimalFootSteps : MonoBehaviour
{
    [SerializeField] private SoundSO sound;

    [Range(.5f, 1f)]
    [SerializeField] private float stepInterval = 0.8f;
    private float currInterval;

    //External
    private Animal animal;

    private bool isMoving;
    private void Start()
    {
        if (animal == null)
            animal = GetComponent<Animal>();

        animal.OnApproach += OnMoving;
        animal.OnScurry += OnMoving;
        animal.OnEating += OnStopped;

        isMoving = true;
    }

    private void OnStopped()
    {
        isMoving = false;
    }

    private void OnMoving()
    {
        isMoving = true;
    }

    private void Update()
    {
        if (isMoving)
        {
            if (currInterval <= 0.0f)
            {
                currInterval = stepInterval;
                SoundManager.instance.PlaySound(sound, gameObject.transform);
            }
        }

        //Tick down event when not valid
        if (currInterval > 0.0f)
        {
            currInterval -= Time.deltaTime;
        }
    }

    private void OnDisable()
    {
        if (animal != null)
        {
            animal.OnApproach += OnMoving;
            animal.OnScurry += OnMoving;
            animal.OnEating += OnStopped;
        }
    }

}
