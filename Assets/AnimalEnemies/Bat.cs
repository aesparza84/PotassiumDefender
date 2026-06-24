using UnityEngine;
using UnityEngine.AI;

public class Bat : Animal
{
    private float defaultSpeed;

    void Start()
    {
        
        AnimalHungerMax = AnimalHungerMax == 0 ? 2 : AnimalHungerMax;

        SetInfoAtStart();

        defaultSpeed = MovementSpeed;
        
    }

    private void FixedUpdate()
    {
        this.TravelToGoal();
    }

    public override void TravelToGoal()
    {
        transform.LookAt(new Vector3(EndGoal.position.x, transform.position.y, EndGoal.position.z), Vector3.up);
        rig.position = Vector3.MoveTowards(transform.position, EndGoal.position, defaultSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Finish"))
        {
            defaultSpeed = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            defaultSpeed = MovementSpeed;
        }
    }
}
