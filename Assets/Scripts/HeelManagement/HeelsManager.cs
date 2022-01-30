using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using DG.Tweening;

public class HeelsManager : MonoBehaviour
{
    public static event EventHandler<HeelsLengthChangedEventArgs> HeelsLengthChanged;

    public List<ConstraintedHeel> heels { get; set; }

    public Transform ground { get; set; }

    public float lengthPerSizing { get; set; }
    public float sizingDuration { get; set; }

    private const float defaultLengthPerSizing = .1f;

    public Bounds bounds { get { return heels.First().transform.GetComponent<Renderer>().bounds; } }
    public Vector3 heelExtents { get { return bounds.extents; } }
    public Vector3 heelSize { get { return bounds.size; } }

    private void Awake()
    {
        EventManager.HeelsCollision += OnHeelsCollide;
    }

    private void Start()
    {
        heels.ForEach((heel) => heel.positionStrategy.SetPivot(y: -heelExtents.y));
    }

    private void OnHeelsCollide(object sender, HeelsCollideEventArgs args)
    {
        float length = lengthPerSizing;
        HeelsLengthChangeType type = HeelsLengthChangeType.INCREASE;

        if (args.collideType.Equals(HeelsCollideType.OBSTACLE))
        {
            type = HeelsLengthChangeType.DECREASE;
            length *= -1;
        }

        SetHeelsHeight(length);
        HeelsLengthChanged?.Invoke(this, new HeelsLengthChangedEventArgs(type, bounds));
    }

    private void SetHeelsHeight(float length, int times = 1)
    {
        heels.ForEach((heel) =>
        {
            float stableLength = length.Equals(0) ? defaultLengthPerSizing : length;
            stableLength *= times;
            float endValue = heel.transform.localScale.y + stableLength;

            float pivot = endValue > heel.transform.localScale.y ? -heelExtents.y : heelExtents.y;
            heel.transform.DOScaleY(endValue, sizingDuration)
                .OnComplete(() => heel.positionStrategy.SetPivot(y: pivot));
        });
    }
}
