using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

#pragma warning disable 0649

[ExecuteInEditMode]
public class VectorMethods : MonoBehaviour
{
    [SerializeField]
    private Method m_Method;

    [Header("Angle")]
    [SerializeField]
    private Transform m_AircraftTRS;

    [SerializeField]
    private Transform m_RunwayTRS;

    [SerializeField]
    private Transform m_CrossProductPointer;

    [SerializeField]
    private LineRenderer m_AircraftLineRenderer;

    [SerializeField]
    private LineRenderer m_RunwayLineRenderer;

    [SerializeField]
    private float m_LineRendererLenght = 10;

    [SerializeField]
    private float m_LineRendererWidth = .2f;

    /// <summary>
    /// Used to pass data from Update() to OnGUI().
    /// OnGUI() must know the order in which the data was added to the list.
    /// </summary>
    private List<object> m_OnGUIData = new List<object>();

    private void Awake()
    {
        Assert.IsNotNull(m_AircraftTRS);
        Assert.IsNotNull(m_RunwayTRS);
        Assert.IsNotNull(m_CrossProductPointer);
        Assert.IsNotNull(m_AircraftLineRenderer);
        Assert.IsNotNull(m_RunwayLineRenderer);
    }

    private void Start()
    {
        // Init Line Renderers.
        m_AircraftLineRenderer.positionCount = 2;
        m_RunwayLineRenderer.positionCount = 2;
    }

    void Update()
    {
        if (!AllRefsSet())
        {
            return;
        }

        HandleMethodType();
        UpdateLineRenderers();
    }

    private bool AllRefsSet()
    {
        if (m_AircraftTRS && m_RunwayTRS && m_CrossProductPointer && m_AircraftLineRenderer && m_RunwayLineRenderer)
        {
            return true;
        }

        return false;
    }

    private void UpdateLineRenderers()
    {
        // Aircraft
        m_AircraftLineRenderer.startWidth = m_LineRendererWidth;
        m_AircraftLineRenderer.endWidth = m_LineRendererWidth;
        m_AircraftLineRenderer.SetPosition(0, m_AircraftTRS.position);
        m_AircraftLineRenderer.SetPosition(1, m_AircraftTRS.position + m_AircraftTRS.forward * m_LineRendererLenght);

        // Runway
        m_RunwayLineRenderer.startWidth = m_LineRendererWidth;
        m_RunwayLineRenderer.endWidth = m_LineRendererWidth;
        m_RunwayLineRenderer.SetPosition(0, m_RunwayTRS.position);
        m_RunwayLineRenderer.SetPosition(1, m_RunwayTRS.position + m_RunwayTRS.forward * m_LineRendererLenght);
    }

    private void OnGUI()
    {
        if (!AllRefsSet())
        {
            return;
        }

        GUILayout.BeginVertical();

        switch (m_Method)
        {
            case Method.Vector3_Angle:
                {
                    GUILayout.Label("Vector3.Angle()");
                    GUILayout.Label(string.Format("Angle: {0}", m_OnGUIData[0]?.ToString()));
                    break;
                }
            case Method.Vector3_Dot:
                {
                    GUILayout.Label("Vector3.Dot()");
                    GUILayout.Label(string.Format("Dot Product: {0}", m_OnGUIData[0]?.ToString()));
                    break;
                }
            case Method.Vector3_Cross:
                {
                    GUILayout.Label("Vector3.Cross()");
                    GUILayout.Label(string.Format("Cross Product: {0}", m_OnGUIData[0]?.ToString()));
                    break;
                }
        }

        GUILayout.EndVertical();
    }

    private void HandleMethodType()
    {
        switch (m_Method)
        {
            case Method.Vector3_Angle:
                {
                    m_OnGUIData.Clear();

                    // Setup scene objects.
                    DisableAllSceneObjects();
                    m_AircraftTRS.gameObject.SetActive(true);
                    m_RunwayTRS.gameObject.SetActive(true);

                    float angle = Vector3.Angle(m_AircraftTRS.forward, m_RunwayTRS.forward);
                    m_OnGUIData.Add(angle);
                    break;
                }
            case Method.Vector3_Dot:
                {
                    m_OnGUIData.Clear();

                    // Setup scene objects.
                    DisableAllSceneObjects();
                    m_AircraftTRS.gameObject.SetActive(true);
                    m_RunwayTRS.gameObject.SetActive(true);

                    float dot = Vector3.Dot(m_AircraftTRS.forward, m_RunwayTRS.forward);
                    m_OnGUIData.Add(dot);
                    break;
                }
            case Method.Vector3_Cross:
                {
                    m_OnGUIData.Clear();

                    // Setup scene objects.
                    DisableAllSceneObjects();
                    m_AircraftTRS.gameObject.SetActive(true);
                    m_RunwayTRS.gameObject.SetActive(true);
                    m_CrossProductPointer.gameObject.SetActive(true);

                    Vector3 cross = Vector3.Cross(m_AircraftTRS.forward, m_RunwayTRS.forward);
                    m_OnGUIData.Add(cross);
                    Quaternion newRot = new Quaternion();
                    newRot.SetLookRotation(cross, Vector3.up);
                    m_CrossProductPointer.rotation = newRot;
                    break;
                }
        }
    }

    private void DisableAllSceneObjects()
    {
        m_AircraftTRS.gameObject.SetActive(false);
        m_RunwayTRS.gameObject.SetActive(false);
        m_CrossProductPointer.gameObject.SetActive(false);
    }
}

public enum Method
{
    Vector3_Angle,

    Vector3_Dot,

    Vector3_Cross,

}
