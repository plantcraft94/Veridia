using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	Rigidbody rb;
	Vector3 Movement;
	[SerializeField] Transform GroundCheckObject;
	[SerializeField] LayerMask GroundLayer;

	public Vector2 movement;

	public float JumpForce;

	public float speed;
	
	[Header("Input")]
	public InputAction MoveAction;
	public InputAction JumpAction;
	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		MoveAction = InputSystem.actions.FindAction("Move");
		JumpAction = InputSystem.actions.FindAction("Jump");
		rb = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{
		bool IsGrounded = Physics.CheckBox(GroundCheckObject.position, new Vector3(1, 0.3f, 1), Quaternion.identity, GroundLayer);
		Input();
		Movement.Normalize();

		Vector3 targetVelocity = Movement * speed;
		
		Vector3 velocity = rb.linearVelocity;
		velocity.x = targetVelocity.x;
		velocity.z = targetVelocity.z *1.414214f;
		rb.linearVelocity = velocity;
		
		// if (Movement ==)
		// {
		// 	rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
		// }
		
		if(JumpAction.WasPressedThisFrame() && IsGrounded)
		{
			rb.linearVelocity = new Vector3(rb.linearVelocity.x, JumpForce * 1.414214f, rb.linearVelocity.z);
		}
	}
	
	void Input()
	{
		movement = MoveAction.ReadValue<Vector2>();
		Movement = new Vector3(movement.x,0,movement.y);
	}
}
