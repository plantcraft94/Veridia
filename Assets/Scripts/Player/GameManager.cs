using UnityEngine;
using AYellowpaper.SerializedCollections;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	[SerializedDictionary("Item", "HasIt?")]
	[SerializeField] SerializedDictionary<Item, bool> ItemPossesion;
	
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
		ItemPossesion[Item.None] = true;
	}
	public bool HasItem(Item item)
	{
		return ItemPossesion[item];
	}

}
