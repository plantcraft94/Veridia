using System.Collections;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;

public class PlayerResource : MonoBehaviour
{
	public float Health{ get; private set; }
	public float MaxHealth{ get; private set; }
	public float Magic { get; private set; }
	[SerializeField] float MagicGenAmountPerSecond;
	public int BombAmount;
	public int ArrowAmount;
	public int HealthPotAmount;
	public int MaxHealthPotAmount;
	public UnityEvent OnDeath;
	bool IsInIFrame = false;
	SpriteRenderer sr;
	Material mat;
	[SerializeField] float opacity;

	[SerializeField] TweenSettings<float> tweenSettings;

	private void Awake()
	{
		sr = GetComponentInChildren<SpriteRenderer>();
		mat = sr.material;
		mat.SetColor("_Color", new Color(1.000f, 0.200f, 0.000f, 1.000f));
	}

	private void Start()
	{
		MaxHealth = 100f;
		Health = MaxHealth;
		HealthPotAmount = MaxHealthPotAmount;
	}
	private void Update()
	{
		mat.SetFloat("_Opacity", opacity);
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
		Tween.Custom(tweenSettings, onValueChange: newVal => opacity = newVal);
		if(!IsInIFrame)
		{
			Health -= amount;
		}
		if(Health <= 0)
		{
			OnDeath.Invoke();
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
	public IEnumerator IFrame()
	{
		IsInIFrame = true;
		yield return new WaitForSeconds(0.5f);
		IsInIFrame = false;
	}
}
