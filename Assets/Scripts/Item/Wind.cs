using UnityEngine;

public class Wind : MonoBehaviour
{
	float Speed;
	private void Start()
	{
		Speed = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemsController>().WindSpeed;
	}
	private void OnCollisionEnter(Collision collision)
	{
		PushableObject PO = collision.gameObject.GetComponent<PushableObject>();
		if(PO != null)
		{
			PO.startMove(collision.transform.position + transform.forward, Speed,transform.forward);
		}
		Destroy(gameObject);
	}
}
