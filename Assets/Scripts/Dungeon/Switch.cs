using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
	public UnityEvent OnToggle;
	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log("Switch");
		if(collision.gameObject.CompareTag("PlayerSword") || collision.gameObject.CompareTag("PlayerArrow") || collision.gameObject.CompareTag("EnemyArrow") || collision.gameObject.GetComponent<ThrowableObject>() != null)
		{
			OnToggle.Invoke();
		}
	}
}
