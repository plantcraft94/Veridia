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
		Interacter.transform.position = new Vector3(transform.position.x + 0.5f * PM.AnimInput.x, transform.position.y, transform.position.z + 0.5f * PM.AnimInput.y);
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
}
