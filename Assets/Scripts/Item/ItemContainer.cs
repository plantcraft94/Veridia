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
		image.sprite = ItemSprite.Instance.SpriteItem[ChangeItemTo];
	}
	
}
