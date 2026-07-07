using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerCameraShake : MonoBehaviour
{
    private CinemachineCamera camera;
    private CinemachineBasicMultiChannelPerlin perlinNoise;
    [SerializeField] private float shakeAmplitude; 
    [SerializeField] private float amplitudeLossSpeed;

    private void Start()
    {
        camera = GetComponent<CinemachineCamera>();

        perlinNoise = (CinemachineBasicMultiChannelPerlin)camera.GetCinemachineComponent(CinemachineCore.Stage.Noise);

        FoodSupply.OnStaticSupplyHit += OnShake;
    }

    private void OnEnable()
    {
        FoodSupply.OnStaticSupplyHit += OnShake;
    }
    private void OnDisable()
    {
        perlinNoise.AmplitudeGain = 0;
        shakeAmplitude = 0;
        FoodSupply.OnStaticSupplyHit -= OnShake;
    }

    private void OnShake()
    {
        shakeAmplitude = 3;
        perlinNoise.AmplitudeGain = shakeAmplitude;
    }

    private void Update()
    {
        if (shakeAmplitude > 0.0f)
        {
            shakeAmplitude -= Time.deltaTime * amplitudeLossSpeed;
            perlinNoise.AmplitudeGain = shakeAmplitude;
        }
        else
        {
            perlinNoise.AmplitudeGain = 0.0f;
            shakeAmplitude = 0.0f;
        }
    }
}
