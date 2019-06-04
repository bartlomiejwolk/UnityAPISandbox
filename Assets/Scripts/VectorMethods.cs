using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649

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

    /// <summary>
    /// Used to pass data from Update() to OnGUI().
    /// OnGUI() must know the order in which the data was added to the list.
    /// </summary>
    private List<object> m_OnGUIData = new List<object>();

    void Update()
    {
        switch (m_Method)
        {
            case Method.Vector3_Angle:
                {
                    m_OnGUIData.Clear();

                    // Setup scene objects.
                    DisableAllSceneObjects();
                    m_ObjectA.gameObject.SetActive(true);
                    m_ObjectB.gameObject.SetActive(true);

                    float angle = Vector3.Angle(m_ObjectA.forward, m_ObjectB.forward);
                    m_OnGUIData.Add(angle);
                    break;
                }
            case Method.Vector3_Dot:
                {
                    m_OnGUIData.Clear();

                    // Setup scene objects.
                    DisableAllSceneObjects();
                    m_ObjectA.gameObject.SetActive(true);
                    m_ObjectB.gameObject.SetActive(true);

                    float dot = Vector3.Dot(m_ObjectA.forward, m_ObjectB.forward);
                    m_OnGUIData.Add(dot);
                    break;
                }
            case Method.Vector3_Cross:
                {
                    m_OnGUIData.Clear();

                    // Setup scene objects.
                    DisableAllSceneObjects();
                    m_ObjectA.gameObject.SetActive(true);
                    m_ObjectB.gameObject.SetActive(true);
                    m_CrossProductPointer.gameObject.SetActive(true);

                    Vector3 cross = Vector3.Cross(m_ObjectA.forward, m_ObjectB.forward);
                    m_OnGUIData.Add(cross);
                    Quaternion newRot = new Quaternion();
                    newRot.SetLookRotation(cross, Vector3.up);
                    m_CrossProductPointer.rotation = newRot;
                    break;
                }
        }
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();

        switch (m_Method)
        {
            case Method.Vector3_Angle:
                {
                    GUILayout.Label("Vector3.Angle()");
                    GUILayout.Label(string.Format("Angle: {0}", m_OnGUIData[0]));
                    break;
                }
            case Method.Vector3_Dot:
                {
                    GUILayout.Label("Vector3.Dot()");
                    GUILayout.Label(string.Format("Dot Product: {0}", m_OnGUIData[0]));
                    break;
                }
            case Method.Vector3_Cross:
                {
                    GUILayout.Label("Vector3.Cross()");
                    GUILayout.Label(string.Format("Cross Product: {0}", m_OnGUIData[0]));
                    break;
                }
        }

        GUILayout.EndVertical();
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

    Vector3_Dot,

    Vector3_Cross,

}
