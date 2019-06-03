using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class VectorMethods : MonoBehaviour
{
    [Header("Angle")]
    [SerializeField]
    private Transform m_ObjectA;

    [SerializeField]
    private Transform m_ObjectB;

    void Update()
    {
        float angle = Vector3.Angle(m_ObjectA.forward, m_ObjectB.forward);
        Debug.Log(string.Format("Angle between A and B: {0}", angle));
    }
}
