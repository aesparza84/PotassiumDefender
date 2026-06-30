using UnityEngine;
using UnityEngine.AI;

public abstract class Animal : MonoBehaviour
{
    [SerializeField] private string animalName;
    public string AnimalName { get { return animalName; } set { animalName = value; } }

    protected AnimalType animalType;

    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/
    
    [SerializeField] private int animalPointAmount;
    public int AnimalPointAmount { get { return animalPointAmount; } set { animalPointAmount = value; } }
    
    [SerializeField] private int animalHungerMax;
    public int AnimalHungerMax { get { return animalHungerMax; } set { animalHungerMax = value; } }
    
    public int AnimalHunger { get; /*{ return animalHunger; }*/ set; /*{ animalHunger = value; }*/ }

    [SerializeField] protected float feedDelay;
    protected float currFeedDelay;

    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

    [SerializeField] protected float approachMovementSpeed;
    public float ApproachMovementSpeed { get { return approachMovementSpeed; } set { approachMovementSpeed = value; } }

    [SerializeField] protected float scurryMovementSpeed;
    public float ScurryMovementSpeed { get { return scurryMovementSpeed; } set { scurryMovementSpeed = value; } }


    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

    [SerializeField] private Transform foodSupplyTransform;
    public Transform FoodSupplyTransform { get { return foodSupplyTransform; } set { foodSupplyTransform = value; } }
    protected Vector3 goalPos;
    protected Vector3 scurryPos; //Debug Vector
    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

    public Rigidbody rig { get; set; }
    public NavMeshAgent agent;
    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

    /// <summary>
    /// Event when animal fills hunger
    /// </summary>
    public event System.Action<Animal> OnFilled;

    public static event System.Action<AnimalType> OnFilledFromPlayer;

    /// <summary>
    /// Event when this gameobject will be disabled
    /// </summary>
    public event System.Action<GameObject, Animal> OnAnimalDisable;


    /*~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*/

    //External
    protected FoodSupply foodSupplyReference; //This will get referenced on collision. De referenced when Scurry

    protected AnimalState currState;
    protected virtual void TravelToGoal()
    {
        //Provide a Vector3
        agent.SetDestination(goalPos);
    }
    protected void SetAgentSpeed(float speed)
    {
        agent.speed = speed;
    }
    protected void SetTargetViaTransform(Transform targetTransform)
    {
        this.FoodSupplyTransform = targetTransform;

        if (targetTransform == null)
        {
            this.goalPos = transform.position;
        }
        else
        {
            this.goalPos = this.FoodSupplyTransform.position;
        }
    }
    protected void SetTargetViaPosition(Vector3 pos)
    {
        this.goalPos = pos;
    }

    public virtual void FillHunger(bool fromPlayer)
    {
        if (currState == AnimalState.SCURRYING)
            return;

        Debug.Log($"{gameObject.name} - Bite");
        AnimalHunger++;

        if (AnimalHunger >= animalHungerMax)
        {
            SwitchState(AnimalState.SCURRYING);

            if (fromPlayer)
                OnFilledFromPlayer?.Invoke(this.animalType); //Only fires when player has last hit animal
        }
    }
    protected virtual void SwitchState(AnimalState state)
    {
        this.currState = state;

        switch (currState)
        {
            case AnimalState.APPROACHING:
                SetAgentSpeed(approachMovementSpeed);
                SetTargetViaTransform(FoodSupplyTransform);
                TravelToGoal();

                break;
            case AnimalState.EATING:
                SetTargetViaTransform(null); //Full stop at position
                SetTargetViaPosition(transform.position);
                SetAgentSpeed(0);
                                
                break;
            case AnimalState.SCURRYING:
                //somehwere behind animal
                SetAgentSpeed(ScurryMovementSpeed);
                Vector3 pos = CalculateScurryPos();
                scurryPos = pos;
                SetTargetViaPosition(pos);
                TravelToGoal();

                //Disable when scurry
                Invoke("DisableAnimal", 3.0f);

                break;
            case AnimalState.IDLE:
                //Stay still, do nothing
                SetAgentSpeed(0);
                SetTargetViaPosition(transform.position); //Full stop at position

                break;
            default:
                break;
        }
    }
    protected virtual Vector3 CalculateScurryPos()
    {
        Vector3 dir = -transform.forward;
        Quaternion rot = Quaternion.AngleAxis(Random.Range(-45,45),transform.up);
        Vector3 f = rot * dir;
        return transform.position + f.normalized * 20;
    }

    protected void DisableAnimal()
    {
        SwitchState(AnimalState.IDLE);

        //Fire event for POOLING
        OnAnimalDisable?.Invoke(gameObject, this);
        
        gameObject.SetActive(false);
    }

    //Initialization
    protected void SetHunger()
    {
        AnimalHunger = AnimalHungerMax;
    }

    public void SetSupplyTransform(Transform transform)
    {
        this.FoodSupplyTransform = transform;
    }
    public void SetDefaults()
    {
        if (rig == null)
            rig = GetComponent<Rigidbody>();
        
        if(agent == null)
            agent = GetComponent<NavMeshAgent>();

        AnimalHunger = 0;
        currFeedDelay = 0;
        SwitchState(AnimalState.APPROACHING);
    }

    protected virtual void RaiseFilledEvent()
    {
        OnFilled?.Invoke(this);
    }

    protected virtual void Update()
    {
        switch (currState)
        {
            case AnimalState.APPROACHING:
     
                break;
            case AnimalState.EATING:
                if (currFeedDelay > 0.0f)
                {
                    currFeedDelay -= Time.deltaTime;
                }
                else
                {
                    currFeedDelay = feedDelay;
                    FillHunger(false);
                    if (foodSupplyReference != null)
                    {
                        foodSupplyReference.ReduceAmount();
                    }
                }


                break;
            case AnimalState.SCURRYING:
                break;
            case AnimalState.IDLE:
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            if (currState == AnimalState.SCURRYING)
                return;

            if (collision.gameObject.TryGetComponent<FoodSupply>(out FoodSupply f))
            {
                this.foodSupplyReference = f;
                SwitchState(AnimalState.EATING);
            }
        }
    }
}
