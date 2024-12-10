using System.Globalization;
using Unity.Netcode;
using UnityEngine;


// bullet that destroys itself after 5 seconds or after a hit with another player
public abstract class Projectile : NetworkBehaviour
{
    [SerializeField] protected float projectileSpeed = 10;

    private GameObject localBullet = null;

    public abstract void Init(Vector3 _direction);

    private void Start()
    {
        if(IsOwner)
        {
            Invoke("NotifyServerOfHitServerRpc", 5f);
        }
    }

    private void OnTriggerEnter(Collider _other)
    {
        if (IsOwner)
        {
            if (_other.CompareTag("Player"))
            {
                NotifyServerOfHitServerRpc();
            }
            else
            {
                Debug.Log("TAG DONT MATCH");
            }
        }
    }

    [ServerRpc]
    private void NotifyServerOfHitServerRpc()
    {
        DestroyProjectileClientRpc();
    }

    [ClientRpc]
    private void DestroyProjectileClientRpc()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if(localBullet != null)
        {
            Destroy(localBullet);
        }
    }

    public void SetLocalBullet(GameObject _localBullet)
    {
        localBullet = _localBullet;
    }
}