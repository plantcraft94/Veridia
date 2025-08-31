using UnityEngine;
using PrimeTween;

public class RoomFog : MonoBehaviour
{
	SpriteRenderer SR;
	RoomStateManager RSM;
	private void Awake()
	{
		SR = GetComponent<SpriteRenderer>();
	}
	private void Start()
	{
		RSM = GetComponent<RoomStateManager>();
	}
	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			Tween.Alpha(SR, endValue: 0f, 0.2f, Ease.InOutSine);
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			Tween.Alpha(SR, endValue: 1f, 0.2f, Ease.InOutSine).OnComplete(() => RSM.DestroyEnemy());
		}
	}
}
