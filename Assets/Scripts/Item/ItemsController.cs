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
	PlayerMovement PM;
	[SerializeField] Transform ProjectileShotLocation;

	GameObject Interacter;
	[Header("HookShot")]
	public float HookSpeed;
	[SerializeField] GameObject Hook;
	public bool HookShoted = false;
	public float MaxDistance;
	[Header("WindFan")]
	[SerializeField] GameObject WindProjectile;
	public float WindSpeed;

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
		if(UiController.Instance.isInInv)
		{
			return;
		}
		if (ItemSlot1Action.WasPressedThisFrame())
		{
			switch (Slot1)
			{
				case Item.HookShot:
					HookShot();
					break;
				case Item.WindFan:
					WindFan();
					break;


				default:
					break;
			}
		}
		else if (ItemSlot2Action.WasPressedThisFrame())
		{
			switch (Slot2)
			{
				case Item.HookShot:
					HookShot();
					break;
				case Item.WindFan:
					WindFan();
					break;


				default:
					break;
			}
		}
	}


	void HookShot()
	{
		if (!HookShoted)
		{
			HookShoted = true;
			GameObject FHook = Instantiate(Hook, ProjectileShotLocation.position, Interacter.transform.rotation);
			Rigidbody frb = FHook.GetComponent<Rigidbody>();
			frb.linearVelocity = HookSpeed * new Vector3(PM.PlayerFacingDirection.x, 0, PM.PlayerFacingDirection.y);
		}

	}
	void WindFan()
	{
		GameObject FWind = Instantiate(WindProjectile, ProjectileShotLocation.position, Interacter.transform.rotation);
		Rigidbody frb = FWind.GetComponent<Rigidbody>();
		frb.linearVelocity = WindSpeed * new Vector3(PM.PlayerFacingDirection.x, 0, PM.PlayerFacingDirection.y);
	}


	public void ChangeItemSlot1(Item nextItem)
	{
		if (nextItem == Slot2)
		{
			Slot2 = Item.None;
		}
		Slot1 = nextItem;
	}
	public void ChangeItemSlot2(Item nextItem)
	{
		if (nextItem == Slot1)
		{
			Slot1 = Item.None;
		}
		Slot2 = nextItem;
	}
}
