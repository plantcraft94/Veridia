using UnityEngine;
using AYellowpaper.SerializedCollections;

public enum DungeonItems
{
	SmallKey,
	BossKey,
	Map,
	Compass
}
public enum RoomState
{
	Normal,
	Challenge
}
public class DungeonManager : MonoBehaviour
{
	[SerializeField] Transform FirstCheckPoint;
	public static DungeonManager Instance { get; set; }
	public Vector3 CurrentCheckPointPos;
	GameObject PlayerGameObject;
	public bool IsInChallenge;
	public bool IsOnCheckPoint;
	public int KeyAmount;
	public int BossKeyAmount;
	public bool HasCompass;
	public bool HasMap;
	private void Awake()
	{
		Instance = this;
	}
	private void Start()
	{
		PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
		CurrentCheckPointPos = FirstCheckPoint.position;
	}
	public void TpPlayer(Vector3 pos)
	{
		PlayerGameObject.transform.position = pos;
	}
	public void ObtainDungeonItem(DungeonItems item)
	{
		switch (item)
		{
			case DungeonItems.SmallKey:
				KeyAmount++;
				break;
			case DungeonItems.BossKey:
				BossKeyAmount++;
				break;
			case DungeonItems.Map:
				HasMap = true;
				break;
			case DungeonItems.Compass:
				HasCompass = true;
				break;
			default:
				break;
		}
	}
}
