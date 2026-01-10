using System;
using System.Collections;
using UnityEngine;

public class SmoothSliderBar : SliderBar
{
    [SerializeField] private float _changeSpeed = 0.1f;
    [SerializeField] private float _smoothStep = 0.01f;

    private WaitForSeconds _smoothStepDelay;
    private Coroutine _smoothChange;

    public override void ChangeValue()
    {
        float targetValue = Convert.ToSingle(Health.Current) / Health.Max;

        if (_smoothChange != null)
            StopCoroutine(_smoothChange);

        _smoothChange = StartCoroutine(SmoothChangeValue(targetValue));
    }

    protected override void Initialize()
    {
        base.Initialize();
        _smoothStepDelay = new WaitForSeconds(_changeSpeed);
    }

    private IEnumerator SmoothChangeValue(float targetValue)
    {
        float currentValue = Slider.value;

        while (Mathf.Approximately(currentValue, targetValue) == false)
        {
            currentValue = Mathf.MoveTowards(currentValue, targetValue, _smoothStep);
            Slider.SetValueWithoutNotify(currentValue);

            yield return _smoothStepDelay;
        }

        Slider.SetValueWithoutNotify(targetValue);
    }
}

