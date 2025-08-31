using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventSystemAccess : MonoBehaviour
{
	[SerializeField] private EventSystem eventSystem;
	[SerializeField] private Selectable firstItemToSelect;

	private void Start()
	{
		if (eventSystem == null)
			eventSystem = EventSystem.current;

		if (eventSystem == null || firstItemToSelect == null) return;

		eventSystem.SetSelectedGameObject(null);
		eventSystem.SetSelectedGameObject(firstItemToSelect.gameObject);
		firstItemToSelect.Select();
	}
}
