using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // moves relative to the camera
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float speed;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        input = input.normalized * speed;

        Vector3 movement = cameraTransform.TransformDirection(input) * Time.deltaTime;
        movement = Vector3.Scale(movement, new Vector3(1, 0, 1));

        transform.position += movement;
    }

    public float GetSpeed()
    {
        return speed;
    }
}
