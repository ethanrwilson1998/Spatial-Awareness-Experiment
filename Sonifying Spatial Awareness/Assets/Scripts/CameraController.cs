using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CameraController : MonoBehaviour
{
    // if VR is enabled, this script shouldn't run

    [SerializeField] private float xSensitivity;
    [SerializeField] private float ySensitivity;

    private void Update()
    {
        //if (!XRDevice.isPresent)
        {
            // camera rotations

            float x = xSensitivity * Input.GetAxis("Mouse X");
            float y = ySensitivity * Input.GetAxis("Mouse Y");

            transform.Rotate(new Vector3(y, x, 0) * Time.deltaTime);
            transform.eulerAngles = Vector3.Scale(transform.eulerAngles, new Vector3(1, 1, 0));
        }
    }
}
