using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
	ArrowElement type;
	public float Damage;
    private void Start()
	{
		Destroy(gameObject, 10f);// failsafe
	}
	public void ArrowType(ArrowElement element)
	{
		type = element;
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			Destroy(gameObject);
		}
		else if (collision.gameObject.CompareTag("Player"))
		{
			switch (type)
			{
				case ArrowElement.Normal:
					Player.Instance.PR.DamageHealth(Damage);
					break;
				case ArrowElement.Fire:
					Debug.Log("Enemy hit + burn" + " " + Damage);
					break;
				case ArrowElement.Ice:
					Debug.Log("Enemy hit + freeze" + " " + Damage);
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
