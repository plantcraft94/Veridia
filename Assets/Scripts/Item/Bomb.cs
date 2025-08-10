
using UnityEngine;

public class Bomb : MonoBehaviour
{
	[SerializeField] float damage;
	public float ExplodeTimer;
	float currentTimer = 0;
	[SerializeField] GameObject ExplodeEffect;
	SpriteRenderer sr;
	public float ColorchangeSpeed;

	private void Start()
	{
		sr = GetComponentInChildren<SpriteRenderer>();
	}
	private void FixedUpdate()
	{
		currentTimer += Time.fixedDeltaTime;
		if (currentTimer >= ExplodeTimer - 1.25f)
		{
			float t = Mathf.PingPong(Time.time * ColorchangeSpeed, 1f);
			sr.color = Color.Lerp(Color.white, new Color(1.000f, 0.490f, 0.490f, 1.000f)
			, t);
		}
		if (currentTimer >= ExplodeTimer)
		{
			Instantiate(ExplodeEffect, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
