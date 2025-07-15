using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	Rigidbody rb;
	Vector3 Movement;

	public float JumpForce;

	public float speed;
	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		var h = Input.GetAxisRaw("Horizontal");
		var v = Input.GetAxisRaw("Vertical");
		
		Movement = (transform.right * h + transform.forward * v).normalized;

		Vector3 targetVelocity = Movement * speed;
		
		Vector3 velocity = rb.linearVelocity;
		velocity.x = targetVelocity.x;
		velocity.z = targetVelocity.z *1.414214f;
		rb.linearVelocity = velocity;
		
		if (h == 0 && v == 0)
		{
			rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
		}
		
		if(Input.GetKeyDown(KeyCode.Z))
		{
			
			rb.linearVelocity = new Vector3(rb.linearVelocity.x, JumpForce * 1.414214f, rb.linearVelocity.z);
		}
	}
}
