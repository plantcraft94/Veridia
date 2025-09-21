using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
	[SerializeField] GameObject HealthBar;
	Image Health;
	[SerializeField] GameObject MagicBar;
	Image Magic;
	[SerializeField] GameObject Item1;
	[SerializeField] GameObject Item2;
	GameObject PlayerGameObject;
	ItemsController Icontroller;
	PlayerResource PR;
	private void Start()
	{
		Health = HealthBar.GetComponent<Image>();
        Health.type = Image.Type.Filled;
        Health.fillMethod = Image.FillMethod.Horizontal;
        Magic = MagicBar.GetComponent<Image>();
        Magic.type = Image.Type.Filled;
        Magic.fillMethod = Image.FillMethod.Horizontal;
        PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
		Icontroller = PlayerGameObject.GetComponent<ItemsController>();
		PR = PlayerGameObject.GetComponent<PlayerResource>();
	}
	private void Update()
	{
		Health.fillAmount = PR.Health / PR.MaxHealth;
		Magic.fillAmount = PR.Magic / 100;
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public void UpdateItemIcon()
	{
		Item1.transform.GetChild(0).GetComponent<Image>().sprite = ItemSprite.Instance.SpriteItem[Icontroller.Slot1];
		Item2.transform.GetChild(0).GetComponent<Image>().sprite = ItemSprite.Instance.SpriteItem[Icontroller.Slot2];
	}
}
