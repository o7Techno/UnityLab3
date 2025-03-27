using UnityEngine.UIElements.Experimental;

public class ObservableInt
{
    int _value = 0;
    public event System.Action<int> OnValueChanged;

    public int Value
    {
        get { return _value; }
        set
        {
            if (value != _value)
            {
                _value = value;
                OnValueChanged?.Invoke(value);
            }
        }
    }
    
}
