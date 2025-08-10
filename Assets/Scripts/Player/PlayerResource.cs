using UnityEngine;

public class PlayerResource : MonoBehaviour
{
	public float Health{ get; private set; }
	public float MaxHealth{ get; private set; }
	public float Magic { get; private set; }
	[SerializeField] float MagicGenAmountPerSecond;

	private void Start()
	{
		MaxHealth = 100f;
		Health = MaxHealth;
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
