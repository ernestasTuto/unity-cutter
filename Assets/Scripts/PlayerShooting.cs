using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private List<Weapon> weapons = new List<Weapon>();

    private int currentWeaponId = 0;   
    private Projectile localProjectile;

    private void Start()
    {
        weapons[currentWeaponId].gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }

        if (Input.GetMouseButton(0) && weapons[currentWeaponId].CanShoot())
        {
            var _dir = transform.forward;

            // server instantly spawns bullet for all clients
            // client sawns local instance of bullet and then asks server to spawn on all other clients
            if (IsServer)
            {
                OnFireWeapon(_dir, NetworkManager.LocalClientId);
            }
            else
            {
                localProjectile = SpawnLocalBullet(_dir);
                OnFireWeaponServerRpc(_dir, NetworkManager.LocalClientId);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            int _oldId = currentWeaponId;
            currentWeaponId++;
            if (currentWeaponId >= weapons.Count)
            {
                currentWeaponId = 0;
            }

            OnChangeWeaponServerRpc(_oldId, currentWeaponId);
        }
    }

    #region WeaponChange

    [ServerRpc(RequireOwnership = false)]
    private void OnChangeWeaponServerRpc(int _disableId, int _enableId)
    {
        OnChangeWeaponClientRpc(_disableId, _enableId);
    }

    [ClientRpc]
    private void OnChangeWeaponClientRpc(int _disableId, int _enableId)
    {
        currentWeaponId = _enableId;
        weapons[_disableId].gameObject.SetActive(false);    
        weapons[_enableId].gameObject.SetActive(true);
    }

    #endregion

    #region BulletSpawning

    // ServerRpc to ask the server to spawn the bullet for all clients
    [ServerRpc(RequireOwnership = false)]
    private void OnFireWeaponServerRpc(Vector3 _dir, ulong _senderId)
    {
        OnFireWeapon(_dir, _senderId);
    }

    private void OnFireWeapon(Vector3 _dir, ulong _senderId)
    {
        Projectile projectile = weapons[currentWeaponId].ShootWeapon(_dir);
        NetworkObject netObj = projectile.GetComponent<NetworkObject>();
        netObj.Spawn();

        HideBulletForOwnerClientRpc(netObj.NetworkObjectId, _senderId);
    }

    [ClientRpc]
    private void HideBulletForOwnerClientRpc(ulong _projectileId, ulong _shooterClientId)
    {
        if(IsServer)
        {
            return;
        }

        if (NetworkManager.LocalClientId == _shooterClientId)
        {
            NetworkObject netObj = NetworkManager.SpawnManager.SpawnedObjects[_projectileId];
            netObj.GetComponent<Projectile>().SetLocalBullet(localProjectile.gameObject);
            localProjectile = null;
            if (netObj != null)
            {
                netObj.gameObject.SetActive(false); 
            }
        }
    }

    private Projectile SpawnLocalBullet(Vector3 _dir)
    {
        Projectile projectile = weapons[currentWeaponId].ShootWeapon(_dir);
        return projectile;
    }

    #endregion
}
