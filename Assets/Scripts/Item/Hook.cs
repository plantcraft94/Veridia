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
	GameObject PlayerGameObject;
	float maxDistance;
	Rigidbody rb;
	bool isreturn = false;
	LineRenderer lr;
	
	private void Start()
	{
		PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
		PM = PlayerGameObject.GetComponent<PlayerMovement>();
		IC = PlayerGameObject.GetComponent<ItemsController>();
		maxDistance = IC.MaxDistance;
		rb = GetComponent<Rigidbody>();
		lr = transform.GetChild(0).GetComponent<LineRenderer>();
	}
	private void Update()
	{
		lr.SetPosition(0, transform.position);
		lr.SetPosition(1, PlayerGameObject.transform.position);
		if(Vector3.Distance(PlayerGameObject.transform.position, transform.position) <= 0.5f)
		{
			IC.HookShoted = false;
			Destroy(gameObject);
		}	
		RaycastHit hit;
		if (Physics.BoxCast(transform.position,new Vector3(0.3f,0.25f,0.25f), transform.forward, out hit,transform.rotation, 0.5f) && !collide)
		{
			
			Debug.Log(Vector3.Distance(PlayerGameObject.transform.position, hit.transform.position));
			if (hit.collider.CompareTag("Hookable"))
			{
				collide = true;
				PM.StartHook(transform.position,transform.gameObject);
			}
			else if(hit.collider.CompareTag("Hook"))
			{
				return;
			}
			else
			{
				HookReturn();
			}
		}
		else if(Vector3.Distance(PlayerGameObject.transform.position,transform.position) > maxDistance && !collide)
		{
			HookReturn();
		}
	}
	void HookReturn()
	{
		if(!isreturn)
		{
			isreturn = true;
			rb.linearVelocity = -rb.linearVelocity;	
		}
	}
}
