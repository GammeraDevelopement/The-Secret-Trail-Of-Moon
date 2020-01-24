using UnityEngine;
using PlayStationVRExample;

namespace ProjectVR
{
    public class VRScape : MonoBehaviour
    {
        void Update()
        {
            if(Input.GetButtonDown("Start"))
            {
                FindObjectOfType<SceneSwitcher>().SwitchToScene("PSVRExample_MainMenu");
            }
        }
    }
}