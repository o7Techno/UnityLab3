using System;
using Unity.VisualScripting;
using UnityEngine;

public class Cell
{
    public event Action OnValueChanged;
    public event Action OnPositionChanged;

    int _value;
    Vector3 _position;
    bool _merged;

    public int Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            OnValueChanged?.Invoke();
        }
    }

    public Vector3 Position
    {
        get
        {
            return _position;
        }
        set
        {
            _position = value;
            OnPositionChanged?.Invoke();
        }
    }

    public bool Merged
    {
        get { return _merged; }
        set { _merged = value; }
    }

    public Cell(int value, Vector3 position)
    {
        _value = value;
        _position = position;
    }
}
