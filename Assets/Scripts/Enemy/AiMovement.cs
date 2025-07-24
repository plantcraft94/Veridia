using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AiMovement : MonoBehaviour
{
    public AiSensor sensor;
    public float updateRate = 0.5f;

    private NavMeshAgent agent;
    private float updateTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        updateTimer = updateRate;
    }

    void Update()
    {
        updateTimer -= Time.deltaTime;
        if (updateTimer <= 0)
        {
            updateTimer = updateRate;

            GameObject target = GetNearestTarget();
            if (target != null)
            {
                agent.SetDestination(target.transform.position);
            }
        }
    }

    GameObject GetNearestTarget()
    {
        if (sensor == null || sensor.Objects.Count == 0)
            return null;

        GameObject nearest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (GameObject obj in sensor.Objects)
        {
            float dist = Vector3.Distance(currentPos, obj.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = obj;
            }
        }

        return nearest;
    }
}
