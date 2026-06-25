using UnityEngine;

public interface IProjectile
{
    void FillEnemy(Animal animal);
    void AlignAndShoot(Transform weaponPoint);
    void SetFillAmount(int fillAmount);
    void DisableProjectile();
    void SetSpeed(float weaponSpeed);
}
