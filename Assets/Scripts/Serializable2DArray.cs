using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
public class Serializable2DArray : ISerializationCallbackReceiver
{
    public Cell[,] cells = new Cell[4, 4];

    [SerializeField]
    List<Package<Pair<int, Vector3>>> serializable;

    [System.Serializable]
    struct Package<TElement>
    {
        public int Index0;
        public int Index1;
        public TElement Element;
        public Package(int idx0, int idx1, TElement element)
        {
            Index0 = idx0;
            Index1 = idx1;
            Element = element;
        }
    }
    public void OnBeforeSerialize()
    {
        serializable = new List<Package<Pair<int, Vector3>>>();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                serializable.Add(new Package<Pair<int, Vector3>>(i, j, new Pair<int, Vector3>(cells[i, j] == null ? -1 : cells[i, j].Value, cells[i, j] == null ? Vector3.zero : cells[i, j].Position)));
            }
        }
    }

    public void OnAfterDeserialize()
    {
        cells = new Cell[4, 4];
        foreach (var package in serializable)
        {
            cells[package.Index0, package.Index1] = new Cell(package.Element.Left, package.Element.Right);
        }
    }
}
