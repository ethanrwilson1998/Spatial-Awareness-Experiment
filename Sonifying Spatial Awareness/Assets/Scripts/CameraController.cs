using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CameraController : MonoBehaviour
{
    // if VR is enabled, we can't move the camera here

    [SerializeField] private bool blinded;

    [SerializeField] private float xSensitivity;
    [SerializeField] private float ySensitivity;

    private void Awake()
    {
        if (blinded)
        {
            GetComponent<Camera>().enabled = false;
        }
    }

    private void Update()
    {
        // don't allow vr device to offset the camera
        transform.localPosition = new Vector3(0, 1.5f, 0);

        if (!XRDevice.isPresent)
        {
            // camera rotations

            float x = xSensitivity * Input.GetAxis("Mouse X");
            float y = ySensitivity * Input.GetAxis("Mouse Y");

            transform.Rotate(new Vector3(y, x, 0) * Time.deltaTime);
            transform.eulerAngles = Vector3.Scale(transform.eulerAngles, new Vector3(1, 1, 0));
        }
        else
        {
            Debug.Log(XRDevice.model);
        }
    }
}
