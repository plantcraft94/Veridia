using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomInputItems : MonoBehaviour,ISelectHandler
{
	[SerializeField] ItemData itemData;
	public Item item;
	Description des;

	Image image;
	private void Awake()
	{
		image = transform.GetChild(0).GetComponent<Image>();
	}
	private void OnEnable()
	{
		if(des == null)
		{
			des = GameObject.Find("Description").GetComponent<Description>();
		}
		if(GameManager.Instance != null)
		{	
			if (GameManager.Instance.HasItem(item))
			{
				image.sprite = ItemSprite.Instance.SpriteItem[item];
			}
			else if (!GameManager.Instance.HasItem(item) && image != null)
			{
				image.sprite = ItemSprite.Instance.SpriteItem[Item.None];
			}
		}
	}
	public void OnSelect(BaseEventData eventData)
	{
		if(GameManager.Instance.HasItem(item))
		{
			des.ChangeDescription(itemData.ItemName, itemData.itemDescription, itemData.CustomInput,itemData.inputPrompt);
		}
		else if(!GameManager.Instance.HasItem(item) || itemData == null)
		{
			des.ChangeDescription("", "", false,itemData.inputPrompt);
		}
	}
}
