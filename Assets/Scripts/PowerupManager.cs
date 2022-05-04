using UnityEngine;

public class PowerupManager : MonoBehaviour
{

    [SerializeField] private Transform[] powerUpLocations;

    [SerializeField] private Transform healthPosition;
    [SerializeField] private Transform ammoPosition;

    private void SetPowerUpPosition(Transform powerUp)
    {
        powerUp.position = powerUpLocations[Random.Range(0, powerUpLocations.Length)].position;
        Debug.Log(powerUp);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetPowerUpPosition(ammoPosition);
            SetPowerUpPosition(healthPosition);
        }
        if (ammoPosition == healthPosition)
            SetPowerUpPosition(healthPosition);
    }
}
