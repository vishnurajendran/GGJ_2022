using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueUpdater : MonoBehaviour
{
#pragma warning disable CS0649 // Field 'ValueUpdater.propertyName' is never assigned to, and will always have its default value null
    [SerializeField] string propertyName;
#pragma warning restore CS0649 // Field 'ValueUpdater.propertyName' is never assigned to, and will always have its default value null
    [Range(0f,1f)]
    [SerializeField]float value = 1f;

    Material material;
    private void Start()
    {
        material = GetComponent<Image>().material;
    }

    private void FixedUpdate()
    {
        material.SetFloat(propertyName, value);
    }
}
