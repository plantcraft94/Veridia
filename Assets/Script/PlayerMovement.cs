using UnityEngine;
using PrimeTween;

public class PlayerMovement : MonoBehaviour
{
	public GameObject Sprite;
	public float Z = 0;
	public float ZSpeed = 0;
	public float ZaxisGravity = 2f;
	private Vector2 moveInput;
	public float Speed;
	public float JumpForce;
	bool IsJumping;
	Rigidbody2D rb;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		
		
		
		
		moveInput.x = Input.GetAxisRaw("Horizontal");
		moveInput.y = Input.GetAxisRaw("Vertical");

		moveInput.Normalize();
		rb.linearVelocity = moveInput * Speed;
		
		if(Input.GetKeyDown(KeyCode.Z) && Z == 0)
		{
			ZSpeed = JumpForce;
			
		}
		Sprite.transform.position = new Vector2(transform.position.x, transform.position.y + Z * 0.01f);
	}
	private void FixedUpdate()
	{
		ZSpeed -= ZaxisGravity;
		Z += ZSpeed;
		if(Z < 0)
		{
			Z = 0;
			ZSpeed = 0;
		}
	}
}
