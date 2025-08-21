using UnityEngine;
using System;

public class Chest : MonoBehaviour
{
	[Tooltip("The item this chest contains")]
	public ChestItem content;

	[SerializeField] private bool isOpen = false;
	Animator anim;
	private void Start()
	{
		anim = GetComponent<Animator>();
	}

	public void Open()
	{
		if (isOpen) return;

		Debug.Log("Chest opened! Got: " + content.GetName());

		GiveItemToPlayer();

		isOpen = true;
		anim.SetBool("Open", true);
		// Optionally play animation, disable collider, etc.
	}

	private void GiveItemToPlayer()
	{
		switch (content.type)
		{
			case ChestItem.ItemType.KeyItems:
				GameManager.Instance.ObtainItem(content.KeyItem);
				break;

			case ChestItem.ItemType.DungeonItem:
				DungeonManager.Instance.ObtainDungeonItem(content.DungeonItem); // or however you handle it
				break;
			default:
				break;
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			Player.Instance.PI.isInChestRange = true;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			Player.Instance.PI.isInChestRange = false;
		}
	}
}
[Serializable]
public class ChestItem
{
	public ItemType type;

	[SerializeField, HideInInspector] private Item _keyItem;
	[SerializeField, HideInInspector] private DungeonItems _dungeonItem;

	// Expose public properties with inspector conditionals
	public Item KeyItem
	{
		get => _keyItem;
		set => _keyItem = value;
	}

	public DungeonItems DungeonItem
	{
		get => _dungeonItem;
		set => _dungeonItem = value;
	}

	public enum ItemType
	{
		KeyItems,
		DungeonItem
	}

	public string GetName()
	{
		return type switch
		{
			ItemType.KeyItems => KeyItem.ToString(),
			ItemType.DungeonItem => DungeonItem.ToString(),
			_ => "Unknown"
		};
	}

	// Optional: Enforce only one item is set
	public void OnValidate()
	{
		// This runs in Editor when values change
		if (type == ItemType.KeyItems)
		{
			_dungeonItem = 0; // assuming DungeonItems is an enum
		}
		else if (type == ItemType.DungeonItem)
		{
			_keyItem = 0;
		}
	}
}
