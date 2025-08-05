using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
	[SerializeField] GameObject HealthBar;
	[SerializeField] GameObject MagicBar;
	[SerializeField] GameObject Item1;
	[SerializeField] GameObject Item2;
	GameObject PLayerGameObject;
	ItemsController Icontroller;
	private void Start()
	{
		PLayerGameObject = GameObject.FindGameObjectWithTag("Player");
		Icontroller = PLayerGameObject.GetComponent<ItemsController>();
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public void UpdateItemIcon()
	{
		Item1.transform.GetChild(0).GetComponent<Image>().sprite = ItemSprite.Instance.SpriteItem[Icontroller.Slot1];
		Item2.transform.GetChild(0).GetComponent<Image>().sprite = ItemSprite.Instance.SpriteItem[Icontroller.Slot2];
	}
}
