using UnityEngine;

public class KillPlane : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			other.gameObject.GetComponent<PlayerResource>().DamageHealth(15f);
			DungeonManager.Instance.TpPlayer(DungeonManager.Instance.CurrentCheckPointPos);
		}
		else
		{
			Destroy(other.gameObject);
		}
	}
}
