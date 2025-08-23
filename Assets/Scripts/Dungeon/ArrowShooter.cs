using UnityEngine;
using PrimeTween;
using System.Collections;

public class ArrowShooter : MonoBehaviour
{
	[SerializeField] float CoolDown;
	[SerializeField] GameObject Arrow;
	[SerializeField] float ArrowSpeed;
	[SerializeField] Transform SpawnLocation;
	private void Start()
	{
		StartCoroutine(SpawnArrow());
	}
	IEnumerator SpawnArrow()
	{
		yield return new WaitForSeconds(CoolDown);
		GameObject NewArrow = Instantiate(Arrow, SpawnLocation.position, transform.rotation);
		NewArrow.GetComponent<EnemyArrow>().ArrowType(ArrowElement.Normal);
		NewArrow.GetComponent<Rigidbody>().linearVelocity = ArrowSpeed * transform.forward;
		StartCoroutine(SpawnArrow());
	}
}
