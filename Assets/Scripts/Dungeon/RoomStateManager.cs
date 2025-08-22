using System.Collections.Generic;
using UnityEngine;

public class RoomStateManager : MonoBehaviour
{
	public RoomState State;
	public List<GameObject> DoorsThatNeedCloseIfChallengeRoom = new List<GameObject>();
	public List<GameObject> ChallengeSpawnPoint = new List<GameObject>();
	public List<GameObject> ChallengeEnemy = new List<GameObject>();
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
		if (other.gameObject.CompareTag("Player"))
		{
			playerInside = true;
			foreach (GameObject SpawnPoint in ChallengeSpawnPoint)
			{
				ObjectSpawn OSpawn = SpawnPoint.GetComponent<ObjectSpawn>();
				if (OSpawn.CurrentSpawn != null)
				{
					if (OSpawn.CurrentSpawn.GetComponent<AiHealth>() != null)
					{
						ChallengeEnemy.Remove(OSpawn.CurrentSpawn);
					}
				}
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
		if (other.gameObject.CompareTag("Player"))
		{
			playerInside = false;
		}
	}
	public void ChallengeClear()
	{
		DungeonManager.Instance.IsInChallenge = false;
		State = RoomState.Normal;
		foreach (GameObject Door in DoorsThatNeedCloseIfChallengeRoom)
		{
			Door.GetComponent<Door>().OpenDoor();
		}
	}
	public void OnEnemyDeath(GameObject Enemy)
	{
		ChallengeEnemy.Remove(Enemy);
		if(ChallengeEnemy.Count <= 0)
		{
			ChallengeClear();
		}
	}
}
