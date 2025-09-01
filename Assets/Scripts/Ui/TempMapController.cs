using UnityEngine;

public class TempMapController : MonoBehaviour
{
	[SerializeField] GameObject MapMap;
	[SerializeField] GameObject CompassMap;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private void OnEnable()
	{
		MapMap.SetActive(DungeonManager.Instance.HasMap);
		CompassMap.SetActive(DungeonManager.Instance.HasCompass);
	}
}
