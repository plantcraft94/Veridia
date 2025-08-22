using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCam == null) return;
        Vector3 camForward = mainCam.transform.forward;
        camForward.y = 0;
        transform.forward = camForward;
        transform.LookAt(transform.position + mainCam.transform.rotation * Vector3.forward,
                 mainCam.transform.rotation * Vector3.up);
    }
}
