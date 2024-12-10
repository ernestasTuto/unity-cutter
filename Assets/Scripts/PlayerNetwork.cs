using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

// if its owner of this class it changes PlayerNetworkData, else receives other users data and interpolates it

public class PlayerNetwork : NetworkBehaviour
{
    private readonly NetworkVariable<PlayerNetworkData> netState = new NetworkVariable<PlayerNetworkData>(writePerm: NetworkVariableWritePermission.Owner);
    private Vector3 vel;
    private float rotVel;
    [SerializeField] private float cheapInterpolationTime = 0.1f;


    private void Update()
    {
        if(IsOwner)
        {
            netState.Value = new PlayerNetworkData()
            {
                Position = transform.position,
                Rotation = transform.rotation.eulerAngles
            };
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, netState.Value.Position, ref vel, cheapInterpolationTime);
            transform.rotation = Quaternion.Euler(
                0,
                Mathf.SmoothDampAngle(transform.rotation.eulerAngles.y, netState.Value.Rotation.y, ref rotVel, cheapInterpolationTime),
                0);
        }
    }

    struct PlayerNetworkData : INetworkSerializable
    {
        private float _x, _z;
        private short _yRot;

        internal Vector3 Position
        {
            get => new Vector3(_x, 0, _z);
            set
            {
                _x = value.x;
                _z = value.z;
            }
        }

        internal Vector3 Rotation
        {
            get => new Vector3(0, _yRot, 0);
            set => _yRot = (short)value.y;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> _serializer) where T : IReaderWriter
        {
            _serializer.SerializeValue(ref _x); 
            _serializer.SerializeValue(ref _z); 
            
            _serializer.SerializeValue(ref _yRot);
        }
    }
}
