using UnityEngine;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using Physics = Nomnom.RaycastVisualization.VisualPhysics;
#else
using Physics = UnityEngine.Physics;
#endif

public class PlayerInteract : MonoBehaviour
{
	InputAction interactAction;
	bool IsHolding = false;

	public GameObject Interacter;

	public bool isInChestRange;
	bool isNearChest;

	ThrowableObject TO;

	PlayerMovement PM;
	public bool IsInteractWithDoor;
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
		RaycastHit hit;
		if (Physics.Raycast(Interacter.transform.position, Interacter.transform.forward, out hit, 1f) && Interacter.activeSelf)
		{
			if (hit.collider.gameObject.GetComponent<ThrowableObject>() != null)
			{
				if (!IsHolding && interactAction.WasPressedThisFrame())
				{
					Interacter.SetActive(false);
					TO = hit.collider.GetComponent<ThrowableObject>();
					TO.Grabbed = 1;
					IsHolding = true;
				}
			}
			if(hit.collider.gameObject.CompareTag("Door"))
			{
				IsInteractWithDoor = interactAction.WasPressedThisFrame();
				if(IsInteractWithDoor)
				{
					hit.collider.gameObject.GetComponent<Door>().InteractWithThisDoor = true;
				}
			}
			isNearChest = hit.collider.CompareTag("Chest");
			if(interactAction.WasPressedThisFrame() && isNearChest)
			{
				if(isInChestRange)
				{
					hit.collider.gameObject.GetComponent<Chest>().Open();
				}
				else if(!isInChestRange)
				{
					Debug.Log("Cannot Open on this side :(");
				}
				
			}
		}
	}
	// private void OnTriggerStay(Collider other)
	// {
	// 	if (other.gameObject.GetComponent<ThrowableObject>() != null)
	// 	{
	// 		if (!IsHolding && interactAction.WasPressedThisFrame())
	// 		{
	// 			Interacter.SetActive(false);
	// 			TO = other.GetComponent<ThrowableObject>();
	// 			TO.Grabbed = 1;
	// 			IsHolding = true;
	// 		}
	// 	}
	// }
	void RotateBasedOnDirection(Vector2 direction)
	{
		if (direction == Vector2.zero) return;

		float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

		Interacter.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
	}
}
