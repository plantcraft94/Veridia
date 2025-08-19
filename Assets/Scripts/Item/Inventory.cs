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
	ItemContainer ICon;

	ItemsController itemsController;
	private void Awake()
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
			ICon = selected.GetComponent<ItemContainer>();
			itemsController.ChangeItemSlot1(ICon.ChangeItem());
			playerHUD.UpdateItemIcon();
		}
		else if (ItemSlot2Action.WasPressedThisFrame() && selected.CompareTag("ItemContainer"))
		{
			ICon = selected.GetComponent<ItemContainer>();
			itemsController.ChangeItemSlot2(ICon.ChangeItem());
			playerHUD.UpdateItemIcon();
		}
	}
}
