using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPositions;
    [SerializeField] private Transform _currentPowerUp;
    [SerializeField] private Transform[] _powerUps;


    private void OnEnable()
    {        
        PickupController.onPickupTaken += SpawnPickup;
        HealthPickupObject.onHealthPickupObjectTaken += SpawnPickup;
    }

    private void SpawnPickup()
    {
        for(int i = 0; i < _powerUps.Length; i++)
        {
            _powerUps[i].position = _spawnPositions[Random.Range(0, _spawnPositions.Length - 1)].position;
            // Временное решение, чтобы каждый следующий приз не появлялся на одной позиции
            // причем он не работает xD
            if(i > 0 && _powerUps[i-1] == _powerUps[i])
                _powerUps[i].position = _spawnPositions[Random.Range(0, _spawnPositions.Length - 1)].position;
        }
    }
    
    private void SpawnAmmoPickup()
    {


    }

    private void SpawnHealthPickup()
    {

    }

    //private void GetSpawnLocation(int type)
    //{
    //    if (type == 1)
    //    {
    //        _currentPowerUp = _powerUps[0];
    //    }
    //    else if (type == 0)
    //        _currentPowerUp = _powerUps[1];

    //    //_currentPowerUp = _powerUps[Random.Range(0, _powerUps.Length - 1)];
        
    //    _currentPowerUp.position = _spawnPositions[Random.Range(0, _spawnPositions.Length - 1)].position;
        
    //}

    private void OnDisable()
    {
        PickupController.onPickupTaken -= SpawnPickup;
        HealthPickupObject.onHealthPickupObjectTaken -= SpawnPickup;
    }
}
