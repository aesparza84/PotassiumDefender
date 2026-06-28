using System.Threading.Tasks;
using UnityEngine;

public class Bat : Animal
{
    /// <summary>
    /// Initial height to return to
    /// </summary>
    private float startHeight;
    void Start()
    {        
        AnimalHungerMax = AnimalHungerMax == 0 ? 2 : AnimalHungerMax;

        SetInfoAtStart(); //Initialization

        rig.useGravity = false;
        startHeight = transform.position.y;
    }

    protected override void TravelToGoal()
    {
        //transform.LookAt(new Vector3(FoodSupplyTransform.position.x, transform.position.y, FoodSupplyTransform.position.z), Vector3.up);
        //rig.position = Vector3.MoveTowards(transform.position, FoodSupplyTransform.position, ApproachMovementSpeed * Time.fixedDeltaTime);

        transform.LookAt(new Vector3(goalPos.x, transform.position.y, goalPos.z), Vector3.up);
        rig.position = Vector3.MoveTowards(transform.position, goalPos, ApproachMovementSpeed * Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
        if (currState == AnimalState.APPROACHING || currState == AnimalState.SCURRYING)
        {
            TravelToGoal();
        }
    }

    protected override Vector3 CalculateScurryPos()
    {
        Vector3 pos = base.CalculateScurryPos();
        pos.y = startHeight;
        return pos;
    }

    protected override void SwitchState(AnimalState state)
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

                rig.useGravity = false;

                SetTargetViaPosition(pos);
                
                TravelToGoal();
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
    protected override void Update()
    {
        base.Update();

        if (scurryPos != Vector3.zero)
        {
            Debug.DrawLine(transform.position, scurryPos, Color.red);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Finish") && currState != AnimalState.SCURRYING)
        {
            rig.useGravity = true;
            rig.AddForce(FoodSupplyTransform.position); //Ensuring the object doesn't get stuck 
        }
    }
}
