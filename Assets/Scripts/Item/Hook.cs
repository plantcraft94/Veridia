using UnityEngine;
#if UNITY_EDITOR
using Physics = Nomnom.RaycastVisualization.VisualPhysics;
#else
using Physics = UnityEngine.Physics;
#endif

public class Hook : MonoBehaviour
{
	PlayerMovement PM;
	ItemsController IC;
	bool collide = false;
	private void Start()
	{
		GameObject PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
		PM = PlayerGameObject.GetComponent<PlayerMovement>();
		IC = PlayerGameObject.GetComponent<ItemsController>();
	}
	private void Update()
	{
		RaycastHit hit;
		if(Physics.Raycast(transform.position,transform.forward,out hit,0.5f) && !collide)
		{
			if(hit.collider.CompareTag("Hookable"))
			{
				collide = true;
				PM.StartHook(transform.position);
			}
			else
			{
				IC.HookShoted = false;
				Destroy(transform.gameObject);
			}
		}
	}
}
