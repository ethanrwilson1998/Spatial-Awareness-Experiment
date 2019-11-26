using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathUI : MonoBehaviour
{
    [SerializeField] private Path path;
    [SerializeField] private Slider slider;

    private void Start()
    {
        if (path == null)
            Destroy(this);

        slider.minValue = 0;
        slider.maxValue = path.GetNumPoints();

    }
    private void Update()
    {
        slider.value = path.GetIndex();
    }
}
