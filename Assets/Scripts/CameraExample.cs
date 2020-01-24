using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.PS4;
using UnityEngine.PS4.IODevices;
using UnityEngine.PS4.Engines;


class CameraExample : MonoBehaviour
{
    UnityEngine.PS4.PS4ImageStream videoTex;

    UnityEngine.PS4.PS4ImageStream depthTex;
    

//	public int infoCount=0;

    int CameraDeviceHandle = -1;
    public Texture2D PadTrackerCursorTexture = null;
    int [] PadTrackerX;
    int [] PadTrackerY;

    SceDepthHeadCandidateTrackingResult[] HeadTrackingData;
    SceDepthHandCandidateTrackingResult[] HandTrackingData;

    int numCursors=0;
    int numHeads = 0;
    int numHands = 0;

    void Start()
    {
        if (PS4Camera.IsAttached(0) != 1)
        {
            // no camera ... we should retry every several seconds
            System.Console.WriteLine("no camera found");
            return;
        }

        PS4Camera.Init();

        PadTracker.Init();

		Depth.Init(true, true);
        //HandDetection.Init();		// TODO: complete

        CameraDeviceHandle = PS4Camera.Open(vars.SCE_USER_SERVICE_USER_ID_SYSTEM, 0, 0, IntPtr.Zero);
        if (CameraDeviceHandle < 0)
        {
            System.Console.WriteLine("camera open failed :0x{0:X}", CameraDeviceHandle);
        }

        System.Console.WriteLine("camera handle : 0x{0:X}", CameraDeviceHandle);

        SceCameraConfig cameraConfig = new SceCameraConfig();

//        cameraConfig.configType = SceCameraConfigType.SCE_CAMERA_CONFIG_TYPE1;
        cameraConfig.configType = SceCameraConfigType.SCE_CAMERA_CONFIG_TYPE3;      // 3 is suitable for depth (as both channels have auto exposure enabled)


        int result = PS4Camera.SetConfig(CameraDeviceHandle, cameraConfig);
        if (result < 0)
        {
            System.Console.WriteLine("PS4Camera.SetConfig failed: {0:X}", result);
        }


        SceCameraStartParameter start = new SceCameraStartParameter();

        start.formatLevel[0] = sceCameraFrameFormat.SCE_CAMERA_FRAME_FORMAT_ALL;
        start.formatLevel[1] = sceCameraFrameFormat.SCE_CAMERA_FRAME_FORMAT_ALL;

        result = PS4Camera.Start(CameraDeviceHandle, start);
        if (result < 0)
        {
            System.Console.WriteLine("PS4Camera.Start failed:" + result);
        }



        videoTex = new UnityEngine.PS4.PS4ImageStream();
        videoTex.Create(1280, 800, PS4ImageStream.Type.YUV422, 0);

        depthTex = new UnityEngine.PS4.PS4ImageStream();
        depthTex.Create(320,200, PS4ImageStream.Type.R16, 0);


        // apply video texture to the cube and the background quad
        GameObject cube = GameObject.Find("Cube");
        //cube.renderer.material.mainTexture = videoTex.GetTexture();
        cube.GetComponent<Renderer>().material.mainTexture = depthTex.GetTexture();
        cube.GetComponent<Renderer>().material.SetVector("colorCoeff", new Vector4(1, 1, 1, 1));

        GameObject BackgroundImage = GameObject.Find("BackgroundImageQuad");
        BackgroundImage.GetComponent<Renderer>().material.mainTexture = videoTex.GetTexture();

        // invert the camera texture to render it correctly
        BackgroundImage.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1, -1);
        BackgroundImage.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, 1);

        BackgroundImage.GetComponent<Renderer>().material.SetVector("colorCoeff", new Vector4(1, 1, 1, 1));


        numCursors = 0;

        PadTrackerX = new int[16];
        PadTrackerY = new int[16];

    }



    void Update()
    {
        if (CameraDeviceHandle >= 0)
        {
            UInt64 cameraFrameHandle = 0;
            uint readMode = 0; // SCE_CAMERA_FRAME_WAIT_NEXTFRAME_ON | SCE_CAMERA_FRAME_MEMORY_TYPE_ONION;

            int res = PS4Camera.GetFrameData(CameraDeviceHandle, readMode, ref cameraFrameHandle);
            if (res < 0)
            {
                System.Console.WriteLine("camera sceCameraGetFrameData error: 0x{0:X}", res);
            }

            int device = 0;
            int level = 0;
            IntPtr rawdata = PS4Camera.getFrameIntPtr(cameraFrameHandle, device, level);
            videoTex.Update(rawdata, 1280, 800, PS4ImageStream.Type.YUV422, 0);

            Depth.Update(cameraFrameHandle, true, true );

            IntPtr rawdepthdata = Depth.getFrameIntPtr();
            depthTex.Update(rawdepthdata, 80, 50, PS4ImageStream.Type.R16, 0);
            
             int[] controllerHandles = new int[6];
#if UNITY_2017_2_OR_NEWER
            int numActiveUsers = PS4Input.PadGetUsersHandles(6, controllerHandles);
#else             
            int numActiveUsers = PS4Input.PadGetUsersPadHandles(6, controllerHandles);
#endif            

            PadTracker.QueueUpdate(cameraFrameHandle, controllerHandles);

            if (numActiveUsers > 0)
            {
                ScePadTrackerData PadTrackerData;

                numCursors = numActiveUsers;
                for (int i = 0; i < numCursors; i++)
                {
                    PadTracker.ReadState(controllerHandles[i], out PadTrackerData);
                    PadTrackerX[i] = (int)(PadTrackerData.imageCoordinates[1].x * 1920);
                    PadTrackerY[i] = (int)(PadTrackerData.imageCoordinates[1].y * 1080);
                }
            }




            bool HighLevelTesting = true;
            if (HighLevelTesting == true)
            {
                numHeads = Depth.GetValidatedHeadResults(out HeadTrackingData, 8);
                if (numHeads > 0)
                {
                    //System.Console.WriteLine("head0 x:{0} y:{1} w:{2} h:{3} dist:{4} id:{5} state:{6}",
                    //    HeadTrackingData[0].x, HeadTrackingData[0].y,
                    //    HeadTrackingData[0].width, HeadTrackingData[0].height,
                    //    HeadTrackingData[0].distanceFromCamera, HeadTrackingData[0].id, HeadTrackingData[0].state);
                }
            }
            else
            {
                numHeads = Depth.HeadCandidateTrackerGetResult(out HeadTrackingData, 8);
                if (numHeads > 0)
                {
                    //System.Console.WriteLine("head0 x:{0} y:{1} w:{2} h:{3} dist:{4} id:{5} state:{6}",
                    //    HeadTrackingData[0].x, HeadTrackingData[0].y,
                    //    HeadTrackingData[0].width, HeadTrackingData[0].height,
                    //    HeadTrackingData[0].distanceFromCamera, HeadTrackingData[0].id, HeadTrackingData[0].state);
                }
            }


            numHands = Depth.HandCandidateTrackerGetResult(out HandTrackingData, 8);


        }

        if (Input.GetKeyDown("joystick 1 button 0"))        // "X"/Cross .. or 0x15e
        {
            System.Console.WriteLine("calibrating pad tracking");
            PadTracker.Calibrate();
        }
        if (Input.GetKeyDown("joystick 1 button 1"))        // "O"/Circle ... or key 0x15f
        {
            System.Console.WriteLine("Initialising pad tracking");
            PadTracker.Init();

        }
        if (Input.GetKeyDown("joystick 1 button 2"))        // "[]"/Square ... or key 0x160
        {
            System.Console.WriteLine("terminating pad tracking");
            PadTracker.Term();
        }
        
//        infoCount++;
    }





    void OnGUI()
    {
        for (int c = 0; c < numCursors; c++)
        {
            GUI.DrawTexture(new Rect(PadTrackerX[c], PadTrackerY[c], 32, 32), PadTrackerCursorTexture);
        }

        int sw = 1920;
        int sh = 1080;

        for (int c = 0; c < numHeads; c++)
        {
            GUI.DrawTexture(new Rect(HeadTrackingData[c].x * sw, HeadTrackingData[c].y*sh, 32, 32), PadTrackerCursorTexture);
            GUI.Label(new Rect(HeadTrackingData[c].x * sw + 32, HeadTrackingData[c].y * sh + 32, 100, 20), string.Format("Head {0:0.00}m", HeadTrackingData[c].distanceFromCamera));
        }


        for (int c = 0; c < numHands; c++)
        {
            GUI.DrawTexture(new Rect(HandTrackingData[c].x*sw, HandTrackingData[c].y*sh, 32, 32), PadTrackerCursorTexture);
            GUI.Label(new Rect(HandTrackingData[c].x*sw + 32, HandTrackingData[c].y*sh + 32, 100, 20), string.Format("Hand {0:0.00}m", HandTrackingData[c].distanceFromCamera));
        }


    }

}
