using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerLength : NetworkBehaviour
{
    public NetworkVariable<ushort> length = new(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] private GameObject bodyTailPrefab;

    private List<GameObject> tails;

    private Transform lastTail;
    //private Collider col;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        this.tails = new List<GameObject>();
        //this.col = GetComponent<Collider>();
        this.lastTail = transform;

        if(!IsServer) this.length.OnValueChanged += this.LengthChanged;
    }


    // Called by Server
    [ContextMenu ("Add Length")]
    public void AddLength()
    {
        this.length.Value += 1;

        this.InstantiateTail();
    }

    private void LengthChanged(ushort previousValue, ushort nextValue)
    {
        this.InstantiateTail();
    }

    private void InstantiateTail()
    {
        GameObject tailObj = Instantiate(this.bodyTailPrefab, transform.position, Quaternion.identity);
        if(tailObj.TryGetComponent(out Tail tail))
        {
            tail.networkedOwner = transform;
            tail.followTransform = this.lastTail;

            this.lastTail = tailObj.transform;

            //Physics.IgnoreCollision(tailObj.GetComponent<Collider>(), this.col);
        }
        
        this.tails.Add(tailObj);
    }
}
