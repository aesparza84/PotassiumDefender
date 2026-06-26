using UnityEngine;

public interface IProjectile
{
    void FillEnemy(Animal animal);
    void AlignAndShoot(Vector3 targetPos);
    void SetFillAmount(int fillAmount);
    void DisableProjectile();
    void SetSpeed(float weaponSpeed);
    void SetMaxDistance(float masDist);
}
