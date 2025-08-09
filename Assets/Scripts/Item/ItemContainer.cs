using UnityEngine;
using UnityEngine.UI;

public class ItemContainer : MonoBehaviour
{
	public string Name;
	public string Description;
	public Item ChangeItemTo;

	Image image;
	private void Start()
	{
		image = transform.GetChild(0).GetComponent<Image>();
	}
	private void OnEnable()
	{
		if(GameManager.Instance != null)
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

}