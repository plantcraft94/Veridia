using UnityEngine;
#if UNITY_EDITOR
using Physics = Nomnom.RaycastVisualization.VisualPhysics;
#else
using Physics = UnityEngine.Physics;
#endif

public class ThrowableObject : MonoBehaviour
{
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	public int Grabbed = 0;
	Transform Player;
	Collider coll;
	Rigidbody rb;
	PlayerMovement PM;
	LayerMask GroundLayer;

	public float ThrowForce;
	void Start()
	{
		Player = GameObject.FindGameObjectWithTag("Player").transform;
		coll = GetComponent<Collider>();
		PM = Player.GetComponent<PlayerMovement>();
		rb = GetComponent<Rigidbody>();
		GroundLayer = LayerMask.NameToLayer("Ground");
	}

	// Update is called once per frame
	void Update()
	{

		if (Grabbed == 1)
		{
			transform.position = new Vector3(Player.position.x, Player.position.y + 1, Player.position.z);
			coll.enabled = false;
		}
		else if (Grabbed == 2)
		{
			rb.isKinematic = false;
			rb.AddForce(new Vector3(PM.PlayerFacingDirection.x, 0.5f, PM.PlayerFacingDirection.y) * ThrowForce, ForceMode.Impulse);
			Grabbed = 0;
			coll.enabled = true;
		}
	}
	private void FixedUpdate()
	{
		if (Physics.BoxCast(transform.position,new Vector3 (0.4f,0.1f,0.4f), Vector3.down, Quaternion.identity, 0.52f,1 << GroundLayer))
		{
			rb.isKinematic = true;
		}
	}
}
