using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeperatedTextCreator : MonoBehaviour
{
#pragma warning disable CS0649 // Field 'SeperatedTextCreator.parent' is never assigned to, and will always have its default value null
    [SerializeField] Transform parent;
#pragma warning restore CS0649 // Field 'SeperatedTextCreator.parent' is never assigned to, and will always have its default value null
#pragma warning disable CS0649 // Field 'SeperatedTextCreator.charObj' is never assigned to, and will always have its default value null
    [SerializeField] GameObject charObj;
#pragma warning restore CS0649 // Field 'SeperatedTextCreator.charObj' is never assigned to, and will always have its default value null
#pragma warning disable CS0649 // Field 'SeperatedTextCreator.textToGenerate' is never assigned to, and will always have its default value null
    [SerializeField] string textToGenerate;
#pragma warning restore CS0649 // Field 'SeperatedTextCreator.textToGenerate' is never assigned to, and will always have its default value null

    [SerializeField] float initialStartDelay = 1;
    [SerializeField] float delayDelta = 0.25f;
    [SerializeField] Vector3 axis;
#pragma warning disable CS0414 // The field 'SeperatedTextCreator.speed' is assigned but its value is never used
    [SerializeField] float speed = 10;
#pragma warning restore CS0414 // The field 'SeperatedTextCreator.speed' is assigned but its value is never used

    [Button("Generate")]
    void Generate()
    {
        for(int i = 0;i< textToGenerate.Length; i++)
        {
            GameObject obj = Instantiate(charObj, parent);
            obj.SetActive(true);
            obj.name = textToGenerate[i].ToString();
            obj.GetComponent<TMPro.TMP_Text>().text = textToGenerate[i].ToString();
            obj.GetComponent<MenuLetter>().Init(initialStartDelay + i * delayDelta);
        }
    }
}
