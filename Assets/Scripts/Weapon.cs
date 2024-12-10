using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Projectile bulletPrefab;
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private float cooldown = 0.5f;

    private float _lastFired = float.MinValue;

    public Vector3 GetSpawnPosition()
    {
        return spawnLocation.position;
    }

    public Projectile ShootWeapon(Vector3 _dir)
    {
        _lastFired = Time.time;
        Projectile projectile = Instantiate(bulletPrefab, spawnLocation.position, Quaternion.identity);
        projectile.Init(_dir);
        return projectile;
    }

    public bool CanShoot()
    {
        return _lastFired + cooldown < Time.time;
    }
}
