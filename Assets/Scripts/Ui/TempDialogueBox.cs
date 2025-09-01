using System.Collections;
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
	private void OnEnable()
	{
		Debug.Log("Dialogue show");
	}
	public void DisplayDialogue(string message)
	{
		StartCoroutine(Display(message));
	}
	IEnumerator Display(string message)
	{
		DialogueBox.SetActive(true);
		TextBox.text = message;
		yield return null;
		GameManager.Instance.isInDialogueBox = true;
	}
	public void HideDialogue()
	{
		StartCoroutine(StopDisplay());
	}
	IEnumerator StopDisplay()
	{
		TextBox.text = "";
		DialogueBox.SetActive(false);
		yield return null;
		GameManager.Instance.isInDialogueBox = false;
	}
}
