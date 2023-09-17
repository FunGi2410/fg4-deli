using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour
{
    private readonly NetworkVariable<PlayerNetworkData> netState = new(writePerm: NetworkVariableWritePermission.Owner);
    private Vector3 vel;
    private float rotVel;
    [SerializeField] private float cheapInterpolationTime = 0.1f;


    private void Update()
    {
        if (IsOwner)
        {
            this.netState.Value = new PlayerNetworkData()
            {
                Position = transform.position,
                Rotation = transform.rotation.eulerAngles
            };
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, this.netState.Value.Position, ref this.vel, this.cheapInterpolationTime);
            transform.rotation = Quaternion.Euler(
                0,
                Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, this.netState.Value.Rotation.y, ref rotVel, this.cheapInterpolationTime),
                0);
        }
    }

    struct PlayerNetworkData : INetworkSerializable
    {
        private float x, z;
        private float yRot;

        internal Vector3 Position
        {
            get => new Vector3(x, 0, z);
            set
            {
                x = value.x;
                z = value.z;
            }
        }

        internal Vector3 Rotation
        {
            get => new Vector3(0, yRot, 0);
            set => yRot = value.y;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref x);
            serializer.SerializeValue(ref z);

            serializer.SerializeValue(ref yRot);
        }
    }

}
