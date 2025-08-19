using UnityEngine;
using UnityEngine.InputSystem;
public enum Item
{
	None,
	HookShot,
	WindFan,
	BowAndArrow,
	Bomb,
	WingBoot
}

public enum ArrowElement
{
	Normal,
	Fire,
	Ice
}
public class ItemsController : MonoBehaviour
{

	public Item Slot1;
	public Item Slot2;



	public InputAction ItemSlot1Action;
	public InputAction ItemSlot2Action;
	PlayerMovement PM;
	PlayerResource PR;
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
	[SerializeField] float WindMagicCost;
	GameObject FWind;
	[Header("Bomb")]
	[SerializeField] GameObject BombObject;
	GameObject FBomb;
	[Header("Bow")]
	[SerializeField] GameObject Arrow;
	[SerializeField] float Damage;
	public bool aim = false;
	float damageMul = 0.25f;
	[SerializeField] float ArrowSpeed;
	bool aim1 = false;
	bool aim2 = false;

	public ArrowElement CurrentArrowElement;

	private void Start()
	{
		Slot1 = Item.None;
		Slot2 = Item.None;
		CurrentArrowElement = ArrowElement.Normal;
		
		ItemSlot1Action = InputSystem.actions.FindAction("ItemSlot1");
		ItemSlot2Action = InputSystem.actions.FindAction("ItemSlot2");
		
		Interacter = transform.GetChild(3).gameObject;
		
		PM = GetComponent<PlayerMovement>();
		PR = GetComponent<PlayerResource>();
	}


	private void Update()
	{
		if (aim)
		{
			damageMul += Time.deltaTime;
			if (damageMul > 1.25f)
			{
				damageMul = 1.25f;
			}
			if (ItemSlot1Action.WasReleasedThisFrame() && aim1)
			{
				ShootArrow();
			}
			else if (ItemSlot2Action.WasReleasedThisFrame() && aim2)
			{
				ShootArrow();
			}
			return;
		}
		if (GameManager.Instance.isInInv)
		{
			return;
		}
		if (PM.isJumping)
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
				case Item.Bomb:
					Bomb();
					break;
				case Item.BowAndArrow:
					aim = true;
					aim1 = true;
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
				case Item.Bomb:
					Bomb();
					break;
				case Item.BowAndArrow:
					aim = true;
					aim2 = true;
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
			ItemFailSafe(FHook);
		}

	}
	void WindFan()
	{
		if (FWind == null && PR.Magic >= WindMagicCost)
		{
			FWind = Instantiate(WindProjectile, ProjectileShotLocation.position, Interacter.transform.rotation);
			PR.MinusMagic(WindMagicCost);
			Rigidbody frb = FWind.GetComponent<Rigidbody>();
			frb.linearVelocity = WindSpeed * new Vector3(PM.PlayerFacingDirection.x, 0, PM.PlayerFacingDirection.y);
			ItemFailSafe(FWind);
		}
	}
	void Bomb()
	{
		if (FBomb == null)
		{
			FBomb = Instantiate(BombObject, ProjectileShotLocation.position, Quaternion.identity);
		}
	}
	void ShootArrow()
	{

		if (PR.ArrowAmount > 0)
		{
			var Farrow = Instantiate(Arrow, ProjectileShotLocation.position, Interacter.transform.rotation);
			Farrow.GetComponent<Arrow>().ArrowType(damageMul, false, CurrentArrowElement);
			Rigidbody frb = Farrow.GetComponent<Rigidbody>();
			frb.linearVelocity = ArrowSpeed * new Vector3(PM.PlayerFacingDirection.x, 0, PM.PlayerFacingDirection.y);
		}
		else
		{
			var Farrow = Instantiate(Arrow, ProjectileShotLocation.position, Interacter.transform.rotation);
			Farrow.GetComponent<Arrow>().ArrowType(damageMul, true, CurrentArrowElement);
			Rigidbody frb = Farrow.GetComponent<Rigidbody>();
			frb.linearVelocity = ArrowSpeed * new Vector3(PM.PlayerFacingDirection.x, 0, PM.PlayerFacingDirection.y);
		}
		aim = false;
		damageMul = 0.25f;
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
	void ItemFailSafe(GameObject theObj)
	{
		Destroy(theObj, 2f);
	}
}
