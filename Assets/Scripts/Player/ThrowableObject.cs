using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public int Grabbed = 0;
	Transform Player;
	Collider coll;
	Rigidbody rb;
	
	PlayerMovement PM;
	
	public float ThrowForce;
	void Start()
	{
		Player = GameObject.FindGameObjectWithTag("Player").transform;
		coll = GetComponent<Collider>();
		PM = Player.GetComponent<PlayerMovement>();
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		if(Grabbed == 1)
		{
			transform.position = new Vector3(Player.position.x, Player.position.y + 1, Player.position.z);
			coll.enabled = false;
		}
		else if(Grabbed == 2)
		{
			rb.isKinematic = false;
			rb.AddForce(new Vector3(PM.AnimInput.x, 0.5f,PM.AnimInput.y) * ThrowForce, ForceMode.Impulse);
			Grabbed = 0;
			coll.enabled = true;
		}
	}
	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.layer == 6)
		{
			rb.isKinematic = true;
		}
	}
}
