using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class AiAnimation : MonoBehaviour
{
    public Transform parentRoot; 
    private Animator anim;
    private NavMeshAgent agent;
    private Rigidbody rb;

    public string paramMoveX = "MoveX";
    public string paramMoveY = "MoveY";
    public string paramSpeed = "Speed";

    public string paramLastX = "LastMoveX";
    public string paramLastY = "LastMoveY";

    private Vector3 lastDir = Vector3.down;

    private AiSensor sensor;
    BossAttack bossAttack;

    void Start()
    {
        anim = GetComponent<Animator>();
        if (parentRoot == null) parentRoot = transform.parent;
        bossAttack = parentRoot.GetComponent<BossAttack>();
        agent = parentRoot.GetComponent<NavMeshAgent>();
        rb = parentRoot.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 velocity = Vector3.zero;

        if (agent != null && agent.enabled)
        {
            velocity = agent.velocity;
        }
        else if (rb != null)
        {
            velocity = rb.linearVelocity; 
        }

        Vector3 flatVel = new Vector3(velocity.x, 0, velocity.z);
        float speed = flatVel.magnitude;

        Vector3 dir;
        if (speed > 0.01f)
        {
            dir = flatVel.normalized; 
            lastDir = dir;            
        }
        else
        {
            Vector3 lookDir = parentRoot.forward;
            lookDir.y = 0;
            if (lookDir.sqrMagnitude > 0.001f)
                lastDir = lookDir.normalized;

            dir = Vector3.zero;
        }

        // Set cho Animator
        anim.SetFloat(paramMoveX, dir.x);
        anim.SetFloat(paramMoveY, dir.z);
        anim.SetFloat(paramSpeed, speed);
        anim.SetFloat(paramLastX, lastDir.x);
        anim.SetFloat(paramLastY, lastDir.z);
    }
    public void doAttackShock()
    {
        bossAttack.DoShockwave();
    }
}
