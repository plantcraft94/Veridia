using TMPro;
using UnityEngine;

public class AmountDisplay : MonoBehaviour
{
	public Component sourceComponent;
	public string varAmount;
	TMP_Text text;
	private void Start()
	{
		text = GetComponent<TMP_Text>();
	}
	// Update is called once per frame
	void Update()
	{
		text.text = $"{Player.Instance.PR.GetType().GetField(varAmount).GetValue(Player.Instance.PR)}";
	}
}
