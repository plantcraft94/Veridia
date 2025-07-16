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
	}
	
	void Animation()
	{
		if(PM.movement != Vector2.zero)
		{
			anim.SetFloat("MoveX", PM.movement.x);
			anim.SetFloat("MoveY", PM.movement.y);
		}
		anim.SetBool("IsMoving", PM.movement != Vector2.zero);
		// TEST

		
		if(Input.GetKeyDown(KeyCode.X))
		{
			Debug.Log("attack");
			anim.SetTrigger("IsAttacking");
		}
	}
}
