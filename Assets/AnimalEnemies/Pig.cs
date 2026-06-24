using UnityEngine;
using UnityEngine.AI;

public class Pig : Animal
{
    void Start()
    {
       
        AnimalHungerMax = AnimalHungerMax == 0 ? 3 : AnimalHungerMax;

        SetInfoAtStart();
    }

    private void FixedUpdate()
    {
        TravelToGoal();
    }
}
