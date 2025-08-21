using UnityEngine;
public class Door : MonoBehaviour
{
	Animator anim;
	enum DoorType
	{
		Normal,
		Lock,
		Boss
	}
	[SerializeField] DoorType Type;
	private void Start()
	{
		anim = GetComponent<Animator>();
	}
	private void Update()
	{
		switch (Type)
		{
			case DoorType.Normal:
				break;
			case DoorType.Lock:
				LockDoor();
				break;
			case DoorType.Boss:
				BossDoor();
				break;
			default:
				break;
		}
	}
	void LockDoor()
	{
		if(Player.Instance.PI.IsInteractWithDoor && !DungeonManager.Instance.IsInChallenge)
		{
			if(DungeonManager.Instance.KeyAmount > 0)
			{
				Type = DoorType.Normal;
				OpenDoor();
			}
			else
			{
				Debug.Log("This door need a key to unlock");
			}
		}
	}
	void BossDoor()
	{
		if(Player.Instance.PI.IsInteractWithDoor && !DungeonManager.Instance.IsInChallenge)
		{
			if(DungeonManager.Instance.BossKeyAmount > 0)
			{
				Type = DoorType.Normal;
				OpenDoor();
			}
			else
			{
				Debug.Log("This door need a boss key to unlock");
			}
		}
	}
	public void OpenDoor()
	{
		if(Type == DoorType.Normal)
		{
			
			anim.SetBool("IsOpen", true);
		}
	}
	public void CloseDoor()
	{
		anim.SetBool("IsOpen", false);
	}
}
