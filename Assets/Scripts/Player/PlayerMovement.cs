using System.Collections.Generic;
using System.Linq;
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

	[Header("Movement")]
	public float Speed;
	float CurrSpeed;

	List<float> SpeedMultipliers = new List<float>();
	Vector3 targetVelocity;

	float MinSpeedMul;

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
		bool IsGrounded = Physics.CheckBox(GroundCheckObject.position, new Vector3(0.9f, 0.3f, 0.9f), Quaternion.identity, GroundLayer);
		Input();
		Movement.Normalize();
		Debug.Log(SpeedMultipliers.Count());
		
		if(SpeedMultipliers.Count() > 0)
		{
			MinSpeedMul = SpeedMultipliers.Min();
		}
		else
		{
			MinSpeedMul = 1f;
		}
		
		CurrSpeed = Speed * MinSpeedMul;

		targetVelocity = Movement * CurrSpeed;
		Vector3 velocity = rb.linearVelocity;
		velocity.x = targetVelocity.x;
		velocity.z = targetVelocity.z;
		rb.linearVelocity = velocity;

		if (JumpAction.WasPressedThisFrame() && IsGrounded)
		{
			rb.linearVelocity = new Vector3(rb.linearVelocity.x, JumpForce, rb.linearVelocity.z);
		}
	}
	
	public void SpeedMul(float SpeedModifier)
	{
		SpeedMultipliers.Add(SpeedModifier);
	}
	public void RemoveSpeedMul(float SpeedModifier)
	{
		SpeedMultipliers.Remove(SpeedModifier);
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
