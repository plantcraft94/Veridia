using UnityEngine;

public class Player : MonoBehaviour
{
	Animator anim;
	PlayerMovement PM;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		anim = transform.GetChild(0).GetComponent<Animator>();
		PM = GetComponent<PlayerMovement>();
	}

	// Update is called once per frame
	void Update()
	{
		Animation();
		// if(Input.GetKeyDown(KeyCode.D))
		// {
		// 	PM.SpeedMul(0.2f);
		// }
		// if(Input.GetKeyUp(KeyCode.D))
		// {
		// 	PM.RemoveSpeedMul(0.2f);
		// }
	}
	
	void Animation()
	{
		if(PM.PlayerFacingDirection != Vector2.zero)
		{
			anim.SetFloat("MoveX", PM.PlayerFacingDirection.x);
			anim.SetFloat("MoveY", PM.PlayerFacingDirection.y);
		}
		anim.SetBool("IsMoving", PM.PlayerFacingDirection != Vector2.zero);
		// TEST

		
		if(Input.GetKeyDown(KeyCode.X))
		{
			anim.SetTrigger("IsAttacking");
		}
	}
}
