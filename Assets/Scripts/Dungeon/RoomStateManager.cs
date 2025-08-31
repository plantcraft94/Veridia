using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomStateManager : MonoBehaviour
{
	public RoomState State;
	public List<GameObject> DoorsThatNeedCloseIfChallengeRoom = new List<GameObject>();
	public List<GameObject> ChallengeSpawnPoint = new List<GameObject>();
	public List<GameObject> ChallengeEnemy = new List<GameObject>();
	public UnityEvent OnRoomClear;
	bool playerInside = false;
	bool DoorIsClosed = false;

	private void Update()
	{
		if (playerInside && !DungeonManager.Instance.IsOnCheckPoint && !DoorIsClosed && State == RoomState.Challenge)
		{
			DoorIsClosed = true;
			DungeonManager.Instance.IsInChallenge = true;
			foreach (GameObject Door in DoorsThatNeedCloseIfChallengeRoom)
			{
				Door.GetComponent<Door>().CloseDoor();
			}
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && State == RoomState.Challenge)
		{
			playerInside = true;
			DungeonManager.Instance.IsInChallenge = true;
			foreach (GameObject SpawnPoint in ChallengeSpawnPoint)
			{
				ObjectSpawn OSpawn = SpawnPoint.GetComponent<ObjectSpawn>();
				OSpawn.Spawn();
				if (OSpawn.CurrentSpawn != null)
				{
					if (OSpawn.CurrentSpawn.GetComponent<AiHealth>() != null)
					{
						ChallengeEnemy.Add(OSpawn.CurrentSpawn);
						OSpawn.CurrentSpawn.GetComponent<AiHealth>().onDeath.AddListener(() => OnEnemyDeath(OSpawn.CurrentSpawn));
					}
				}
			}
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player") && State == RoomState.Challenge)
		{
			playerInside = false;
			DungeonManager.Instance.IsInChallenge = false;
		}
	}
	public void ChallengeClear()
	{
		DungeonManager.Instance.IsInChallenge = false;
		State = RoomState.Normal;
		OnRoomClear.Invoke();
		foreach (GameObject Door in DoorsThatNeedCloseIfChallengeRoom)
		{
			Door.GetComponent<Door>().OpenDoor();
		}
	}
	public void OnEnemyDeath(GameObject Enemy)
	{
		ChallengeEnemy.Remove(Enemy);
		if (ChallengeEnemy.Count <= 0 && DungeonManager.Instance.IsInChallenge && playerInside)
		{
			ChallengeClear();
		}
	}
	public void DestroyEnemy()
	{
		foreach (GameObject SpawnPoint in ChallengeSpawnPoint)
		{
			ObjectSpawn OSpawn = SpawnPoint.GetComponent<ObjectSpawn>();
			if (OSpawn.CurrentSpawn != null)
			{
				if (OSpawn.CurrentSpawn.GetComponent<AiHealth>() != null)
				{
					ChallengeEnemy.Remove(OSpawn.CurrentSpawn);
					Destroy(OSpawn.CurrentSpawn);
				}
			}
		}
	}
}
