using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class Description : MonoBehaviour
{
	TMP_Text itemName;
	TextSetter itemDescription;
	private void Awake()
	{
		itemName = transform.GetChild(0).GetComponent<TMP_Text>();
		itemDescription = transform.GetChild(1).GetComponent<TextSetter>();
	}
	public void ChangeDescription(string iName,string iDescription,bool hasCustomInput, InputActionReference newinput)
	{
		itemName.text = iName;
		if(!hasCustomInput)
		{
			itemDescription.ChangeMessage(iDescription);
		}
		else
		{
			itemDescription.ChangeMessageInput(iDescription,newinput);
		}
	}
}
