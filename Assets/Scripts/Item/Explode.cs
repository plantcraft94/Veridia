using UnityEngine;

public class Explode : MonoBehaviour
{
	private void Start()
	{
		GameManager.Instance.ShakeCamera();
		
		Destroy(gameObject, 2f);
	}
}
