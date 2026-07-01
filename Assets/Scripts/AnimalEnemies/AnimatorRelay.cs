using UnityEngine;

public class AnimatorRelay : MonoBehaviour
{
    [Header("External")]
    [SerializeField] private Animal animal;

    [SerializeField] private string PileBiteAnimName;
    [SerializeField] private string PlayerBiteAnimName;
    [SerializeField] private string MovementAnimName;

    private int PileBiteHash;
    private int PlayerBiteHash;
    private int MovementHash;

    private Animator animator;
    private void Start()
    {
        PlayerBiteHash = Animator.StringToHash(PlayerBiteAnimName);
        PileBiteHash = Animator.StringToHash(PileBiteAnimName);
        MovementHash = Animator.StringToHash(MovementAnimName);

        if (animator == null)
            animator = GetComponent<Animator>();

        if (animal != null)
        {
            animal.OnPileBite += Animal_OnPileBite;
            animal.OnPlayerBite += Animal_OnPlayerBite;
            animal.OnScurry += Animal_OnScurry;
        }
    }

    private void Animal_OnScurry()
    {
        if (animator != null)
            animator.CrossFade(MovementHash, 0.3f);
    }

    private void Animal_OnPlayerBite()
    {
        if (animator != null)
            animator.CrossFade(PlayerBiteHash, 0.3f);
    }

    private void Animal_OnPileBite()
    {
        if (animator != null)
            animator.CrossFade(PileBiteHash, 0.3f);
    }

    private void OnDisable()
    {
        if(animal != null)
        {
            animal.OnPileBite -= Animal_OnPileBite;
            animal.OnPlayerBite -= Animal_OnPlayerBite;
            animal.OnScurry -= Animal_OnScurry;
        }
    }
}
