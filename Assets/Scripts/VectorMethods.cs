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

    [SerializeField]
    private Method m_Method;

    void Update()
    {
        switch (m_Method)
        {
            case Method.Vector3_Angle:
                {
                    DisableAllSceneObjects();
                    m_ObjectA.gameObject.SetActive(true);
                    m_ObjectB.gameObject.SetActive(true);

                    float angle = Vector3.Angle(m_ObjectA.forward, m_ObjectB.forward);
                    //Debug.Log(string.Format("Angle between A and B: {0}", angle));
                    break;
                }
            case Method.Vector3_Cross:
                {
                    DisableAllSceneObjects();
                    m_ObjectA.gameObject.SetActive(true);
                    m_ObjectB.gameObject.SetActive(true);
                    m_CrossProductPointer.gameObject.SetActive(true);

                    Vector3 cross = Vector3.Cross(m_ObjectA.forward, m_ObjectB.forward);
                    //Debug.Log(string.Format("Cross product between A and B: {0}", cross));
                    Quaternion newRot = new Quaternion();
                    newRot.SetLookRotation(cross, Vector3.up);
                    m_CrossProductPointer.rotation = newRot;
                    break;
                }
        }
    }

    private void DisableAllSceneObjects()
    {
        m_ObjectA.gameObject.SetActive(false);
        m_ObjectB.gameObject.SetActive(false);
        m_CrossProductPointer.gameObject.SetActive(false);
    }
}

public enum Method
{
    Vector3_Angle,

    Vector3_Cross
}
