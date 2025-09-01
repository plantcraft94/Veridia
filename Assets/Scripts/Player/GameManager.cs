using UnityEngine;
using AYellowpaper.SerializedCollections;
using PrimeTween;
using Unity.Cinemachine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	[SerializedDictionary("Item", "HasIt?")]
	[SerializeField] SerializedDictionary<Item, bool> ItemPossesion;
	GameObject cam;
	CinemachineBasicMultiChannelPerlin amp;
	
	public bool isInInv = false;
	public bool isInMap = false;
	public bool isInDialogueBox = false;
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
		foreach (Item item in System.Enum.GetValues(typeof(Item)))
		{
			if (!ItemPossesion.ContainsKey(item))
			{
				ItemPossesion[item] = false;
			}
		}
		ItemPossesion[Item.None] = true;
		cam = GameObject.Find("CinemachineCamera").gameObject;
		amp = cam.GetComponent<CinemachineBasicMultiChannelPerlin>();
	}
	public bool HasItem(Item item)
	{
		return ItemPossesion[item];
	}
	public void ShakeCamera()
	{
		amp.AmplitudeGain = 1f;
		Tween.Delay(duration: 0.5f, () => { amp.AmplitudeGain = 0f; });
	}
	public void ObtainItem(Item item)
	{
		ItemPossesion[item] = true;
	}
}
