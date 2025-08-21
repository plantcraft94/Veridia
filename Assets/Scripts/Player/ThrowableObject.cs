using UnityEngine;
#if UNITY_EDITOR
using Physics = Nomnom.RaycastVisualization.VisualPhysics;
#else
using Physics = UnityEngine.Physics;
#endif

public class ThrowableObject : MonoBehaviour
{
    public int Grabbed = 0;
    Transform Player;
    Collider coll;
    Rigidbody rb;
    PlayerMovement PM;
    LayerMask GroundLayer;
    [SerializeField] bool BreakAfterThrow = false;

    public float ThrowForce;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        coll = GetComponent<Collider>();
        PM = Player.GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        GroundLayer = 1 << LayerMask.NameToLayer("Ground"); // Proper layer mask
        if (BreakAfterThrow)
        {
            rb.isKinematic = true; // Initially kinematic (held state)
        }
    }

    void Update()
    {
        if (Grabbed == 1)
        {
            transform.position = new Vector3(Player.position.x, Player.position.y + 1, Player.position.z);
            coll.enabled = false;
        }
        else if (Grabbed == 2)
        {
            rb.isKinematic = false; // Enable physics for throwing
            rb.AddForce(new Vector3(PM.PlayerFacingDirection.x, 0.5f, PM.PlayerFacingDirection.y) * ThrowForce, ForceMode.Impulse);
            Grabbed = 0; // Reset grab state
            coll.enabled = true;
        }
    }

    private void FixedUpdate()
    {
        if (!BreakAfterThrow && Grabbed == 0) // Only relevant after throw
        {
            if (Physics.BoxCast(transform.position, new Vector3(0.4f, 0.1f, 0.4f), Vector3.down, Quaternion.identity, 0.52f, GroundLayer))
            {
                rb.isKinematic = true;
            }
        }
    }

    // NEW: Handle destruction on ANY collision when breaking
    private void OnCollisionEnter(Collision collision)
    {
        if (BreakAfterThrow && Grabbed == 0 && !rb.isKinematic && !collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}