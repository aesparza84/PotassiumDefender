using UnityEngine;

public class Mice : Animal
{
    void Start()
    {        
        AnimalHungerMax = AnimalHungerMax == 0 ? 1 : AnimalHungerMax;

        this.animalType = AnimalType.MICE;
        SetDefaults();
    }

    public override void FillHunger(bool fromPlayer)
    {
        base.FillHunger(fromPlayer);
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
