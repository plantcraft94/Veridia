using UnityEngine;
using UnityEngine.InputSystem;

public class UiController : MonoBehaviour
{
	public static UiController Instance{ get; private set; }
	[Header("Inventory_Item")]
	[SerializeField] GameObject ItemInventory;
	InputAction OpenInv;
	public bool isInInv = false;
	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
	}
	private void Start()
	{
		OpenInv = InputSystem.actions.FindAction("OpenInventory");
		ItemInventory.SetActive(false);
	}
	private void Update()
	{
		if(!isInInv && OpenInv.WasPressedThisFrame())
		{
			isInInv = true;
			Time.timeScale = 0f;
			ItemInventory.SetActive(true);
		}
		else if(isInInv && OpenInv.WasPressedThisFrame())
		{
			isInInv = false;
			Time.timeScale = 1f;
			ItemInventory.SetActive(false);
		}
	}
}
