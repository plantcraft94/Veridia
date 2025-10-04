using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    [SerializeField] GameObject SpawnObject;

    public GameObject CurrentSpawn;

    public void Spawn()
    {
        if (CurrentSpawn != null)
        {
            Destroy(CurrentSpawn);
        }

        CurrentSpawn = Instantiate(
            SpawnObject,
            transform.position + Vector3.up * 0.5f,
            Quaternion.identity,
            transform
        );

        BossAttack boss = CurrentSpawn.GetComponent<BossAttack>();
        if (boss != null)
        {
            
            Transform room = transform.parent;
            if (room != null)
            {
                Transform wp1 = room.Find("Waypoint1");
                Transform wp2 = room.Find("Waypoint2");

                if (wp1 != null) boss.waypoint1 = wp1;
                if (wp2 != null) boss.waypoint2 = wp2;
            }
        }
    }
}
