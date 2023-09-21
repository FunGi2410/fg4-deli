using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Food : NetworkBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (!col.CompareTag("Player")) return;

        if (!NetworkManager.Singleton.IsServer) return;

        if(col.TryGetComponent(out PlayerLength playerLength))
        {
            playerLength.AddLength();
        }

        NetworkObject.Despawn();
    }
}
