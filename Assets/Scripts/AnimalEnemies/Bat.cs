using System.Threading.Tasks;
using UnityEngine;

public class Bat : Animal
{
    
    void Start()
    {        
        AnimalHungerMax = AnimalHungerMax == 0 ? 2 : AnimalHungerMax;

        SetInfoAtStart();

        rig.useGravity = false;
    }

    private void FixedUpdate()
    {
        this.TravelToGoal();
    }

    public override void TravelToGoal()
    {           
        transform.LookAt(new Vector3(EndGoal.position.x, transform.position.y, EndGoal.position.z), Vector3.up);
        rig.position = Vector3.MoveTowards(transform.position, EndGoal.position, MovementSpeed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Finish"))
        {
            rig.useGravity = true;
            rig.AddForce(EndGoal.position); //Ensuring the object doesn't get stuck 
        }
    }
}
