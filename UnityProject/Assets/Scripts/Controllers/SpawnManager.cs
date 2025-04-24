using Unity.BossRoom.Infrastructure;
using Unity.Netcode;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject _enemyPrefab;

    private GameObject[] _players;

    private void Awake()
    {
        //NetworkManager.Singleton.OnServerStarted += StartSpawnEnemies;
    }

    void Start()
    {
        StartSpawnEnemies();
    }

    private void OnDestroy()
    {
        //NetworkManager.Singleton.OnServerStarted -= StartSpawnEnemies;
    }


    private void SpawnEnemies()
    {
        if (!NetworkManager.Singleton.IsServer) return;
        _players = GameObject.FindGameObjectsWithTag("Player");
        int index = Random.Range(0, spawnPoints.Length);
        NetworkObject enemy = NetworkObjectPool.Singleton.GetNetworkObject(_enemyPrefab, spawnPoints[index].position, Quaternion.identity);
        enemy.Spawn();
    }

    private void StartSpawnEnemies()
    {
        InvokeRepeating("SpawnEnemies", 0.5f, 1f);
    }
}
