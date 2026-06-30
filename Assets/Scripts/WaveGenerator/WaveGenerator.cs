using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;

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
    [SerializeField] private float spawnDelay;
    private float currSpawnDelay;

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
    private int difficultyIndex;

    /// <summary>
    /// How many rounds before increasing difficulty
    /// </summary>
    [SerializeField] private int waveDifficultyStepIncrease;
    
    /// <summary>
    /// Current wave iteration
    /// </summary>
    private int waveCount;

    /// <summary>
    /// How long the generator waits before executing next wave
    /// </summary>
    [SerializeField] private float nextWaveDelay;
    private float currNextWaveDelay;

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

    private void Start()
    {
        availablePigs = new Stack<GameObject>();
        availableMice = new Stack<GameObject>();
        availableBats = new Stack<GameObject>();

        weightMap = new Dictionary<System.Type, int>();
        weightMap.Add(typeof(Pig), pigWeight);
        weightMap.Add(typeof(Bat), batWeight);
        weightMap.Add(typeof(Mice), mouseWeight);

        currSpawnDelay = 0.0f;
        difficultyIndex = 0;
        currDifficulty = difficulties[difficulties.Length-1];

        SwitchToState(WaveState.GENERATING);
    }

    private void Update()
    {
        switch (waveState)
        {
            case WaveState.GENERATING:
                if (currSpawnDelay > 0.0f)
                {
                    currSpawnDelay -= Time.deltaTime;
                }
                else
                {
                    currSpawnDelay = spawnDelay;
                    spawnAnimal();
                }
                
                if (waveSpecificWeight >= expectedWeight)
                    SwitchToState(WaveState.ACTIVE);
                
                break;
            case WaveState.ACTIVE:

                if (currWeight == 0)
                    SwitchToState(WaveState.SWITCHING);

                break;
            case WaveState.SWITCHING:
                if (currNextWaveDelay > 0.0f)
                {
                    currNextWaveDelay -= Time.deltaTime;
                }
                else
                {
                    SwitchToState(WaveState.GENERATING);
                }

                waveSpecificWeight = 0;
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

                if (waveCount > 0)
                {
                    expectedWeight = Mathf.RoundToInt(expectedWeight * multiplier);
                }
                else
                {
                    expectedWeight = baseWeightLimit;
                }

                currSpawnDelay = spawnDelay;
                break;
            case WaveState.ACTIVE:
                break;
            case WaveState.SWITCHING:
                currNextWaveDelay = nextWaveDelay;

                if (waveCount > 0 && waveCount % waveDifficultyStepIncrease == 0)
                    difficultyIndex++;

                if (difficultyIndex < difficulties.Length)
                    currDifficulty = difficulties[difficultyIndex];
                
                waveCount++;
                break;
            default:
                break;
        }
    }

    private void spawnAnimal()
    {
        Vector3 spawnPos = chooseSpawnPos();
        Vector3 dirToSupply = FoodSupplyTransform.position - spawnPos;
        Quaternion lookRot = Quaternion.LookRotation(dirToSupply.normalized);

        GameObject animalObj;
        Animal animal;
        
        float rand = Random.Range(0.0f, 1.0f);

        if (rand < currDifficulty.mouseChance)
        {
            animalObj = getMouse();
        }
        else
        {
            rand -= currDifficulty.mouseChance;
            if (rand < currDifficulty.batChance)
            {
                if (batWeight + waveSpecificWeight > expectedWeight)
                {
                    Debug.Log("Bat overfill - SKIP");
                    return;
                }

                float height = Random.Range(4.0f, 6.0f);
                spawnPos.y = height;

                animalObj = getBat();
            }
            else
            {
                if (pigWeight + waveSpecificWeight > expectedWeight)
                {
                    Debug.Log("Pig overfill - SKIP");
                    return;
                }

                animalObj = getPig();
            }
        }
        
        animal = animalObj.GetComponent<Animal>();

        //Adding to wave expected AND current weight
        int w = weightMap[animal.GetType()];
        waveSpecificWeight += w;
        currWeight += w;


        animalObj.transform.position = spawnPos;
        animalObj.transform.rotation = lookRot;

        animalObj.SetActive(true);
        animal.SetSupplyTransform(FoodSupplyTransform);
        animal.SetDefaults();

        animal.OnFilled += Animal_OnFilled;
        animal.OnAnimalDisable += Animal_OnAnimalDisable;
    }

    private void Animal_OnAnimalDisable(GameObject obj, Animal animInst)
    {
        animInst.OnAnimalDisable -= Animal_OnAnimalDisable;

        System.Type t = animInst.GetType();
        
        if (t == typeof(Pig))
        {
            availablePigs.Push(obj);
        }
        else if(t == typeof(Bat))
        {
            availableBats.Push(obj);
        }
        else
        {
            availableMice.Push(obj);
        }
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
            Debug.Log("Pulled from Pool - MOUSE");
            mouse = availableMice.Pop();           
        }
        else
        {
            mouse = Instantiate(MicePrefab);
        }

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
        
        return pig;
    }
    private Vector3 chooseSpawnPos()
    {
        return customBorder.GetPoint();
    }
}
