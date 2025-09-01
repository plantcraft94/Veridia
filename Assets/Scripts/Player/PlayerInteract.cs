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
	public bool IsHolding = false;

	public GameObject Interacter;

	public bool isInChestRange;
	public bool Interactable;

	ThrowableObject TO;

	PlayerMovement PM;
	public bool IsInteractWithDoor;
	[SerializeField] GameObject InteractionPrompt;
	private void Start()
	{
		interactAction = InputSystem.actions.FindAction("Interact");
		PM = GetComponent<PlayerMovement>();
	}
	private void Update()
	{
		RotateBasedOnDirection(PM.PlayerFacingDirection);
		if(GameManager.Instance.isInInv || GameManager.Instance.isInMap || GameManager.Instance.isInDialogueBox)
		{
			InteractionPrompt.SetActive(false);
			return;
		}
		InteractionPrompt.SetActive(Interactable);
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
			if(hit.collider.TryGetComponent<Chest>(out var chest))
			{
				Interactable = isInChestRange && !chest.isOpen;
				if(interactAction.WasPressedThisFrame())
				{
					if(isInChestRange)
					{
						chest.Open();
					}
					else if(!isInChestRange)
					{
						Debug.Log("Cannot Open on this side :(");
					}
					
				}
			}
			else if (hit.collider.gameObject.TryGetComponent<ThrowableObject>(out var throwable))
			{
				Interactable = true;
				if (!IsHolding && interactAction.WasPressedThisFrame())
				{
					Interacter.SetActive(false);
					TO = throwable;
					TO.Grabbed = 1;
					IsHolding = true;
				}
			}
			else if(hit.collider.gameObject.CompareTag("Door"))
			{
				Interactable = true;
				IsInteractWithDoor = interactAction.WasPressedThisFrame();
				if(IsInteractWithDoor)
				{
					hit.collider.transform.parent.gameObject.GetComponent<Door>().InteractWithThisDoor = true;
				}
			}
			else 
			{
				Interactable = false;
			}
		}
		else
		{
			Interactable = false;
		}
	}
	void RotateBasedOnDirection(Vector2 direction)
	{
		if (direction == Vector2.zero) return;

		float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

		Interacter.transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
	}
}
