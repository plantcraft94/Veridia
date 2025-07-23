using UnityEngine;

public class PlayerHM : MonoBehaviour
{
	float Health;
	float MaxHealth;
	float Magic = 0;
	[SerializeField] float MagicGenAmountPerSecond;

	private void Start()
	{
		
	}
	private void Update()
	{
		if(Magic > 100)
		{
			Magic = 100;
		}
	}
	private void FixedUpdate()
	{
		Magic += Time.deltaTime * MagicGenAmountPerSecond;
	}
	public void AddHealth(float amount)
	{
		Health += amount;
		if(Health > MaxHealth)
		{
			Health = MaxHealth;
		}
	}
	public void AddMaxHealth(float amount)
	{
		Health += amount;
	}
	public void DamageHealth(float amount)
	{
		Health -= amount;
		if(Health <= 0)
		{
			Debug.Log("Ded Ded");
		}
	}
	public void AddMagic(float amount)
	{
		Magic += amount;
	}
	public void MinusMagic(float amount)
	{
		Magic -= amount;
		if(Magic <= 0)
		{
			Magic = 0;
		}
	}
}
