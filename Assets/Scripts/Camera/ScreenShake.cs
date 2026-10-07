using Unity.Cinemachine;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance { get; private set; }

    private CinemachineImpulseSource _impulseSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("There's more than one ScreenShake! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float intensity = 1f)
    {
        _impulseSource.GenerateImpulse(intensity);
    }
}
