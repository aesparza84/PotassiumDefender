using UnityEngine;

public class Mice : Animal
{
    void Start()
    {        
        AnimalHungerMax = AnimalHungerMax == 0 ? 1 : AnimalHungerMax;

        SetDefaults();
    }

    public override void FillHunger()
    {
        base.FillHunger();
        RaiseFilledEvent();
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
