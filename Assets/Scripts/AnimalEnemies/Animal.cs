using UnityEngine;
using UnityEngine.AI;

public abstract class Animal : MonoBehaviour
{
    [SerializeField] private string animalName;
    public string AnimalName { get { return animalName; } set { animalName = value; } }
    
    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
    
    [SerializeField] private int animalPointAmount;
    public int AnimalPointAmount { get { return animalPointAmount; } set { animalPointAmount = value; } }
    
    [SerializeField] private int animalHungerMax;
    public int AnimalHungerMax { get { return animalHungerMax; } set { animalHungerMax = value; } }
    
    public int AnimalHunger { get; /*{ return animalHunger; }*/ set; /*{ animalHunger = value; }*/ }

    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

    [SerializeField] private float movementSpeed;
    public float MovementSpeed { get { return movementSpeed; } set { movementSpeed = value; } }


    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

    [SerializeField] private Transform endGoal;
    public Transform EndGoal { get { return endGoal; } set { endGoal = value; } }

    public Rigidbody rig { get; set; }
    public NavMeshAgent agent;

    public virtual void TravelToGoal()
    {
        agent.SetDestination(EndGoal.position);
    }

    public void DecreaseHunger()
    {
        if (AnimalHunger > 0)
        {
            --AnimalHunger;
        }
            
        if(AnimalHunger == 0)
        {
            gameObject.SetActive(false);
        }
        
    }

    private void SetHunger()
    {
        AnimalHunger = AnimalHungerMax;
    }

    private void SetSpeed()
    {
        agent.speed = MovementSpeed;
    }

    public void SetInfoAtStart()
    {
        rig = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        SetHunger();
        SetSpeed();

    }
}
