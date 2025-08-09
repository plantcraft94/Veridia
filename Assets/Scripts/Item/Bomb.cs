using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	public float ExplodeTimer;
	[SerializeField] GameObject ExplodeEffect;
    private void Start()
    {
		StartCoroutine(Explode());
    }
    IEnumerator Explode()
	{
		yield return new WaitForSeconds(ExplodeTimer);
		Instantiate(ExplodeEffect, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
