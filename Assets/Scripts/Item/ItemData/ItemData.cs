using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
	public string ItemName;
	[TextArea(2, 3)]
	public string itemDescription;
	public bool CustomInput;
	public InputActionReference inputPrompt;
}
