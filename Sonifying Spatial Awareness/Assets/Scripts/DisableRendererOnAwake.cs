using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableRendererOnAwake : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Renderer>().enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            bool value = !GetComponent<Renderer>().enabled;
            GetComponent<Renderer>().enabled = value;
        }
    }
}
