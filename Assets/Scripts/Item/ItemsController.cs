using UnityEngine;
using UnityEngine.InputSystem;

public class ItemsController : MonoBehaviour
{

	public enum Item
	{
		None,
		HookShot,
		WindFan,
		BowAndArrow,
		Bomb
	}
	public Item Slot1;
	public Item Slot2;
	
	public InputAction ItemSlot1Action;
	public InputAction ItemSlot2Action;

	ItemsController itemsController;
	PlayerMovement PM;
	[SerializeField] Transform ProjectileShotLocation;

	GameObject Interacter;
	[Header("HookShot")]
	public float HookSpeed;
	[SerializeField] GameObject Hook;
	public bool HookShoted = false;
	float MaxDistance;
	
	private void Start()
	{
		Slot1 = Item.None;
		Slot2 = Item.None;
		ItemSlot1Action = InputSystem.actions.FindAction("ItemSlot1");
		ItemSlot2Action = InputSystem.actions.FindAction("ItemSlot2");
		Interacter = transform.GetChild(3).gameObject;
		PM = GetComponent<PlayerMovement>();
	}


	private void Update()
	{
		if (ItemSlot1Action.WasPressedThisFrame())
		{
			switch (Slot1)
			{
				case Item.HookShot:
					HookShot();
					break;
				
				default:
					break;
			}
		}
	}


	void HookShot()
	{
		if(!HookShoted)
		{
			HookShoted = true;
			GameObject FHook = Instantiate(Hook, ProjectileShotLocation.position, Interacter.transform.rotation);
			Rigidbody frb = FHook.GetComponent<Rigidbody>();
			frb.linearVelocity = HookSpeed * new Vector3(PM.PlayerFacingDirection.x,0,PM.PlayerFacingDirection.y);
		}
		
	}


	public void ChangeItemSlot1(Item nextItem)
	{
		if(nextItem == Slot2)
		{
			Slot2 = Item.None;
		}
		Slot1 = nextItem;
	}
	public void ChangeItemSlot2(Item nextItem)
	{
		if(nextItem == Slot1)
		{
			Slot1 = Item.None;
		}
		Slot2 = nextItem;
	}
}
