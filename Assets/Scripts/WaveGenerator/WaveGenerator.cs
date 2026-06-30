using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour
{
    /// <summary>
    /// Current state of the generator
    /// </summary>
    private WaveState waveState;

    /// <summary>
    /// Currently active animals
    /// </summary>
    private int amount;

    /// <summary>
    /// Delay between spawning/enabling animals in wave
    /// </summary>
    [SerializeField] private float delay;
    private float currDelay;

    /// <summary>
    /// Multiplier applied to baseAmount each wave iteration
    /// </summary>
    private float multiplier;

    /// <summary>
    /// Starting spawn count to multiply each wave
    /// </summary>
    [SerializeField] private int baseWeightLimit; //base weight - wave 1

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
    }

    private void spawnMouse() //Pre-checked weight
    {
        Vector3 spawnPos = chooseSpawnPos();
        Vector3 dirToSupply = FoodSupplyTransform.position - spawnPos;
        Quaternion lookRot = Quaternion.LookRotation(dirToSupply.normalized);

        GameObject mouse = getMouse();
        Animal a = mouse.GetComponent<Animal>();

        //Reposition & Rotate
        mouse.transform.position = spawnPos;
        mouse.transform.rotation = lookRot;

        mouse.SetActive(true);
        a.SetDefaults();
    }
    private void spawnBat() //Pre-checked weight
    {
        Vector3 spawnPos = chooseSpawnPos();
        Vector3 dirToSupply = FoodSupplyTransform.position - spawnPos;
        Quaternion lookRot = Quaternion.LookRotation(dirToSupply.normalized);

        GameObject bat = getMouse();
        Animal a = bat.GetComponent<Animal>();

        //Reposition & Rotate
        bat.transform.position = spawnPos;
        bat.transform.rotation = lookRot;

        bat.SetActive(true);
        a.SetDefaults();
    }
    private void spawnPig() //Pre-checked weight
    {
        Vector3 spawnPos = chooseSpawnPos();
        Vector3 dirToSupply = FoodSupplyTransform.position - spawnPos;
        Quaternion lookRot = Quaternion.LookRotation(dirToSupply.normalized);

        GameObject pig = getMouse();
        Animal a = pig.GetComponent<Animal>();

        //Reposition & Rotate
        pig.transform.position = spawnPos;
        pig.transform.rotation = lookRot;

        pig.SetActive(true);
        a.SetDefaults();
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
