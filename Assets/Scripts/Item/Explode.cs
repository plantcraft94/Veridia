using UnityEngine;

public class Explode : MonoBehaviour
{
	float t;
	private void Start()
	{
		GameManager.Instance.ShakeCamera();
		
		Destroy(gameObject, 1f);
	}
	private void Update()
	{
		t += Time.deltaTime;
	}
	private void OnTriggerEnter(Collider other)
	{
		if(t<=0.25f)
		{
			if(other.gameObject.CompareTag("Enemy"))
			{
				other.gameObject.GetComponent<AiHealth>().TakeDamage(25f, gameObject);
			}
			if(other.gameObject.CompareTag("Player"))
			{
				other.gameObject.GetComponent<PlayerResource>().DamageHealth(25f);
			}
		}
	}
}
