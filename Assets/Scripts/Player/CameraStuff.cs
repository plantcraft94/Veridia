using UnityEngine;
using PrimeTween;
using UnityEngine.InputSystem;

public class CameraStuff : MonoBehaviour
{
	private Vector3 currentTargetDirection;
	InputAction MoveCamAction;
	Vector2 CamDir;

	private void Start()
	{
		currentTargetDirection = new Vector3(0, 0, 0);
		MoveCamAction = InputSystem.actions.FindAction("MoveCam");
	}
	private void Update()
	{
		CamDir = MoveCamAction.ReadValue<Vector2>();
	}

	private void LateUpdate()
	{
		Vector3 newDirection = new Vector3(CamDir.x, 0, CamDir.y);
		
		// Only tween when direction actually changes
		if (newDirection != currentTargetDirection)
		{
			// Cancel any existing tween and start new one
			Tween.LocalPosition(
				transform, 
				endValue: newDirection*3f, 
				duration: 0.5f, 
				ease: Ease.InOutSine
			);
			
			// Immediately update our reference
			currentTargetDirection = newDirection;
		}
	}
}
