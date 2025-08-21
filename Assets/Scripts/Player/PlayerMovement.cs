using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
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
	bool isSlowed = false;

	[Header("Input")]
	public InputAction MoveAction;
	public InputAction JumpAction;
	public InputAction RunAction;

	[Header("Animaition Stuff")]
	public Vector2 PlayerFacingDirection;
	[Header("ItemInteraction")]
	ItemsController IC;
	[Header("Boot")]
	public float RunMultiplyer;
	bool isRunning = false;
	Vector2 runStartDirection;
	bool startRun;
	Coroutine currentco;
	public GameObject Interacter;
	public UnityEvent Crash;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		MoveAction = InputSystem.actions.FindAction("Move");
		JumpAction = InputSystem.actions.FindAction("Jump");
		RunAction = InputSystem.actions.FindAction("WingBoot");

		rb = GetComponent<Rigidbody>();
		IC = GetComponent<ItemsController>();
		PlayerFacingDirection = Vector2.down;
	}

	// Update is called once per frame
	void Update()
	{
		if (GameManager.Instance.isInInv)
		{
			return;
		}
		if (IC.HookShoted)
		{
			rb.linearVelocity = Vector2.zero;
			return;
		}
		bool IsGrounded = Physics.CheckBox(GroundCheckObject.position, new Vector3(0.5f, 0.1f, 0.5f), Quaternion.identity, GroundLayer);
		isJumping = !IsGrounded;
		Input();
		Movement.Normalize();
		if (JumpAction.WasPressedThisFrame() && IsGrounded)
		{
			if(isRunning)
			{
				rb.linearVelocity = new Vector3(rb.linearVelocity.x, 15.5f, rb.linearVelocity.z);
			}
			else
			{		
				rb.linearVelocity = new Vector3(rb.linearVelocity.x, JumpForce, rb.linearVelocity.z);
			}
		}

		if (SpeedMultipliers.Count() > 0)
		{
			MinSpeedMul = SpeedMultipliers.Min();
		}
		else
		{
			MinSpeedMul = 1f;
		}
		isSlowed = SpeedMultipliers.Count() > 0;
		WingBoot();
		if (isSlowed)
		{
			CurrSpeed = Speed * MinSpeedMul;
		}
		if(runStartDirection != PlayerFacingDirection)
		{
			isRunning = false;
		}
		if (isRunning)
		{
			if (movement != Vector2.zero)
			{
				targetVelocity = Movement * Speed * RunMultiplyer;
			}
			else
			{
				targetVelocity = new Vector3(PlayerFacingDirection.x,0,PlayerFacingDirection.y) * Speed * RunMultiplyer;
			}
			Vector3 runvelocity = rb.linearVelocity;
			runvelocity.x = targetVelocity.x;
			runvelocity.z = targetVelocity.z;
			rb.linearVelocity = runvelocity;
			RaycastHit hit;
			if (Physics.Raycast(Interacter.transform.position, Interacter.transform.forward, out hit, 0.75f))
			{
				if(hit.collider.gameObject.CompareTag("RampBlock"))
				{
					hit.collider.gameObject.GetComponent<RampBlock>().Break();
				}
				else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
				{
					isRunning = false;
					GameManager.Instance.ShakeCamera();
					if(Crash != null)
					{
						Crash.Invoke();
					}
				}
			}
		}
		if (!isRunning && !isSlowed)
		{
			CurrSpeed = Speed;
		}

		if (!isRunning)
		{
			targetVelocity = Movement * CurrSpeed;
			Vector3 velocity = rb.linearVelocity;
			velocity.x = targetVelocity.x;
			velocity.z = targetVelocity.z;
			rb.linearVelocity = velocity;
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
			if ((Mathf.Abs(movement.x) > Mathf.Abs(movement.y)) && !IC.aim)
			{
				PlayerFacingDirection = new Vector2(Mathf.Sign(movement.x), 0);
			}
			else if ((Mathf.Abs(movement.x) < Mathf.Abs(movement.y)) && !IC.aim)
			{
				PlayerFacingDirection = new Vector2(0, Mathf.Sign(movement.y));
			}
			if (((PlayerFacingDirection.x * movement.x < 0) || (PlayerFacingDirection.y * movement.y < 0)) && !IC.aim)
			{
				PlayerFacingDirection = new Vector2(0, Mathf.Sign(movement.y));
			}
			Movement = new Vector3(movement.x, 0, movement.y);
		}
	}
	void WingBoot()
	{
		if(GameManager.Instance.HasItem(Item.WingBoot))
		{
			if (RunAction.WasPressedThisFrame())
			{
				startRun = true;
			}
			if (RunAction.WasReleasedThisFrame())
			{
				StopCoroutine(currentco);
				RemoveSpeedMul(0.1f);
				isRunning = false;
			}
		}
		if(startRun)
		{
			if(currentco != null)
			{
				StopCoroutine(currentco);
			}
			currentco = StartCoroutine(StartRun());
		}
	}
	IEnumerator StartRun()
	{
		startRun = false;
		SpeedMul(0.1f);
		yield return new WaitForSeconds(1f);
		runStartDirection = PlayerFacingDirection;
		RemoveSpeedMul(0.1f);
		isRunning = true;
	}
	public void StartHook(Vector3 target, GameObject Hook)
	{
		StartCoroutine(HookPlayer(target, Hook));
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
