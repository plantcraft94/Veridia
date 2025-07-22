using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	Rigidbody rb;
	Vector3 Movement;
	[SerializeField] Transform GroundCheckObject;
	[SerializeField] LayerMask GroundLayer;

	Vector2 movement;

	public float JumpForce;

	public float speed;

	[Header("Input")]
	public InputAction MoveAction;
	public InputAction JumpAction;

	[Header("Animaition Stuff")]
	public Vector2 AnimInput;

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
		velocity.z = targetVelocity.z;
		rb.linearVelocity = velocity;

		if (JumpAction.WasPressedThisFrame() && IsGrounded)
		{
			rb.linearVelocity = new Vector3(rb.linearVelocity.x, JumpForce, rb.linearVelocity.z);
		}
	}

	void Input()
	{
		movement = MoveAction.ReadValue<Vector2>();
		if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
		{
			AnimInput = new Vector2(movement.x, movement.y * 0.5f);
		}
		else if (Mathf.Abs(movement.x) < Mathf.Abs(movement.y))
		{
			AnimInput = new Vector2(movement.x * 0.5f, movement.y);
		}
		if ((AnimInput.x * movement.x < 0) || (AnimInput.y * movement.y < 0))
		{
			AnimInput = new Vector2(movement.x * 0.5f, movement.y);
		}
		Movement = new Vector3(movement.x, 0, movement.y);
	}
}
