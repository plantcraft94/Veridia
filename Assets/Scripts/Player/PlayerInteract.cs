using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
	InputAction interactAction;
	bool IsHolding = false;

	public GameObject Interacter;

	ThrowableObject TO;

	PlayerMovement PM;
	private void Start()
	{
		interactAction = InputSystem.actions.FindAction("Interact");
		PM = GetComponent<PlayerMovement>();
	}
	private void Update()
	{
		RotateBasedOnDirection(PM.PlayerFacingDirection);
		if (IsHolding && interactAction.WasPressedThisFrame())
		{
			TO.Grabbed = 2;
			IsHolding = false;
		}
		if (!IsHolding && interactAction.WasReleasedThisFrame())
		{
			Interacter.SetActive(true);
		}
	}
	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.CompareTag("Throwable"))
		{
			if (!IsHolding && interactAction.WasPressedThisFrame())
			{
				Interacter.SetActive(false);
				TO = other.GetComponent<ThrowableObject>();
				TO.Grabbed = 1;
				IsHolding = true;
			}
		}
	}
	void RotateBasedOnDirection(Vector2 direction)
	{
		if (direction == Vector2.zero) return;

		float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

		Interacter.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
	}
}
