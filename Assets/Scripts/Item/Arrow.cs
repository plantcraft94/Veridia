using UnityEngine;
public class Arrow : MonoBehaviour
{
	float DamageMul;
	bool MagicArrow;
	ArrowElement type;
	private void Start()
	{
		Destroy(gameObject, 10f);// failsafe
	}
	private void Update()
	{
		if (MagicArrow)
		{
			DamageMul = 0.25f;
		}
	}
	public void ArrowType(float damageMul, bool isMagicArrow, ArrowElement element)
	{
		DamageMul = damageMul;
		MagicArrow = isMagicArrow;
		type = element;
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			Destroy(gameObject);
		}
		else if (collision.gameObject.CompareTag("Enemy"))
		{
			switch (type)
			{
				case ArrowElement.Normal:
					Debug.Log("Enemy hit" + " " + DamageMul);
					break;
				case ArrowElement.Fire:
					Debug.Log("Enemy hit + burn" + " " + DamageMul);
					break;
				case ArrowElement.Ice:
					Debug.Log("Enemy hit + freeze" + " " + DamageMul);
					break;
				default:
					break;
			}
			Destroy(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
