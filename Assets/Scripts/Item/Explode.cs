using UnityEngine;

public class Explode : MonoBehaviour
{
	private void Start()
	{
		GameManager.Instance.ShakeCamera();
		
		Destroy(gameObject, 2f);
	}
	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Enemy"))
		{
			other.gameObject.GetComponent<AiHealth>().TakeDamage(25f);
		}
		if(other.gameObject.CompareTag("Player"))
		{
			other.gameObject.GetComponent<PlayerResource>().DamageHealth(25f);
		}
	}
}
