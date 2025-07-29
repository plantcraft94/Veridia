using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
	GameObject selected;


	[Header("Input")]
	public InputAction ItemSlot1Action;
	public InputAction ItemSlot2Action;

	ItemsController itemsController;
	private void Start()
	{
		ItemSlot1Action = InputSystem.actions.FindAction("ItemSlot1");
		ItemSlot2Action = InputSystem.actions.FindAction("ItemSlot2");
		itemsController = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemsController>();
	}
	private void Update()
	{
		selected = EventSystem.current.currentSelectedGameObject;
		if (ItemSlot1Action.WasPressedThisFrame() && selected != null)
		{
			itemsController.ChangeItemSlot1(selected.GetComponent<ItemContainer>().ChangeItemTo);
		}
		else if (ItemSlot2Action.WasPressedThisFrame() && selected != null)
		{
			itemsController.ChangeItemSlot2(selected.GetComponent<ItemContainer>().ChangeItemTo);
		}
	}
}
