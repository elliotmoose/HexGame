using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Metric
{
    public string title;
    public float current;
    public float max;
    public MetricDisplayType metricDisplayType;

    public Metric(string title, float current, float max, MetricDisplayType metricDisplayType)
    {
        this.title = title;
        this.current = current;
        this.max = max;
        this.metricDisplayType = metricDisplayType;
    }
}