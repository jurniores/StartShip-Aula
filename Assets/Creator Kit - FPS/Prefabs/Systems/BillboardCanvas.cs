using System.Collections;
using System.Collections.Generic;
using Omni.Core;
using UnityEngine;
using UnityEngine.UI;

public class BillboardCanvas : MonoBehaviour
{
    Transform mainCamera;
    [SerializeField]
    private Slider slider;

    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (mainCamera) transform.LookAt(transform.position + mainCamera.rotation * Vector3.forward, mainCamera.rotation * Vector3.up);
    }

    public void HPCanvas(float hp, float hpTotal)
    {
        slider.value = hp / hpTotal;
    }

}
