using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    private const int MAXPREFABCOUNT = 3;

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += SpawnFoodStart;
    }

    private void SpawnFoodStart()
    {
        NetworkManager.Singleton.OnServerStarted -= SpawnFoodStart;
        NetworkObjectPool.Singleton.InitializePool();
        for(int i = 0; i < 3; i++)
        {
            SpawnFood();
        }

        StartCoroutine(SpawnOverTime());
    }

    private IEnumerator SpawnOverTime()
    {
        while(NetworkManager.Singleton.ConnectedClients.Count > 0)
        {
            yield return new WaitForSeconds(2f);
            if(NetworkObjectPool.Singleton.GetCurrentPrefabCount(prefab) < MAXPREFABCOUNT)
            {
                SpawnFood();
            }
        }
    }

    private void SpawnFood()
    {
        NetworkObject obj = NetworkObjectPool.Singleton.GetNetworkObject(prefab,
            GetRandomPosOnMap(), Quaternion.identity);
        obj.GetComponent<Food>().prefab = this.prefab;
        if(!obj.IsSpawned) obj.Spawn(true);
    }

    private Vector3 GetRandomPosOnMap()
    {
        return new Vector3(Random.Range(0.3f, 9f), 0f, Random.Range(0.5f, 18f));
    }
}
