using UnityEngine;
using PrimeTween;

public class RoomFog : MonoBehaviour
{
	SpriteRenderer SR;
	private void Awake()
	{
		SR = GetComponent<SpriteRenderer>();
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
			Tween.Alpha(SR, endValue: 1f, 0.2f, Ease.InOutSine);
		}
	}
}
