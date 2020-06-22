using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarMetricUI : MonoBehaviour
{
    public Text metricTitleText;
    public Text metricValueText;
    public Image progressImage;

    public void SetMetric(Metric metric) 
    {
        metricTitleText.text = $"{metric.title}";
        metricValueText.text = $"{metric.current}/{metric.max}";
        progressImage.fillAmount = Mathf.Clamp(metric.current/metric.max, 0, 1);        
    }
}
