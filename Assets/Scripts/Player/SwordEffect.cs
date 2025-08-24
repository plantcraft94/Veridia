using UnityEngine;

public class SwordEffect : MonoBehaviour
{
	PlayerAttack PA;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		PA = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
	}

	public void FinishSlash()
	{
		gameObject.SetActive(false);
		PA.isSlashing = false;
	}
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
		{
			other.GetComponent<AiHealth>().TakeDamage(PA.SwordDamage, gameObject);
		}
    }
}
