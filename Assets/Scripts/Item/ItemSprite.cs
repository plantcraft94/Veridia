using UnityEngine;
using AYellowpaper.SerializedCollections;

public class ItemSprite : MonoBehaviour
{
	public static ItemSprite Instance { get; private set; }
	
	[SerializedDictionary("Item", "Sprite")]
	public SerializedDictionary<Item, Sprite> SpriteItem;
	[SerializedDictionary("ArrowElement", "Sprite")]
	public SerializedDictionary<ArrowElement, Sprite> SpriteArrow;
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
}
