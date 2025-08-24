using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
	[SerializeField] GameObject SwordSlashEffect;
	InputAction SlashAction;
	public bool isSlashing;
	public float SwordDamage;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Awake()
	{
		SlashAction = InputSystem.actions.FindAction("Attack");
		SwordSlashEffect.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		if(SlashAction.WasPressedThisFrame() && !isSlashing)
		{
			isSlashing = true;
			SwordSlashEffect.SetActive(true);
		}
	}
}
