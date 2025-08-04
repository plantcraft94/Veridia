using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using Physics = Nomnom.RaycastVisualization.VisualPhysics;
#else
using Physics = UnityEngine.Physics;
#endif

public class PushableObject : MonoBehaviour
{
	Rigidbody rb;
	bool IsMoving = false;

	RigidbodyConstraints lockConstraints;

	RigidbodyConstraints unLockConstraints;
	private void Start()
	{
		rb = GetComponent<Rigidbody>();
		lockConstraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
		unLockConstraints = RigidbodyConstraints.FreezeRotation;
		rb.constraints = lockConstraints;
	}

	public void startMove(Vector3 target, float speed, Vector3 dir)
	{
		if (!IsMoving && !Physics.Raycast(transform.position, dir, 1f))
		{
			StartCoroutine(Move(target, speed));
		}
	}
	IEnumerator Move(Vector3 target,float speed)
	{
		Debug.Log(target);
		rb.useGravity = false;
		rb.constraints = unLockConstraints;
		// Continue until very close to target
		while (Vector3.Distance(rb.position, target) != 0)
		{
			// Move towards target at constant speed
			Vector3 newPosition = Vector3.MoveTowards(
				rb.position,
				target,
				(speed + 1f) * Time.deltaTime
			);

			rb.MovePosition(newPosition);

			yield return null; // Wait for next frame
		}

		// Ensure final position is exact
		transform.position = target;
		UnityEngine.Physics.SyncTransforms();
		rb.constraints = lockConstraints;
		rb.useGravity = true;
		IsMoving = false;
	}
}
