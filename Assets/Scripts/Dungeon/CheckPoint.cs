using UnityEngine;

public class CheckPoint : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			DungeonManager.Instance.IsOnCheckPoint = true;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Player"))
		{	
			DungeonManager.Instance.CurrentCheckPointPos = other.transform.position;
			DungeonManager.Instance.IsOnCheckPoint = false;
		}
	}
}
