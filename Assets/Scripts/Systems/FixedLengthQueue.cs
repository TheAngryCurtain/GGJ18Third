using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FixedLengthLinkedList
/// - Tossed this container class quickly so that I could have a fixed length LinkedList for my Calculator Screen without tossing that logic into the screen.
/// - Only exposes what I need to Populate my UI.
/// </summary>
/// <typeparam name="T"></typeparam>
public class FixedLengthLinkedList<T> where T : class 
{
    private int m_Length;
    private LinkedList<T> m_InternalList;
    
    public int Length { get { return m_Length; } }
    public int Count
    {
        get
        {
            if (m_InternalList != null)
            {
                return m_InternalList.Count;
            }

            Debug.AssertFormat(true, "FixedLengthQueue.Count - Failed to return the Count of this FixedLengthQueue of type: {0} because the internal queue is null.", typeof(T));

            return -1;
        }
    }

    public FixedLengthLinkedList(int length)
    {
        m_Length = length;
        m_InternalList = new LinkedList<T>();
    }

    public T Push_Front(T newObject)
    {
        m_InternalList.AddFirst(newObject);
        if(m_InternalList.Count > m_Length)
        {
            T retVal = m_InternalList.Last.Value;
            m_InternalList.RemoveLast();
            return retVal;
        }

        return null;
    }

    public T Front()
    {
        return m_InternalList.First.Value;
    }
}