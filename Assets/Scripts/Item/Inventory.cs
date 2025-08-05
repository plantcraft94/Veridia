using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
	GameObject selected;


	[Header("Input")]
	public InputAction ItemSlot1Action;
	public InputAction ItemSlot2Action;
	PlayerHUD playerHUD;

	ItemsController itemsController;
	private void Start()
	{
		ItemSlot1Action = InputSystem.actions.FindAction("ItemSlot1");
		ItemSlot2Action = InputSystem.actions.FindAction("ItemSlot2");
		itemsController = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemsController>();
		playerHUD = GameObject.FindGameObjectWithTag("PlayerHUD").GetComponent<PlayerHUD>();
	}
	private void Update()
	{
		selected = EventSystem.current.currentSelectedGameObject;
		if (ItemSlot1Action.WasPressedThisFrame() && selected.CompareTag("ItemContainer"))
		{
			itemsController.ChangeItemSlot1(selected.GetComponent<ItemContainer>().ChangeItemTo);
			playerHUD.UpdateItemIcon();
		}
		else if (ItemSlot2Action.WasPressedThisFrame() && selected.CompareTag("ItemContainer"))
		{
			itemsController.ChangeItemSlot2(selected.GetComponent<ItemContainer>().ChangeItemTo);
			playerHUD.UpdateItemIcon();
		}
	}
}
