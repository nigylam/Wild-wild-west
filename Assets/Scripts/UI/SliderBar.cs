using System;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Slider))]
public class SliderBar : Bar
{
    protected Slider Slider;

    public override void ChangeValue()
    {
        float currentValue = Convert.ToSingle(Stat.Current) / Stat.Max;
        Slider.SetValueWithoutNotify(currentValue);
    }

    public override void Initialize(ICountable stat)
    {
        base.Initialize(stat);
        Slider = GetComponent<Slider>();
        ChangeValue();
    }
}
