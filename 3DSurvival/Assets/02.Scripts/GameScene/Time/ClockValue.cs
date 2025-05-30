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
            this.value = value;
            onValueChanged?.Invoke(this.value);
        }
    }

    public static implicit operator int(ClockValue c) => c.Value;
    public static implicit operator ClockValue(int i) => new ClockValue { Value = i };
}
