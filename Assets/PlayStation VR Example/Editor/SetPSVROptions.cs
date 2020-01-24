using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;
using System.IO;

namespace PlayStationVRExample
{
    public class SetPsvrOptions
    {
        // Set all of the Player Settings at once, rather than individually
        [MenuItem("PlayStation VR/Individual Settings/Change All Settings", false, 0)]
        static void SetAllPlayerSettings()
        {
            if (EditorUtility.DisplayDialog("Set PlayStation VR Settings?", "This will overwrite many of your Player Settings. Are you sure you want to continue? ", "Yes", "No"))
            {
                SetOptions();
                ReplaceInputManager();
                ReplaceTagManager();
                SetScenesForBuild();
            }
        }

        // Set a VR resolution and enable VR and Social Screen support
        [MenuItem("PlayStation VR/Individual Settings/Set VR Player Settings")]
        static void SetOptions()
        {
            PlayerSettings.PS4.videoOutReprojectionRate = 120;
            PlayerSettings.PS4.videoOutInitialWidth = 3840;

            // Enable VR (cross-platform setting)
            PlayerSettings.virtualRealitySupported = true;

            // Enable Social Screen Support (shows a different, non-VR view on the monitor)
            PlayerSettings.PS4.socialScreenEnabled = 1;

            // Post-reprojection support currently only works in Forward
            var tierSettings = EditorGraphicsSettings.GetTierSettings(BuildTargetGroup.PS4, UnityEngine.Rendering.GraphicsTier.Tier1);
            tierSettings.renderingPath = RenderingPath.Forward;
            EditorGraphicsSettings.SetTierSettings(BuildTargetGroup.PS4, UnityEngine.Rendering.GraphicsTier.Tier1, tierSettings);

            Debug.Log("PlayerSettings configured!");
        }

        // Replace whatever Input Manager you currently have with one to work with the VR Project
        [MenuItem("PlayStation VR/Individual Settings/Set VR Input Manager")]
        static void ReplaceInputManager()
        {
            // This is the InputManager asset that comes with the example project. Note that to avoid an import error, the '.asset' file extension has been removed
            var sourceFile = Path.Combine(Application.dataPath, "PlayStation VR Example/Editor/InputManager");

            // This is the InputManager in your ProjectSettings folder
            var targetFile = Application.dataPath;
            targetFile = targetFile.Replace("/Assets", "/ProjectSettings/InputManager.asset");

            // Replace the ProjectSettings file with the new one, and trigger a refresh so the Editor sees it
            FileUtil.ReplaceFile(sourceFile, targetFile);
            AssetDatabase.Refresh();

            Debug.Log("InputManager replaced!");
        }

        // Replace your layers and tags with the ones required by the VR Project
        [MenuItem("PlayStation VR/Individual Settings/Set VR Tag Manager")]
        static void ReplaceTagManager()
        {
            // This is the InputManager asset that comes with the example project. Note that to avoid an import error, the '.asset' file extension has been removed
            var sourceFile = Path.Combine(Application.dataPath, "PlayStation VR Example/Editor/TagManager");

            // This is the InputManager in your ProjectSettings folder
            var targetFile = Application.dataPath;
            targetFile = targetFile.Replace("/Assets", "/ProjectSettings/TagManager.asset");

            // Replace the ProjectSettings file with the new one, and trigger a refresh so the Editor sees it
            FileUtil.ReplaceFile(sourceFile, targetFile);
            AssetDatabase.Refresh();

            Debug.Log("TagManager replaced!");
        }

        // Replace the current list of the scenes for the bUIld with the ones for the VR Example
        [MenuItem("PlayStation VR/Individual Settings/Set Build Scenes")]
        static void SetScenesForBuild()
        {
            var vrScenes = new EditorBuildSettingsScene[5];
            vrScenes[0] = new EditorBuildSettingsScene("Assets/PlayStation VR Example/Scenes/PSVRExample_Setup.unity", true);
            vrScenes[1] = new EditorBuildSettingsScene("Assets/PlayStation VR Example/Scenes/PSVRExample_MainMenu.unity", true);
            vrScenes[2] = new EditorBuildSettingsScene("Assets/PlayStation VR Example/Scenes/PSVRExample_DualShock4.unity", true);
            vrScenes[3] = new EditorBuildSettingsScene("Assets/PlayStation VR Example/Scenes/PSVRExample_MoveControllers.unity", true);
            vrScenes[4] = new EditorBuildSettingsScene("Assets/PlayStation VR Example/Scenes/PSVRExample_AimController.unity", true);

            EditorBuildSettings.scenes = vrScenes;

            Debug.Log("Scenes in Build changed!");
        }
    }
}