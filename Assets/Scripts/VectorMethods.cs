using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

#pragma warning disable 0649

// TODO get refs. from code by tag. Remove references from the inspector
[ExecuteInEditMode]
public class VectorMethods : MonoBehaviour
{
    [SerializeField]
    private Method m_Method;

    [SerializeField]
    private Transform m_AircraftTRS;

    [SerializeField]
    private Transform m_RunwayTRS;

    [SerializeField]
    private Transform m_TargetPoint;

    [SerializeField]
    private LineRenderer m_AircraftLineRenderer;

    [SerializeField]
    private LineRenderer m_RunwayLineRenderer;

    [SerializeField]
    private LineRenderer m_HelperLineA;

    [SerializeField]
    private float m_LineRendererLenght = 10;

    [SerializeField]
    private float m_LineRendererWidth = .2f;

    /// <summary>
    /// Used to pass data from Update() to OnGUI().
    /// OnGUI() must know the order in which the data was added to the list.
    /// </summary>
    private List<object> m_OnGUIData = new List<object>();

    #region UNITY MESSAGES

    private void Awake()
    {
        Assert.IsNotNull(m_AircraftTRS);
        Assert.IsNotNull(m_RunwayTRS);
        Assert.IsNotNull(m_HelperLineA);
        Assert.IsNotNull(m_AircraftLineRenderer);
        Assert.IsNotNull(m_RunwayLineRenderer);
        Assert.IsNotNull(m_HelperLineA);
        Assert.IsNotNull(m_TargetPoint);
    }

    private void Start()
    {
        InitLineRenderers();
    }

    void Update()
    {
        if (!AllRefsSet())
        {
            return;
        }

        HandleMethodType();
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
            case Method.Vector3Angle:
                {
                    if (m_OnGUIData.Count < 1)
                    {
                        return;
                    }
                    GUILayout.Label("Vector3.Angle()");
                    GUILayout.Label(string.Format("Angle: {0}", m_OnGUIData[0]));
                    break;
                }
            case Method.Vector3Dot:
                {
                    if (m_OnGUIData.Count < 1)
                    {
                        return;
                    }
                    GUILayout.Label("Vector3.Dot()");
                    GUILayout.Label(string.Format("Dot Product: {0}", m_OnGUIData[0]));
                    break;
                }
            case Method.Vector3Cross:
                {
                    if (m_OnGUIData.Count < 1)
                    {
                        return;
                    }
                    GUILayout.Label("Vector3.Cross()");
                    GUILayout.Label(string.Format("Cross Product: {0}", m_OnGUIData[0]));
                    break;
                }
            case Method.QuaternionSetFromToRotation:
                {
                    if (m_OnGUIData.Count < 2)
                    {
                        return;
                    }
                    GUILayout.Label("Quaternion.SetFromToRotation()");
                    GUILayout.Label("public void SetFromToRotation(Vector3 fromDirection, Vector3 toDirection)");
                    GUILayout.Label("fromDirection: aircraft forward vector. toDirection: direction from aircraft to the target");
                    GUILayout.Label("Aircraft rotation is multiplied by the result of this method.");
                    GUILayout.Label(string.Format("Aircraft dir to target (G): {0}", m_OnGUIData[0]));
                    GUILayout.Label(string.Format("Quaternion: {0}", m_OnGUIData[1]));
                    break;
                }
            case Method.QuaternionSetLookRotation:
                {
                    if (m_OnGUIData.Count < 2)
                    {
                        return;
                    }
                    GUILayout.Label("Quaternion.SetLookRotation()");
                    GUILayout.Label("public void SetLookRotation(Vector3 view, Vector3 up = Vector3.up)");
                    GUILayout.Label("view: direction from aircraft to the target");
                    GUILayout.Label("Resulting quaternion is assigned to aircraft rotation");
                    GUILayout.Label(string.Format("Aircraft dir to target (G): {0}", m_OnGUIData[0]));
                    GUILayout.Label(string.Format("Quaternion: {0}", m_OnGUIData[1]));
                    break;
                }
            case Method.QuaternionToAngleAxis:
                {
                    if (m_OnGUIData.Count < 2)
                    {
                        return;
                    }
                    GUILayout.Label("Quaternion.ToAngleAxis()");
                    GUILayout.Label("public void ToAngleAxis(out float angle, out Vector3 axis)");
                    GUILayout.Label("Converts aircraft rotation (quaternion) to angle and axis");
                    GUILayout.Label(string.Format("Angle: {0}", m_OnGUIData[0]));
                    GUILayout.Label(string.Format("Axis: {0}", m_OnGUIData[1]));
                    break;
                }
        }

        GUILayout.EndVertical();
    }

    #endregion

    private void HandleMethodType()
    {
        switch (m_Method)
        {
            case Method.Vector3Angle:
                {
                    ResetScene();

                    UpdateAircraftLineRenderer();
                    UpdateRunwayLineRenderer();

                    // Setup scene objects.
                    m_AircraftTRS.gameObject.SetActive(true);
                    m_RunwayTRS.gameObject.SetActive(true);

                    float angle = Vector3.Angle(m_AircraftTRS.forward, m_RunwayTRS.forward);
                    m_OnGUIData.Add(angle);
                    break;
                }
            case Method.Vector3Dot:
                {
                    ResetScene();

                    UpdateAircraftLineRenderer();
                    UpdateRunwayLineRenderer();

                    m_AircraftTRS.gameObject.SetActive(true);
                    m_RunwayTRS.gameObject.SetActive(true);

                    float dot = Vector3.Dot(m_AircraftTRS.forward, m_RunwayTRS.forward);
                    m_OnGUIData.Add(dot);
                    break;
                }
            case Method.Vector3Cross:
                {
                    ResetScene();

                    UpdateAircraftLineRenderer();
                    UpdateRunwayLineRenderer();

                    m_AircraftTRS.gameObject.SetActive(true);
                    m_RunwayTRS.gameObject.SetActive(true);
                    m_HelperLineA.gameObject.SetActive(true);

                    Vector3 cross = Vector3.Cross(m_AircraftTRS.forward, m_RunwayTRS.forward);
                    m_OnGUIData.Add(cross);

                    // Draw line.
                    m_HelperLineA.SetPosition(0, m_RunwayTRS.position);
                    m_HelperLineA.SetPosition(1, m_RunwayTRS.position + cross * m_LineRendererLenght);

                    break;
                }
            case Method.QuaternionSetFromToRotation:
                {
                    ResetScene();
                    ResetAircraft();

                    UpdateAircraftLineRenderer();
                    UpdateRunwayLineRenderer();

                    m_AircraftTRS.gameObject.SetActive(true);
                    m_RunwayTRS.gameObject.SetActive(true);
                    m_TargetPoint.gameObject.SetActive(true);

                    Quaternion quaternion = new Quaternion();
                    Vector3 dirToTarget = m_TargetPoint.position - m_AircraftTRS.position;
                    m_OnGUIData.Add(dirToTarget.normalized);
                    quaternion.SetFromToRotation(m_AircraftTRS.forward, dirToTarget);
                    m_OnGUIData.Add(quaternion.eulerAngles);
                    m_AircraftTRS.rotation = quaternion * m_AircraftTRS.rotation;

                    break;
                }
            case Method.QuaternionSetLookRotation:
                {
                    ResetScene();
                    ResetAircraft();

                    UpdateAircraftLineRenderer();
                    UpdateRunwayLineRenderer();

                    m_AircraftTRS.gameObject.SetActive(true);
                    m_RunwayTRS.gameObject.SetActive(true);
                    m_TargetPoint.gameObject.SetActive(true);

                    Quaternion quaternion = new Quaternion();
                    Vector3 dirToTarget = m_TargetPoint.position - m_AircraftTRS.position;
                    m_OnGUIData.Add(dirToTarget.normalized);
                    quaternion.SetLookRotation(dirToTarget);
                    m_OnGUIData.Add(quaternion.eulerAngles);
                    m_AircraftTRS.rotation = quaternion;

                    break;
                }
            case Method.QuaternionToAngleAxis:
                {
                    ResetScene();

                    UpdateAircraftLineRenderer();
                    UpdateRunwayLineRenderer();

                    m_AircraftTRS.gameObject.SetActive(true);
                    m_RunwayTRS.gameObject.SetActive(true);
                    m_HelperLineA.gameObject.SetActive(true);

                    float angle = 0;
                    Vector3 axis = Vector3.zero;
                    m_AircraftTRS.rotation.ToAngleAxis(out angle, out axis);

                    // Draw line along axis.
                    m_HelperLineA.SetPosition(0, m_RunwayTRS.position);
                    m_HelperLineA.SetPosition(1, m_RunwayTRS.position + axis * m_LineRendererLenght);

                    m_OnGUIData.Add(angle);
                    m_OnGUIData.Add(axis);

                    break;
                }
        }
    }

    private void InitLineRenderers()
    {
        m_AircraftLineRenderer.startWidth = m_LineRendererWidth;
        m_AircraftLineRenderer.endWidth = m_LineRendererWidth;
        m_RunwayLineRenderer.startWidth = m_LineRendererWidth;
        m_RunwayLineRenderer.endWidth = m_LineRendererWidth;
        m_HelperLineA.startWidth = m_LineRendererWidth;
        m_HelperLineA.endWidth = m_LineRendererWidth;
    }

    private bool AllRefsSet()
    {
        if (m_AircraftTRS && m_RunwayTRS && m_HelperLineA && m_AircraftLineRenderer && m_RunwayLineRenderer)
        {
            return true;
        }

        return false;
    }

    // TODO use NaughtyAttributes OnValueChanged to call these methods
    private void ResetScene()
    {
        m_OnGUIData.Clear();
        DisableAllSceneObjects();
    }

    private void ResetAircraft()
    {
        m_AircraftTRS.position = Vector3.zero;
        m_AircraftTRS.rotation = Quaternion.identity;
    }

    private void DisableAllSceneObjects()
    {
        m_AircraftTRS.gameObject.SetActive(false);
        m_RunwayTRS.gameObject.SetActive(false);
        m_HelperLineA.gameObject.SetActive(false);
        m_TargetPoint.gameObject.SetActive(false);
    }

    private void UpdateAircraftLineRenderer()
    {
        m_AircraftLineRenderer.SetPosition(0, m_AircraftTRS.position);
        m_AircraftLineRenderer.SetPosition(1, m_AircraftTRS.position + m_AircraftTRS.forward * m_LineRendererLenght);
    }

    private void UpdateRunwayLineRenderer()
    {
        m_RunwayLineRenderer.SetPosition(0, m_RunwayTRS.position);
        m_RunwayLineRenderer.SetPosition(1, m_RunwayTRS.position + m_RunwayTRS.forward * m_LineRendererLenght);
    }

}

public enum Method
{
    Vector3Angle,

    Vector3Dot,

    Vector3Cross,

    QuaternionSetFromToRotation,

    QuaternionSetLookRotation,

    QuaternionToAngleAxis
}
