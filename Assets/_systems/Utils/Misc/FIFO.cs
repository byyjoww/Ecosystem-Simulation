using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FIFO<T>
{
    int size;
    List<T> list = new List<T>();

    public void Resize(int size, T defaultValue)
    {
        this.size = size;
        while (list.Count < this.size)
        {
            list.Add(defaultValue);
        }
        while (list.Count > this.size)
        {
            list.RemoveAt(0);
        }
    }

    public T EnqueueDequeue(T e)
    {
        list.Add(e);
        e = list[0];
        list.RemoveAt(0);
        return e;
    }

    public T Peek(int index)
    {
        return list[list.Count - 1 - index];
    }
}
