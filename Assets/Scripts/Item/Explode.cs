using UnityEngine;
using PrimeTween;
using Unity.Cinemachine;

public class Explode : MonoBehaviour
{
	GameObject cam;
	CinemachineBasicMultiChannelPerlin amp;
	private void Start()
	{
		cam = GameObject.Find("CinemachineCamera").gameObject;
		amp = cam.GetComponent<CinemachineBasicMultiChannelPerlin>();
		amp.AmplitudeGain = 1f;
		Tween.Delay(duration: 0.5f, () => { amp.AmplitudeGain = 0f; });
		
		Destroy(gameObject, 2f);
	}
}
