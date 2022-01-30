using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event EventHandler<HeelsCollideEventArgs> HeelsCollision;
    public static event EventHandler<HeelsLengthChangedEventArgs> HeelsLengthChanged;

    private void Start()
    {
        Collision.HeelsCollide += HeelsCollision;
        HeelsManager.HeelsLengthChanged += HeelsLengthChanged;
    }
}

public class HeelsLengthChangedEventArgs : EventArgs
{
    public HeelsLengthChangeType changeType { get; private set; }
    public Bounds bounds { get; private set; }

    public HeelsLengthChangedEventArgs(HeelsLengthChangeType changeType, Bounds bounds)
    {
        this.changeType = changeType;
        this.bounds = bounds;
    }
}

public enum HeelsLengthChangeType
{
    INCREASE,
    DECREASE
}
