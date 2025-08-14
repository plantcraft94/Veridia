using UnityEngine;
using UnityEngine.UI;

public class ArrowContainer : MonoBehaviour
{
	public string Name;
	public string Description;
	public ArrowElement ChangeElementTo;
	ItemsController IC;

	Image image;
	private void Awake()
	{
		IC = GameObject.FindGameObjectWithTag("Player").GetComponent<ItemsController>();
		image = transform.GetChild(0).GetComponent<Image>();
	}
	private void OnEnable()
	{
		if (GameManager.Instance != null)
		{
			if (GameManager.Instance.HasItem(Item.BowAndArrow))
			{
				image.sprite = ItemSprite.Instance.SpriteArrow[ChangeElementTo];
			}
			else if (!GameManager.Instance.HasItem(Item.BowAndArrow) && image != null)
			{
				image.sprite = ItemSprite.Instance.SpriteArrow[ArrowElement.Normal];
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
}
