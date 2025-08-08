using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using Physics = Nomnom.RaycastVisualization.VisualPhysics;
#else
using Physics = UnityEngine.Physics;
#endif

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
	public bool isJumping;

	float MinSpeedMul;

	[Header("Input")]
	public InputAction MoveAction;
	public InputAction JumpAction;

	[Header("Animaition Stuff")]
	public Vector2 PlayerFacingDirection;
	[Header("ItemInteraction")]
	ItemsController IC;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		MoveAction = InputSystem.actions.FindAction("Move");
		JumpAction = InputSystem.actions.FindAction("Jump");
		rb = GetComponent<Rigidbody>();
		IC = GetComponent<ItemsController>();
		PlayerFacingDirection = Vector2.down;
	}

	// Update is called once per frame
	void Update()
	{
		if(UiController.Instance.isInInv)
		{
			return;
		}
		if(IC.HookShoted)
		{
			rb.linearVelocity = Vector2.zero;
			return;
		}
		bool IsGrounded = Physics.CheckBox(GroundCheckObject.position, new Vector3(0.5f, 0.1f, 0.5f), Quaternion.identity, GroundLayer);
		isJumping = !IsGrounded;
		Input();
		Movement.Normalize();

		if (SpeedMultipliers.Count() > 0)
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
		if (movement.sqrMagnitude < 0.01f) // Small threshold to avoid floating point issues
		{
			Movement = Vector3.zero;
		}
		else
		{
			if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
			{
				PlayerFacingDirection = new Vector2(Mathf.Sign(movement.x), 0);
			}
			else if (Mathf.Abs(movement.x) < Mathf.Abs(movement.y))
			{
				PlayerFacingDirection = new Vector2(0, Mathf.Sign(movement.y));
			}
			if ((PlayerFacingDirection.x * movement.x < 0) || (PlayerFacingDirection.y * movement.y < 0))
			{
				PlayerFacingDirection = new Vector2(0, Mathf.Sign(movement.y));
			}
			Movement = new Vector3(movement.x, 0, movement.y);
		}
	}
	public void StartHook(Vector3 target,GameObject Hook)
	{
		StartCoroutine(HookPlayer(target,Hook));
	}

	// Coroutine: Move player to target using Rigidbody.MovePosition
	private IEnumerator HookPlayer(Vector3 target, GameObject Hook)
	{
		rb.useGravity = false;
		// Continue until very close to target
		while (Vector3.Distance(rb.position, target) > 0.01f)
		{
			// Move towards target at constant speed
			Vector3 newPosition = Vector3.MoveTowards(
				rb.position,
				target,
				IC.HookSpeed * Time.deltaTime
			);

			rb.MovePosition(newPosition);

			yield return null; // Wait for next frame
		}

		// Ensure final position is exact
		rb.MovePosition(target);
		IC.HookShoted = false;
		rb.useGravity = true;
		Destroy(Hook);
	}

}
