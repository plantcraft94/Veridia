using UnityEngine;

public class KillPlane : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			var phealth = other.gameObject.GetComponent<PlayerResource>();
			phealth.DamageHealth(15f);
			if(phealth.Health > 0)
			{
				DungeonManager.Instance.TpPlayer(DungeonManager.Instance.CurrentCheckPointPos);
			}
			else
			{
				DungeonManager.Instance.TpPlayer(DungeonManager.Instance.FirstCheckPoint.position);
				phealth.AddHealth(phealth.MaxHealth*2);
			}
		}
		else
		{
			Destroy(other.gameObject);
		}
	}
}
