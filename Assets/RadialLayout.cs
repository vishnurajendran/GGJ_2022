using UnityEngine;
using UnityEngine.UI;

public class RadialLayout : LayoutGroup
{
    [Range(0f, 3f)]
    public float spreadMultiplier = 1f;
    public float fDistance;
    [Range(-180f, 180f)]
    public float MinAngle, MaxAngle;
    protected override void OnEnable() { base.OnEnable(); CalculateRadial(); }
    public override void SetLayoutHorizontal()
    {
    }
    public override void SetLayoutVertical()
    {
    }
    public override void CalculateLayoutInputVertical()
    {
        CalculateRadial();
    }
    public override void CalculateLayoutInputHorizontal()
    {
        CalculateRadial();
    }
#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        CalculateRadial();
    }
#endif
    async void CalculateRadial()
    {
        int activeChildCount = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            if (child && child.gameObject.activeSelf)
            {
                activeChildCount++;
            }
        }

        m_Tracker.Clear();
        if (activeChildCount == 0)
            return;

        float fOffsetAngle = 0;
        if (activeChildCount > 1)
        {
            fOffsetAngle = ((MaxAngle - MinAngle)) / (activeChildCount - 1);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform child = (RectTransform)transform.GetChild(i);
            if (child != null && child.gameObject.activeSelf)
            {
                child.localPosition = new Vector3(fDistance * Mathf.Sin(Mathf.Deg2Rad * (MinAngle + (i * fOffsetAngle))), fDistance * Mathf.Cos(Mathf.Deg2Rad * (MinAngle + (i * fOffsetAngle))), 0);
                child.localEulerAngles = new Vector3(0, 0, -spreadMultiplier * (MinAngle + (i * fOffsetAngle)));
            }
        }
    }
}