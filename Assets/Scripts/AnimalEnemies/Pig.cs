using UnityEngine;

public class Pig : Animal
{
    
    void Start()
    {       
        AnimalHungerMax = AnimalHungerMax == 0 ? 3 : AnimalHungerMax;

        this.animalType = AnimalType.PIG;
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
