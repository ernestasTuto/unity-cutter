using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LMB spawns bullet with no rigidbody, spawn on server and synchronizes using ServerManager.Spawn
// RMB spawns bullet with rigidbody, spawns locally then on other clients

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float speed = 3f;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
            GetComponent<PlayerShooting>().enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            SpawnBulletLocal(transform.position + transform.forward, transform.forward);
            SpawnBullet(transform.position + transform.forward, transform.forward, TimeManager.Tick);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SpawnBall(transform.position + transform.forward, transform.forward, TimeManager.Tick);
        }
    }

    [ServerRpc]
    private void SpawnBullet(Vector3 startPosition, Vector3 direction, uint startTick)
    {
        SpawnBulletObserver(startPosition, direction, startTick);
    }

    [ObserversRpc(ExcludeOwner = true)]
    private void SpawnBulletObserver(Vector3 startPosition, Vector3 direction, uint startTick)
    {
        float timeDifference = (float)(TimeManager.Tick - startTick) / TimeManager.TickRate;
        Vector3 spawnPosition = startPosition + direction * speed * timeDifference;

        Bullet spawned = Instantiate(bulletPrefab, spawnPosition, Quaternion.identity);
        spawned.SetDirection(direction);
    }

    private void SpawnBulletLocal(Vector3 startPosition, Vector3 direction)
    {
        Bullet spawned = Instantiate(bulletPrefab, startPosition, Quaternion.identity);
        spawned.SetDirection(direction);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnBall(Vector3 startPosition, Vector3 direction, uint startTick)
    {
        GameObject obj = Instantiate(ballPrefab, startPosition, Quaternion.identity);
        ServerManager.Spawn(obj);

        Rigidbody ballRigidbody = obj.GetComponent<Rigidbody>();
        Vector3 force = direction.normalized * 5f;
        ballRigidbody.AddForce(force, ForceMode.Impulse);
    }
}
