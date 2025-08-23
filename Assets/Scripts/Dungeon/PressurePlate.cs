using System.Collections.Generic;
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
	[SerializeField] List<GameObject> ObjectsToSetActiveIFNeeded;
	bool Pressed = false;
	bool NoObject = true;
	private void Start()
	{
		if (ObjectsToSetActiveIFNeeded.Count > 0)
		{
			NoObject = false;
			foreach (GameObject Object in ObjectsToSetActiveIFNeeded)
			{
				Object.SetActive(!Object.activeSelf);
			}
		}
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player") || other.gameObject.GetComponent<PushableObject>() != null)
		{
			if (pressureType == PressureType.Hold)
			{
				Debug.Log("testt");
				OnStep.Invoke();
				if (!NoObject)
				{
					foreach (GameObject Object in ObjectsToSetActiveIFNeeded)
					{
						Object.SetActive(!Object.activeSelf);
					}
				}
			}
			else if (pressureType == PressureType.Press && !Pressed)
			{
				Pressed = true;
				OnStep.Invoke();
				if (!NoObject)
				{
					foreach (GameObject Object in ObjectsToSetActiveIFNeeded)
					{
						Object.SetActive(!Object.activeSelf);
					}
				}
			}

		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (pressureType == PressureType.Hold)
		{
			if (other.gameObject.CompareTag("Player") || other.gameObject.GetComponent<PushableObject>() != null)
			{
				if (pressureType == PressureType.Hold)
				{
					Debug.Log("testt");
					OnRelease.Invoke();
					if (!NoObject)
					{
						foreach (GameObject Object in ObjectsToSetActiveIFNeeded)
						{
							Object.SetActive(!Object.activeSelf);
						}
					}
				}
			}
		}
	}
}
