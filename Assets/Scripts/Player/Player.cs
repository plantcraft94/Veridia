using UnityEngine;

public class Player : MonoBehaviour
{
	public static Player Instance{ get; set; }
	Animator anim;
	public PlayerMovement PM;
	public PlayerInteract PI;
	public PlayerResource PR;
	public PlayerCommonInventory PCI;
	public ItemsController IC;
	private void Awake()
	{
		
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		anim = transform.GetChild(0).GetComponent<Animator>();
		PM = GetComponent<PlayerMovement>();
		PI = GetComponent<PlayerInteract>();
		PR = GetComponent<PlayerResource>();
		PCI = GetComponent<PlayerCommonInventory>();
		IC = GetComponent<ItemsController>();
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created

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
