using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempDialogueBox : MonoBehaviour
{
	[SerializeField] TMP_Text TextBox;
	[SerializeField] GameObject DialogueBox;
	InputAction interactAction;
	private void Start()
	{
		interactAction = InputSystem.actions.FindAction("Interact");
		HideDialogue();
	}
	private void Update()
	{
		if(GameManager.Instance.isInDialogueBox)
		{
			if(interactAction.WasPressedThisFrame())
			{
				HideDialogue();
			}
		}
	}
	public void DisplayDialogue(string message)
	{
		
		GameManager.Instance.isInDialogueBox = true;
		DialogueBox.SetActive(true);
		TextBox.text = message;
	}
	public void HideDialogue()
	{
		TextBox.text = "";
		DialogueBox.SetActive(false);
		GameManager.Instance.isInDialogueBox = true;
	}
}
