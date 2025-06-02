using System;
using UnityEngine;

[Serializable]
public class ClockValue
{
    [SerializeField] private int value;

    public event Action<int> onValueChanged;

    public int Value
    {
        get => value;
        set
        {
            if (this.value != value)
            {
                this.value = value;
                onValueChanged?.Invoke(this.value);
            }
        }
    }

    public ClockValue() { }

    public ClockValue(int initialValue)
    {
        value = initialValue;
    }

    public static implicit operator int(ClockValue c) => c?.Value ?? 0;

    public static implicit operator ClockValue(int i) => new ClockValue(i);
}
