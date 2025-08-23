using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
	public UnityEvent OnStep;
	public UnityEvent OnRelease;
	enum PressureType
	{
		Press,
		Hold
	}
	[SerializeField] PressureType pressureType;
	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player") || other.gameObject.GetComponent<PushableObject>()!=null)
		{
			OnStep.Invoke();
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if(pressureType == PressureType.Hold)
		{
			if(other.gameObject.CompareTag("Player") || other.gameObject.GetComponent<PushableObject>()!=null)
			{
				OnRelease.Invoke();
			}
		}
	}
}
