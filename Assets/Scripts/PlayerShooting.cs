using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// LMB bullet with no rigidbody
// RMB bullet with rigidbody spawned on client side
// MMB bullet with rigidbody spawned on server side using Spawn

public class PlayerShooting : NetworkBehaviour
{
    public float weaponSpeed = 30.0f;
    public float weaponForce = 2f;
    public float weaponLife = 3.0f;
    public float weaponCooldown = 1.0f;

    public GameObject weaponBullet;
    public ProjectileWithTransformPosition weaponBulletNoRb;
    public GameObject weaponBulletPredicted;
    public Transform weaponFirePosition;

    private float weaponCooldownTime;

    private void Update()
    {
        if(!isLocalPlayer)
        {
            return;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (Time.time > weaponCooldownTime)
            {
                weaponCooldownTime = Time.time + weaponCooldown;
                CmdShootRay();
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (Time.time > weaponCooldownTime)
            {
                weaponCooldownTime = Time.time + weaponCooldown;
                CmdShootBulletRb();
            }
        }

        if (Input.GetButtonDown("Fire3"))
        {
            if (Time.time > weaponCooldownTime)
            {
                weaponCooldownTime = Time.time + weaponCooldown;
                FireWeaponLocal();
            }
        }
    }

    [Command]
    void CmdShootRay()
    {
        RpcFireWeapon();
    }

    [ClientRpc]
    void RpcFireWeapon()
    {
        var _dir = transform.forward;
        ProjectileWithTransformPosition bullet = Instantiate(weaponBulletNoRb, weaponFirePosition.position, weaponFirePosition.rotation);
        bullet.Init(_dir);

        Destroy(bullet, weaponLife);
    }

    [Command]
    void CmdShootBulletRb()
    {
        RpcShootBulletRb();
    }

    [ClientRpc]
    void RpcShootBulletRb()
    {
        GameObject bullet = Instantiate(weaponBullet, weaponFirePosition.position, weaponFirePosition.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * weaponSpeed;

        Destroy(bullet, weaponLife);
    }

    [Command]
    void CmdFireWeapon()
    {
        GameObject bullet = Instantiate(weaponBulletPredicted, weaponFirePosition.position, weaponFirePosition.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(bullet.transform.forward * weaponForce, ForceMode.Impulse);

        NetworkServer.Spawn(bullet);

        Destroy(bullet, weaponLife);
    }

    void FireWeaponLocal()
    {
        CmdFireWeapon();
    }
}
