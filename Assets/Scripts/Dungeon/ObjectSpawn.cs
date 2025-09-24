using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
	[SerializeField] GameObject SpawnObject;

	public GameObject CurrentSpawn;
    public void Spawn()
	{
		if(CurrentSpawn != null)
		{
			Destroy(CurrentSpawn);
		}
		CurrentSpawn = Instantiate(SpawnObject, new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z), Quaternion.identity);
	}
}
