using UnityEngine;
using UnityEngine.EventSystems;

public class BowSubMenu : MonoBehaviour
{
	GameObject PlayerGameObject;
	[SerializeField] GameObject FirstMaterial;
	RememberCurrentlySelectedGameObject RCSG;
	private void Awake()
	{
		RCSG = FindFirstObjectByType<RememberCurrentlySelectedGameObject>().GetComponent<RememberCurrentlySelectedGameObject>();
		PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
	}
	private void Update()
	{

	}
}
