using UnityEngine;

public class Pig : Animal
{
    
    void Start()
    {       
        AnimalHungerMax = AnimalHungerMax == 0 ? 3 : AnimalHungerMax;

        SetDefaults();
    }

    protected override void Update()
    {
        base.Update();

        if (scurryPos != Vector3.zero)
        {
            Debug.DrawLine(transform.position, scurryPos, Color.red);
        }
    }
}
