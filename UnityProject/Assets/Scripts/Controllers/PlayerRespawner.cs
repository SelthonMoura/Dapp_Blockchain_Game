using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerRespawner : NetworkBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private List<Transform> spawnPoints;

    public static PlayerRespawner Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RespawnPlayer(ulong clientId)
    {
        if (!IsServer) return;

        StartCoroutine(RespawnDelay(clientId));
    }

    private IEnumerator RespawnDelay(ulong clientId)
    {
        yield return new WaitForSeconds(1.3f); // wait before respawning

        Vector3 spawnPos = GetRandomSpawnPoint();
        GameObject playerInstance = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
        playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
    }

    private Vector3 GetRandomSpawnPoint()
    {
        if (spawnPoints.Count == 0)
            return Vector3.zero;

        int index = Random.Range(0, spawnPoints.Count);
        return spawnPoints[index].position;
    }
}
