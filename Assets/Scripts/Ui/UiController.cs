using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;


public class UiController : MonoBehaviour
{
	[Header("Inventory_Item")]
	[SerializeField] GameObject ItemInventory;
	[SerializeField] GameObject Map;
	InputAction OpenInv;
	InputAction OpenMap;
	private void Start()
	{
		OpenInv = InputSystem.actions.FindAction("OpenInventory");
		OpenMap = InputSystem.actions.FindAction("OpenMap");
		Tween.Delay(0.005f, () => { ItemInventory.SetActive(false); Map.SetActive(false); });
	}
	private void Update()
	{
		if (GameManager.Instance.isInDialogueBox)
		{
			return;
		}
		if (!GameManager.Instance.isInInv && !GameManager.Instance.isInMap && OpenInv.WasPressedThisFrame())
		{
			GameManager.Instance.isInInv = true;
			Time.timeScale = 0f;
			ItemInventory.SetActive(true);
		}
		else if (GameManager.Instance.isInInv && !GameManager.Instance.isInMap && OpenInv.WasPressedThisFrame())
		{
			GameManager.Instance.isInInv = false;
			Time.timeScale = 1f;
			ItemInventory.SetActive(false);
		}
		if (!GameManager.Instance.isInMap && !GameManager.Instance.isInInv && OpenMap.WasPressedThisFrame())
		{
			GameManager.Instance.isInMap = true;
			Time.timeScale = 0f;
			Map.SetActive(true);
		}
		else if (GameManager.Instance.isInMap && !GameManager.Instance.isInInv && OpenMap.WasPressedThisFrame())
		{
			GameManager.Instance.isInMap = false;
			Time.timeScale = 1f;
			Map.SetActive(false);
		}
	}
}
