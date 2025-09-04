using UnityEngine;
using UnityEngine.SceneManagement;
public class Door : MonoBehaviour
{
	Animator anim;
	public bool InteractWithThisDoor = false;
	enum DoorType
	{
		Normal,
		Lock,
		Boss,
		Puzzle //use when design a puzzle related to this door, else, use normal / lock door

	}
	[SerializeField] DoorType Type;
	TempDialogueBox tempDialogueBox;
	private void Start()
	{
		tempDialogueBox = GameObject.FindGameObjectWithTag("UI").GetComponent<TempDialogueBox>();
		anim = GetComponent<Animator>();
		if (Type == DoorType.Lock || Type == DoorType.Boss || Type == DoorType.Puzzle)
		{
			CloseDoor();
		}
	}
	private void Update()
	{
		if (!InteractWithThisDoor)
		{
			return;
		}
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
			case DoorType.Puzzle:
				PuzzleDoor();
				break;
			default:
				break;
		}
	}
	void LockDoor()
	{
		if (Player.Instance.PI.IsInteractWithDoor && !DungeonManager.Instance.IsInChallenge)
		{
			if (DungeonManager.Instance.KeyAmount > 0)
			{
				DungeonManager.Instance.KeyAmount--;
				Type = DoorType.Normal;
				OpenDoor();
			}
			else
			{
				tempDialogueBox.DisplayDialogue("This door need a key to unlock");
			}
		}
		InteractWithThisDoor = false;
	}
	void BossDoor()
	{
		if (Player.Instance.PI.IsInteractWithDoor && !DungeonManager.Instance.IsInChallenge)
		{
			if (DungeonManager.Instance.BossKeyAmount > 0)
			{
				DungeonManager.Instance.BossKeyAmount--;
				Type = DoorType.Normal;
				OpenDoor();
				SceneManager.LoadScene("DemoWin");
			}
			else
			{
				tempDialogueBox.DisplayDialogue("This door need a boss key to unlock");
			}
		}
		InteractWithThisDoor = false;
	}
	void PuzzleDoor()
	{
		if (Player.Instance.PI.IsInteractWithDoor && !DungeonManager.Instance.IsInChallenge)
		{
			tempDialogueBox.DisplayDialogue("This door won't budge ... There's no keyhole. There must be another way to open it.");
		}
		InteractWithThisDoor = false;
	}
	public void SolvePuzzleDoor()
	{
		if(Type != DoorType.Puzzle)
		{
			return;
		}
		Type = DoorType.Normal;
		OpenDoor();
	}
	public void OpenDoor()
	{
		if (Type == DoorType.Normal)
		{
			anim.SetBool("IsOpen", true);
		}
	}
	public void CloseDoor()
	{
		anim.SetBool("IsOpen", false);
	}
}
