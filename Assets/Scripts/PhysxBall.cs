using UnityEngine;
using Fusion;
using Fusion.Addons.Physics;

// rigidbody bullet script using TickTimer for accurate representation
// upon collision with player reduces health

public class PhysxBall : NetworkBehaviour
{
    [Networked] private TickTimer life { get; set; }
    private Collider _collider;
    private NetworkBool _isDestroyed { get; set; }
    public void Init(Vector3 forward)
    {
        life = TickTimer.CreateFromSeconds(Runner, 2f);
        NetworkRigidbody3D _rigidbody = GetComponent<NetworkRigidbody3D>(); 
        _rigidbody.Rigidbody.isKinematic = false;
        _rigidbody.Rigidbody.AddForce(forward * 1.1f, ForceMode.Impulse);
    }

    public override void FixedUpdateNetwork()
    {
        _collider.enabled = _isDestroyed == false;

        if (life.Expired(Runner) || _isDestroyed)
            Runner.Despawn(Object);
    }

    private void Awake()
    {
        _collider = GetComponentInChildren<Collider>();

        _collider.enabled = false;
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (!HasStateAuthority) return;

        Debug.Log("COLLISION");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit Player");
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.RPC_SendDamage(50);
            }

            ProcessHit();
        }
    }

    private void ProcessHit()
    {
        _isDestroyed = true;
    }
}