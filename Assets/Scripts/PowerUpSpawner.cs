using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPositions;
    [SerializeField] private Transform _currentPowerUp;
    [SerializeField] private Transform[] _powerUps;

    private void OnEnable()
    {        
        PickupObject.OnPickupObjectTaken += SpawnPickup;
    }

    /// <summary>
    /// Spawn a pickup
    /// </summary>
    /// <param name="takenObject">Pickup type that was taken</param>
    private void SpawnPickup(Transform takenObject)
    {
        takenObject.position = GetSpawnLocation(takenObject.position);
    }

    /// <summary>
    /// Get powerup spawn location
    /// </summary>
    /// <param name="currentPowerupPosition">Current powerup position</param>
    /// <returns></returns>
    private Vector3 GetSpawnLocation(Vector3 currentPowerupPosition)
    {
        Vector3 newSpawnPosition = currentPowerupPosition;

        for (int i = 0; i < _powerUps.Length; i++)
            if (_powerUps[i].position == newSpawnPosition)
                newSpawnPosition = _spawnPositions[Random.Range(0, _spawnPositions.Length - 1)].position;
         
        return newSpawnPosition;
    }

    private void OnDisable()
    {
        PickupObject.OnPickupObjectTaken -= SpawnPickup;
    }
}
