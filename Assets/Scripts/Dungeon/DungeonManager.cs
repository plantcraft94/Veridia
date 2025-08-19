using UnityEngine;

public class DungeonManager : MonoBehaviour
{
	public static DungeonManager Instance{ get; set; }
	public Vector3 CurrentCheckPointPos;
	GameObject PlayerGameObject;
	private void Awake()
	{
		Instance = this;
	}
	private void Start()
	{
		PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
	}
	public void TpPlayer(Vector3 pos)
	{
		PlayerGameObject.transform.position = pos;
	}
}
