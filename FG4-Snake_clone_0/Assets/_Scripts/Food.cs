using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Food : NetworkBehaviour
{
    public GameObject prefab;
    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;

        if (!NetworkManager.Singleton.IsServer) return;

        if(col.TryGetComponent(out PlayerLength playerLength))
        {
            playerLength.AddLength();
        }

        NetworkObjectPool.Singleton.ReturnNetworkObject(NetworkObject, prefab);

        NetworkObject.Despawn();
    }
}
