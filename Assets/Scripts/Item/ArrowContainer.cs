using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ArrowContainer : MonoBehaviour,ISelectHandler
{
	[SerializeField] ItemData itemData;
	public ArrowElement ChangeElementTo;
	ItemsController IC;
	Description des;

	Image image;
	private void Awake()
	{
		IC = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemsController>();
		image = transform.GetChild(0).GetComponent<Image>();
	}
	private void OnEnable()
	{
		if(des == null)
		{
			des = GameObject.Find("Description").GetComponent<Description>();
		}
		if (GameManager.Instance != null)
		{
			if (GameManager.Instance.HasItem(Item.BowAndArrow))
			{
				image.sprite = ItemSprite.Instance.SpriteArrow[ChangeElementTo];
			}
			else if (!GameManager.Instance.HasItem(Item.BowAndArrow) && image != null)
			{
				image.sprite = ItemSprite.Instance.SpriteItem[Item.None];
			}
		}
	}
	public void ChangeArrow()
	{
		if (GameManager.Instance.HasItem(Item.BowAndArrow))
		{
			IC.CurrentArrowElement = ChangeElementTo;
		}
		else
		{
			IC.CurrentArrowElement = ArrowElement.Normal;
		}
	}
	public void OnSelect(BaseEventData eventData)
	{
		if(GameManager.Instance.HasItem(Item.BowAndArrow))
		{
			des.ChangeDescription(itemData.ItemName, itemData.itemDescription, itemData.CustomInput,itemData.inputPrompt);
		}
		else
		{
			des.ChangeDescription("", "", false,itemData.inputPrompt);
		}
	}
}
