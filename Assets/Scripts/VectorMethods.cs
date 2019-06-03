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

    [SerializeField]
    private Transform m_CrossProductPointer;

    void Update()
    {
        // Vector3.Angle()
        {
            float angle = Vector3.Angle(m_ObjectA.forward, m_ObjectB.forward);
            Debug.Log(string.Format("Angle between A and B: {0}", angle));
        }

        // Vector3.Cross()
        {
            Vector3 cross = Vector3.Cross(m_ObjectA.forward, m_ObjectB.forward);
            Debug.Log(string.Format("Cross product between A and B: {0}", cross));
            Quaternion newRot = new Quaternion();
            newRot.SetLookRotation(cross, Vector3.up);
            m_CrossProductPointer.rotation = newRot;
        }
    }
}
