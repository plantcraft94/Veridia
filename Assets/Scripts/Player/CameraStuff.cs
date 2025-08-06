using UnityEngine;
using PrimeTween;

public class CameraStuff : MonoBehaviour
{
	[SerializeField] private Transform player; // Better than FindGameObjectWithTag
	private PlayerMovement PM;
	private Vector3 currentTargetDirection;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		PM = player.GetComponent<PlayerMovement>();
		currentTargetDirection = new Vector3(PM.PlayerFacingDirection.x, 0, PM.PlayerFacingDirection.y);
	}

	private void LateUpdate()
	{
		Vector3 newDirection = new Vector3(PM.PlayerFacingDirection.x, 0, PM.PlayerFacingDirection.y);
		
		// Only tween when direction actually changes
		if (newDirection != currentTargetDirection)
		{
			// Cancel any existing tween and start new one
			Tween.LocalPosition(
				transform, 
				endValue: newDirection*1.5f, 
				duration: 1.5f, 
				ease: Ease.InOutSine
			);
			
			// Immediately update our reference
			currentTargetDirection = newDirection;
		}
	}
}
