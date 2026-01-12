using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextBar : Bar
{
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public override void Initialize(ICountable stat)
    {
        base.Initialize(stat);
        ChangeValue();
    }

    public override void ChangeValue()
    {
        float currentValue = Stat.Current;
        _text.text = currentValue.ToString();
    }
}
