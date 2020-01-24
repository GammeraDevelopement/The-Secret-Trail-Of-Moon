using UnityEditor;
using UnityEngine;

namespace PlayStationVRExample
{
    [CustomEditor(typeof(TrackedPlayStationDevices))]
    [CanEditMultipleObjects]
    public class TrackedPlayStationDevicesEditor : Editor
    {
        SerializedProperty m_TrackedDualShock4Transform;
        SerializedProperty m_TrackedAimTransform;
        SerializedProperty m_TrackedMoveTransformPrimary;
        SerializedProperty m_TrackedMoveTransformSecondary;
        SerializedProperty m_TrackedDualShock4Light;
        SerializedProperty m_TrackedAimLight;
        SerializedProperty m_TrackedMoveLightPrimary;
        SerializedProperty m_TrackedMoveLightSecondary;

        SerializedProperty m_TrackingType;
        SerializedProperty m_TrackerUsageType;

        void OnEnable()
        {
            // Setup the SerializedProperties.
            m_TrackedDualShock4Transform = serializedObject.FindProperty("deviceDualShock4").FindPropertyRelative("transform");
            m_TrackedAimTransform = serializedObject.FindProperty("deviceAim").FindPropertyRelative("transform");
            m_TrackedMoveTransformPrimary = serializedObject.FindProperty("deviceMovePrimary").FindPropertyRelative("transform");
            m_TrackedMoveTransformSecondary = serializedObject.FindProperty("deviceMoveSecondary").FindPropertyRelative("transform");
            m_TrackedDualShock4Light = serializedObject.FindProperty("deviceDualShock4").FindPropertyRelative("light");
            m_TrackedAimLight = serializedObject.FindProperty("deviceAim").FindPropertyRelative("light");
            m_TrackedMoveLightPrimary = serializedObject.FindProperty("deviceMovePrimary").FindPropertyRelative("light");
            m_TrackedMoveLightSecondary = serializedObject.FindProperty("deviceMoveSecondary").FindPropertyRelative("light");
            m_TrackingType = serializedObject.FindProperty("trackingType");
            m_TrackerUsageType = serializedObject.FindProperty("trackerUsageType");
        }

        public override void OnInspectorGUI()
        {
            // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
            serializedObject.Update();
            var script = (TrackedPlayStationDevices)target;

            script.trackedDevicesType = (TrackedPlayStationDevices.TrackedDevicesType)EditorGUILayout.EnumFlagsField(new GUIContent("Trackable Devices"), script.trackedDevicesType);
            var deviceTypes = script.trackedDevicesType;

            EditorGUILayout.PropertyField(m_TrackingType, new GUIContent("Tracking Type"));
            EditorGUILayout.PropertyField(m_TrackerUsageType, new GUIContent("Tracker Usage Type"));

            if ((deviceTypes & TrackedPlayStationDevices.TrackedDevicesType.DualShock4) == TrackedPlayStationDevices.TrackedDevicesType.DualShock4)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("DualShock 4 Controller", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_TrackedDualShock4Transform, new GUIContent("Transform"));
                EditorGUILayout.PropertyField(m_TrackedDualShock4Light, new GUIContent("Light Renderer"));
            }

            if ((deviceTypes & TrackedPlayStationDevices.TrackedDevicesType.Aim) == TrackedPlayStationDevices.TrackedDevicesType.Aim)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("PS VR Aim Controller", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_TrackedAimTransform, new GUIContent("Transform"));
                EditorGUILayout.PropertyField(m_TrackedAimLight, new GUIContent("Light Renderer"));
            }

            if ((deviceTypes & TrackedPlayStationDevices.TrackedDevicesType.Move) == TrackedPlayStationDevices.TrackedDevicesType.Move)
            {
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Move Controller (Primary)", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_TrackedMoveTransformPrimary, new GUIContent("Transform"));
                EditorGUILayout.PropertyField(m_TrackedMoveLightPrimary, new GUIContent("Light Renderer"));

                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("Move Controller (Secondary)", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(m_TrackedMoveTransformSecondary, new GUIContent("Transform"));
                EditorGUILayout.PropertyField(m_TrackedMoveLightSecondary, new GUIContent("Light Renderer"));
            }

            // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
            serializedObject.ApplyModifiedProperties();
        }
    }
}