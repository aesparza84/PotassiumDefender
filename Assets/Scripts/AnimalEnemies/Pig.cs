using UnityEngine;

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
