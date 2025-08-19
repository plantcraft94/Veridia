using UnityEngine;

public class BillBoard : MonoBehaviour
{
	GameObject cam;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{
		cam = Camera.main.gameObject;
	}

	// Update is called once per frame
	void Update()
	{
		transform.eulerAngles = new Vector3(cam.transform.eulerAngles.x, 0f, 0f);
	}
}
