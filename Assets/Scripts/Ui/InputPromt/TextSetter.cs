using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TextSetter : MonoBehaviour
{
	[TextArea(2, 3)]
	[SerializeField] string message = "BUTTONPROMPT to do something.";
	[Header("Setup for sprites")]
	[SerializeField] ListOfTMPSpriteAssets listOfTMPSpriteAssets;
	[SerializeField] private InputActionReference Iaction;
	TMP_Text textBox;
	private void Awake()
	{
		textBox = GetComponent<TMP_Text>();
	}
	private void Start()
	{
		InputPromptManager.Instance.OnControlChange.AddListener(SetText);
		SetText();
	}
	void SetText()
	{
		if((int)InputPromptManager.Instance.controlType > listOfTMPSpriteAssets.SpriteAssets.Count-1)
		{
			Debug.LogError($"Missing Sprite Asset for {InputPromptManager.Instance.controlType}");
		}
		textBox.text = CompleteText.ReadAndReplaceBinding(message, Iaction.action.bindings[(int)InputPromptManager.Instance.controlType], listOfTMPSpriteAssets.SpriteAssets[(int)InputPromptManager.Instance.controlType]);
	}
}
