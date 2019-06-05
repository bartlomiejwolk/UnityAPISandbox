using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

#pragma warning disable 0649

[ExecuteInEditMode]
public class RotationMethods : MonoBehaviour
{
    [NaughtyAttributes.OnValueChanged("ResetScene")]
    [SerializeField]
    private APIMethod m_Method;

    [Header("References")]
    [SerializeField]
    private Transform m_AircraftTRS;

    [SerializeField]
    private Transform m_RunwayTRS;

    [SerializeField]
    private Transform m_TargetPointTRS;

    [SerializeField]
    private LineRenderer m_HelperLine;

    [Header("Settings")]
    [SerializeField]
    private float m_LineRendererLenght = 10;

    [SerializeField]
    private float m_LineRendererWidth = .2f;

    /// <summary>
    /// Used to pass data from Update() to OnGUI().
    /// OnGUI() must know the order in which the data was added to the list.
    /// </summary>
    private List<object> m_OnGUIData = new List<object>();

    private LineRenderer m_AircraftLineRenderer;

    private LineRenderer m_RunwayLineRenderer;

    #region UNITY MESSAGES

    private void Awake()
    {
        Assert.IsNotNull(m_AircraftTRS);
        Assert.IsNotNull(m_RunwayTRS);
        Assert.IsNotNull(m_HelperLine);
        Assert.IsNotNull(m_HelperLine);
        Assert.IsNotNull(m_TargetPointTRS);

        m_AircraftLineRenderer = m_AircraftTRS.GetComponent<LineRenderer>();
        m_RunwayLineRenderer = m_RunwayTRS.GetComponent<LineRenderer>();

        Assert.IsNotNull(m_AircraftLineRenderer);
        Assert.IsNotNull(m_RunwayLineRenderer);
    }

    private void Start()
    {
        SetupLineRenderers();
    }

    void Update()
    {
        if (!AllRefsSet())
        {
            return;
        }

        ExecuteSelectedAPI();
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
            case APIMethod.Vector3Angle:
                {
                    if (m_OnGUIData.Count < 1)
                    {
                        return;
                    }
                    GUILayout.Label("Vector3.Angle()");
                    GUILayout.Label(string.Format("Angle: {0}", m_OnGUIData[0]));
                    break;
                }
            case APIMethod.Vector3Dot:
                {
                    if (m_OnGUIData.Count < 1)
                    {
                        return;
                    }
                    GUILayout.Label("Vector3.Dot()");
                    GUILayout.Label(string.Format("Dot Product: {0}", m_OnGUIData[0]));
                    break;
                }
            case APIMethod.Vector3Cross:
                {
                    if (m_OnGUIData.Count < 1)
                    {
                        return;
                    }
                    GUILayout.Label("Vector3.Cross()");
                    GUILayout.Label(string.Format("Cross Product: {0}", m_OnGUIData[0]));
                    break;
                }
            case APIMethod.Vector3RotateTowards:
                {
                    if (m_OnGUIData.Count < 2)
                    {
                        return;
                    }
                    GUILayout.Label("Vector3.RotateTowards()");
                    GUILayout.Label("public static Vector3 RotateTowards(Vector3 current, Vector3 target, float maxRadiansDelta, float maxMagnitudeDelta)");
                    GUILayout.Label("current: aircraft forward vector. target: direction from aircraft to the target");
                    GUILayout.Label("Aircraft is rotated with Quaternion.LookRotation(newDir)");
                    GUILayout.Label(string.Format("Target dir. (G): {0}", m_OnGUIData[0]));
                    GUILayout.Label(string.Format("New dir. (G): {0}", m_OnGUIData[1]));
                    break;
                }
            case APIMethod.QuaternionFromToRotation:
                {
                    if (m_OnGUIData.Count < 2)
                    {
                        return;
                    }
                    GUILayout.Label("Quaternion.FromToRotation()");
                    GUILayout.Label("public static Quaternion FromToRotation(Vector3 fromDirection, Vector3 toDirection)");
                    GUILayout.Label("fromDirection: aircraft forward vector. toDirection: direction from aircraft to the target");
                    GUILayout.Label("Aircraft rotation is multiplied by the result of this method.");
                    GUILayout.Label(string.Format("Aircraft dir to target (G): {0}", m_OnGUIData[0]));
                    GUILayout.Label(string.Format("Quaternion: {0}", m_OnGUIData[1]));
                    break;
                }
            case APIMethod.QuaternionLookRotation:
                {
                    if (m_OnGUIData.Count < 2)
                    {
                        return;
                    }
                    GUILayout.Label("Quaternion.LookRotation()");
                    GUILayout.Label("public static Quaternion LookRotation(Vector3 forward, Vector3 upwards = Vector3.up)");
                    GUILayout.Label("forward: direction from aircraft to the target");
                    GUILayout.Label("Resulting quaternion is assigned to aircraft rotation");
                    GUILayout.Label(string.Format("Aircraft dir to target (G): {0}", m_OnGUIData[0]));
                    GUILayout.Label(string.Format("Quaternion: {0}", m_OnGUIData[1]));
                    break;
                }
            case APIMethod.QuaternionRotateTowards:
                {
                    if (m_OnGUIData.Count < 2)
                    {
                        return;
                    }
                    GUILayout.Label("Quaternion.RotateTowards()");
                    GUILayout.Label("public static Quaternion RotateTowards(Quaternion from, Quaternion to, float maxDegreesDelta)");
                    GUILayout.Label("from: aircraft rotation. to: rot. to target");
                    GUILayout.Label("Resulting quaternion is assigned to aircraft rotation");
                    GUILayout.Label(string.Format("Aircraft dir to target (G): {0}", m_OnGUIData[0]));
                    GUILayout.Label(string.Format("Rot. to target (from Vector3.zero): {0}", m_OnGUIData[1]));
                    GUILayout.Label(string.Format("New aircraft rotation: {0}", m_OnGUIData[2]));
                    break;
                }
            case APIMethod.QuaternionToAngleAxis:
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

    private void ExecuteSelectedAPI()
    {
        UpdateLineRenderers();

        switch (m_Method)
        {
            case APIMethod.Vector3Angle:
                {
                    m_OnGUIData.Clear();

                    // Setup scene objects.
                    m_AircraftTRS.gameObject.SetActive(true);
                    m_RunwayTRS.gameObject.SetActive(true);

                    float angle = Vector3.Angle(m_AircraftTRS.forward, m_RunwayTRS.forward);
                    m_OnGUIData.Add(angle);

                    break;
                }
            case APIMethod.Vector3Dot:
                {
                    m_OnGUIData.Clear();

                    m_AircraftTRS.gameObject.SetActive(true);
                    m_RunwayTRS.gameObject.SetActive(true);

                    float dot = Vector3.Dot(m_AircraftTRS.forward, m_RunwayTRS.forward);
                    m_OnGUIData.Add(dot);

                    break;
                }
            case APIMethod.Vector3Cross:
                {
                    m_OnGUIData.Clear();

                    m_AircraftTRS.gameObject.SetActive(true);
                    m_RunwayTRS.gameObject.SetActive(true);
                    m_HelperLine.gameObject.SetActive(true);

                    Vector3 cross = Vector3.Cross(m_AircraftTRS.forward, m_RunwayTRS.forward);
                    m_OnGUIData.Add(cross);

                    // Draw line.
                    m_HelperLine.SetPosition(0, m_RunwayTRS.position);
                    m_HelperLine.SetPosition(1, m_RunwayTRS.position + cross * m_LineRendererLenght);

                    break;
                }
            case APIMethod.Vector3RotateTowards:
                {
                    m_OnGUIData.Clear();

                    m_AircraftTRS.gameObject.SetActive(true);
                    m_RunwayTRS.gameObject.SetActive(true);
                    m_TargetPointTRS.gameObject.SetActive(true);

                    Vector3 targetDir = m_TargetPointTRS.position - m_AircraftTRS.position;
                    m_OnGUIData.Add(targetDir.normalized);
                    float speed = 0.26f * Time.deltaTime;
                    Vector3 newDir = Vector3.RotateTowards(m_AircraftTRS.forward, targetDir, speed, 0);
                    m_OnGUIData.Add(newDir.normalized);
                    m_AircraftTRS.rotation = Quaternion.LookRotation(newDir);

                    break;
                }
            case APIMethod.QuaternionFromToRotation:
                {
                    m_OnGUIData.Clear();

                    m_AircraftTRS.gameObject.SetActive(true);
                    m_RunwayTRS.gameObject.SetActive(true);
                    m_TargetPointTRS.gameObject.SetActive(true);

                    Vector3 dirToTarget = m_TargetPointTRS.position - m_AircraftTRS.position;
                    m_OnGUIData.Add(dirToTarget.normalized);
                    Quaternion quaternion = Quaternion.FromToRotation(m_AircraftTRS.forward, dirToTarget);
                    m_OnGUIData.Add(quaternion.eulerAngles);
                    m_AircraftTRS.rotation = quaternion * m_AircraftTRS.rotation;

                    break;
                }
            case APIMethod.QuaternionLookRotation:
                {
                    m_OnGUIData.Clear();

                    m_AircraftTRS.gameObject.SetActive(true);
                    m_RunwayTRS.gameObject.SetActive(true);
                    m_TargetPointTRS.gameObject.SetActive(true);

                    Vector3 dirToTarget = m_TargetPointTRS.position - m_AircraftTRS.position;
                    m_OnGUIData.Add(dirToTarget.normalized);
                    Quaternion quaternion = Quaternion.LookRotation(dirToTarget);
                    m_OnGUIData.Add(quaternion.eulerAngles);
                    m_AircraftTRS.rotation = quaternion;

                    break;
                }
            case APIMethod.QuaternionRotateTowards:
                {
                    m_OnGUIData.Clear();

                    m_AircraftTRS.gameObject.SetActive(true);
                    m_RunwayTRS.gameObject.SetActive(true);
                    m_TargetPointTRS.gameObject.SetActive(true);

                    Vector3 dirToTarget = m_TargetPointTRS.position - m_AircraftTRS.position;
                    m_OnGUIData.Add(dirToTarget.normalized);
                    Quaternion rotationToTarget = Quaternion.LookRotation(dirToTarget);
                    m_OnGUIData.Add(rotationToTarget.eulerAngles);
                    Quaternion result = Quaternion.RotateTowards(m_AircraftTRS.rotation, rotationToTarget, 1);
                    m_OnGUIData.Add(result.eulerAngles);
                    m_AircraftTRS.rotation = result;

                    break;
                }
            case APIMethod.QuaternionToAngleAxis:
                {
                    m_OnGUIData.Clear();

                    m_AircraftTRS.gameObject.SetActive(true);
                    m_RunwayTRS.gameObject.SetActive(true);
                    m_HelperLine.gameObject.SetActive(true);

                    float angle = 0;
                    Vector3 axis = Vector3.zero;
                    m_AircraftTRS.rotation.ToAngleAxis(out angle, out axis);

                    // Draw line along axis.
                    m_HelperLine.SetPosition(0, m_RunwayTRS.position);
                    m_HelperLine.SetPosition(1, m_RunwayTRS.position + axis * m_LineRendererLenght);

                    m_OnGUIData.Add(angle);
                    m_OnGUIData.Add(axis);

                    break;
                }
        }
    }

    private void SetupLineRenderers()
    {
        m_AircraftLineRenderer.startWidth = m_LineRendererWidth;
        m_AircraftLineRenderer.endWidth = m_LineRendererWidth;
        m_RunwayLineRenderer.startWidth = m_LineRendererWidth;
        m_RunwayLineRenderer.endWidth = m_LineRendererWidth;
        m_HelperLine.startWidth = m_LineRendererWidth;
        m_HelperLine.endWidth = m_LineRendererWidth;
    }

    private bool AllRefsSet()
    {
        if (m_AircraftTRS && m_RunwayTRS && m_HelperLine && m_AircraftLineRenderer && m_RunwayLineRenderer)
        {
            return true;
        }

        return false;
    }

    private void ResetScene()
    {
        DisableAllSceneObjects();

        // Reset this transform in case it was modified accidentally.
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        // Reset aircraft.
        m_AircraftTRS.position = Vector3.zero;
        m_AircraftTRS.rotation = Quaternion.identity;

        // Reset runway.
        m_RunwayTRS.position = Vector3.zero;
        m_RunwayTRS.rotation = Quaternion.identity;
    }
    
    private void DisableAllSceneObjects()
    {
        m_AircraftTRS.gameObject.SetActive(false);
        m_RunwayTRS.gameObject.SetActive(false);
        m_HelperLine.gameObject.SetActive(false);
        m_TargetPointTRS.gameObject.SetActive(false);
    }

    private void UpdateLineRenderers()
    {
        m_AircraftLineRenderer.SetPosition(0, m_AircraftTRS.position);
        m_AircraftLineRenderer.SetPosition(1, m_AircraftTRS.position + m_AircraftTRS.forward * m_LineRendererLenght);

        m_RunwayLineRenderer.SetPosition(0, m_RunwayTRS.position);
        m_RunwayLineRenderer.SetPosition(1, m_RunwayTRS.position + m_RunwayTRS.forward * m_LineRendererLenght);
    }
}

public enum APIMethod
{
    Vector3Angle,

    Vector3Dot,

    Vector3Cross,

    Vector3RotateTowards,

    QuaternionFromToRotation,

    QuaternionLookRotation,

    QuaternionRotateTowards,

    QuaternionToAngleAxis,

}
