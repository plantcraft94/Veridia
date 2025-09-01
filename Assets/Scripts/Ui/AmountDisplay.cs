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
		if (sourceComponent == null || string.IsNullOrEmpty(varAmount))
            return;

        var field = sourceComponent.GetType().GetField(varAmount);
        if (field != null)
        {
            var value = field.GetValue(sourceComponent);
            text.text = value.ToString();
        }
	}
}
