using Fusion;
using UnityEngine;

// player health management class
// relays health decrease to other players

public class PlayerHealth : NetworkBehaviour
{
    [Networked] public int Health { get; private set; } = 100;

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            Health = 100;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_SendDamage(int damage)
    {
        RPC_RelayDamage(damage);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    public void RPC_RelayDamage(int damage)
    {
        if (!HasStateAuthority) return;

        Health -= damage;

        if (Health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
    }
}
