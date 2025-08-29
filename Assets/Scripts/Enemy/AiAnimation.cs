using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class AiAnimation : MonoBehaviour
{
    public Transform parentRoot; // Drag Parent vào đây (Slime root)
    private Animator anim;
    private NavMeshAgent agent;
    private Rigidbody rb;

    // Animator Parameters
    public string paramMoveX = "MoveX";
    public string paramMoveY = "MoveY";
    public string paramSpeed = "Speed";

    void Start()
    {
        anim = GetComponent<Animator>();
        if (parentRoot == null) parentRoot = transform.parent;

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

        // Chỉ quan tâm đến X/Z vì game top-down 2.5D
        Vector3 flatVel = new Vector3(velocity.x, 0f, velocity.z);
        float speed = flatVel.magnitude;
        Vector3 dir = speed > 0.01f ? flatVel.normalized : Vector3.zero;

        // Đẩy dữ liệu sang Animator
        anim.SetFloat(paramMoveX, dir.x);
        anim.SetFloat(paramMoveY, dir.z);
        anim.SetFloat(paramSpeed, speed);
    }
}
