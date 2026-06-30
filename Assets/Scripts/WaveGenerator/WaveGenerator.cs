using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaveGenerator : MonoBehaviour
{
    /// <summary>
    /// Current state of the generator
    /// </summary>
    private WaveState waveState;

    /// <summary>
    /// Currently active animals
    /// </summary>
    private int currWeight;

    /// <summary>
    /// Weight for current wave
    /// </summary>
    private int expectedWeight;

    /// <summary>
    /// Weights added on new wave. When reaches expectedWeight, stop spawning
    /// </summary>
    private int waveSpecificWeight;

    /// <summary>
    /// Delay between spawning/enabling animals in wave
    /// </summary>
    [SerializeField] private float delay;
    private float currDelay;

    /// <summary>
    /// Multiplier applied to baseAmount each wave iteration
    /// </summary>
    [SerializeField] private float multiplier = 1.5f;

    /// <summary>
    /// Starting spawn count to multiply each wave
    /// </summary>
    [SerializeField] private int baseWeightLimit; //base weight - wave 1

    [Header("Wave Difficulty")]
    [SerializeField] private WaveDifficulty[] difficulties;
    private WaveDifficulty currDifficulty;

    [Header("Animal Prefabs")]
    [SerializeField] private GameObject PigPrefab;
    [SerializeField] private GameObject BatPrefab;
    [SerializeField] private GameObject MicePrefab;

    [Header("Spawn Weights")]
    [SerializeField] private int mouseWeight;
    [SerializeField] private int batWeight;
    [SerializeField] private int pigWeight;

    [Header("External")]
    [SerializeField] private Transform FoodSupplyTransform;
    [SerializeField] private CustomBorder customBorder;

    private Dictionary<System.Type, int> weightMap;

    //Animal Pools
    private Stack<GameObject> availablePigs;
    private Stack<GameObject> availableBats;
    private Stack<GameObject> availableMice;

    //Active animals - current wave
    private List<GameObject> activeAnimals;
    private void Start()
    {
        availablePigs = new Stack<GameObject>();
        availableMice = new Stack<GameObject>();
        availableBats = new Stack<GameObject>();

        activeAnimals = new List<GameObject>();

        weightMap = new Dictionary<System.Type, int>();
        weightMap.Add(typeof(Pig), pigWeight);
        weightMap.Add(typeof(Bat), batWeight);
        weightMap.Add(typeof(Mice), mouseWeight);

        currDelay = 0.0f;

        currDifficulty = difficulties[difficulties.Length-1];

        SwitchToState(WaveState.GENERATING);
    }

    private void Update()
    {
        switch (waveState)
        {
            case WaveState.GENERATING:
                if (currDelay > 0.0f)
                {
                    currDelay -= Time.deltaTime;
                }
                else
                {
                    currDelay = delay;
                    spawnAnimal();
                }
                
                if (waveSpecificWeight >= expectedWeight)
                    SwitchToState(WaveState.ACTIVE);
                
                break;
            case WaveState.ACTIVE:

                if (currWeight == 0)
                    SwitchToState(WaveState.GENERATING);

                break;
            default:
                break;
        }
    }

    private void SwitchToState(WaveState state)
    {
        this.waveState = state;

        switch (state)
        {
            case WaveState.GENERATING:
                //expectedWeight = Mathf.RoundToInt(expectedWeight * multiplier);
                expectedWeight = baseWeightLimit;
                currDelay = delay;

                break;
            
            case WaveState.ACTIVE:

                
                break;
            default:
                break;
        }
    }

    private Animal spawnAnimal()
    {
        Vector3 spawnPos = chooseSpawnPos();
        Vector3 dirToSupply = FoodSupplyTransform.position - spawnPos;
        Quaternion lookRot = Quaternion.LookRotation(dirToSupply.normalized);

        GameObject animalObj;
        Animal animal;

        /*
        int rand = Random.Range(0, 3);
        switch (rand)
        {
            case 0:
                animalObj = getMouse();
                break;
            case 1:
                float height = Random.Range(4.0f, 6.0f);
                spawnPos.y = height;

                animalObj = getBat();
                break;
            case 2:
                animalObj = getMouse();
                break;
            default:
                animalObj = getMouse();
                break;
        }
        */
        animalObj = getMouse();
        animal = animalObj.GetComponent<Animal>();


        int w = weightMap[animal.GetType()];
        waveSpecificWeight += w;


        animalObj.transform.position = spawnPos;
        animalObj.transform.rotation = lookRot;

        animalObj.SetActive(true);
        animal.SetSupplyTransform(FoodSupplyTransform);
        animal.SetDefaults();

        animal.OnFilled += Animal_OnFilled;
        return animal;
    }

    private void Animal_OnFilled(Animal animInst)
    {
        //Debug.Log($"type - {animType}");

        currWeight -= weightMap[animInst.GetType()];
        animInst.OnFilled -= Animal_OnFilled;
    }

    private GameObject getMouse()
    {
        GameObject mouse;
        if (availableMice.Count > 0)
        {
            mouse = availableMice.Pop();           
        }
        else
        {
            mouse = Instantiate(MicePrefab);
        }

        //Add to active LIST
        activeAnimals.Add(mouse);
        
        return mouse;
    }

    private GameObject getBat()
    {
        GameObject bat;
        if (availableBats.Count > 0)
        {
            bat = availableBats.Pop();
        }
        else
        {
            bat = Instantiate(BatPrefab);
        }

        activeAnimals.Add(bat);
        return bat;
    }

    private GameObject getPig()
    {
        GameObject pig;
        if (availablePigs.Count > 0)
        {
            pig = availablePigs.Pop();
        }
        else
        {
            pig = Instantiate(PigPrefab);
        }

        activeAnimals.Add(pig);
        return pig;
    }
    private Vector3 chooseSpawnPos()
    {
        return customBorder.GetPoint();
    }
}
