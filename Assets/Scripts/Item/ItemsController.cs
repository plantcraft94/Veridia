using UnityEngine;

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
	
	private void Start()
	{
		Slot1 = Item.None;
		Slot2 = Item.None;
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
