using UnityEngine;
using UnityEngine.InputSystem;

public class UiController : MonoBehaviour
{
	[Header("Inventory_Item")]
	[SerializeField] GameObject ItemInventory;
	InputAction OpenInv;
	private void Start()
	{
		OpenInv = InputSystem.actions.FindAction("OpenInventory");
		ItemInventory.SetActive(false);
	}
	private void Update()
	{
		if(!GameManager.Instance.isInInv && OpenInv.WasPressedThisFrame())
		{
			GameManager.Instance.isInInv = true;
			Time.timeScale = 0f;
			ItemInventory.SetActive(true);
		}
		else if(GameManager.Instance.isInInv && OpenInv.WasPressedThisFrame())
		{
			GameManager.Instance.isInInv = false;
			Time.timeScale = 1f;
			ItemInventory.SetActive(false);
		}
	}
}
