using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemContainer : MonoBehaviour, ISelectHandler
{
	[SerializeField] ItemData itemData;
	public Item ChangeItemTo;
	Description des;

	Image image;
	private void Awake()
	{
		image = transform.GetChild(0).GetComponent<Image>();
	}
	private void OnEnable()
	{
		if (des == null)
		{
			des = GameObject.Find("Description").GetComponent<Description>();
		}
		if (GameManager.Instance != null)
		{
			if (GameManager.Instance.HasItem(ChangeItemTo))
			{
				image.sprite = ItemSprite.Instance.SpriteItem[ChangeItemTo];
			}
			else if (!GameManager.Instance.HasItem(ChangeItemTo) && image != null)
			{
				image.sprite = ItemSprite.Instance.SpriteItem[Item.None];
			}
		}
	}
	public Item ChangeItem()
	{
		if (GameManager.Instance.HasItem(ChangeItemTo))
		{
			return ChangeItemTo;
		}
		return Item.None;
	}
	public void OnSelect(BaseEventData eventData)
	{
		if (GameManager.Instance.HasItem(ChangeItemTo))
		{
			des.ChangeDescription(itemData.ItemName, itemData.itemDescription, itemData.CustomInput, itemData.inputPrompt);
		}
		else if (itemData == null || !GameManager.Instance.HasItem(ChangeItemTo))
		{
			des.ChangeDescription("", "", false, itemData.inputPrompt);
		}
	}
}