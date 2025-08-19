using UnityEngine;

public class CheckPoint : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		DungeonManager.Instance.CurrentCheckPointPos = transform.position;
	}
}
