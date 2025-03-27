using System;

[Serializable]
public class Pair<T, U>
{
    public T Left;
    public U Right;

    public Pair(T Left, U Right)
    {
        this.Left = Left;
        this.Right = Right;
    }
}
