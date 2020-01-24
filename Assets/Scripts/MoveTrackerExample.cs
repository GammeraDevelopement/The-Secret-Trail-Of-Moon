using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.PS4;
using UnityEngine.PS4.IODevices;
using UnityEngine.PS4.Engines;
using System.ComponentModel;
using System.IO;

class MoveTrackerExample : MonoBehaviour
{
	UnityEngine.PS4.PS4ImageStream videoTex;

	UnityEngine.PS4.PS4ImageStream depthTex;
	


	int CameraDeviceHandle = -1;
	public Texture2D PadTrackerCursorTexture = null;

	UnityEngine.PS4.Engines.SceMoveTrackerState MoveTrackerData;
	GameObject trackerobject;
	GameObject []trackedObjects;
	bool doTracking = true;
	int lastmovebuttons = 0;
	
	private int numTrackedControllers=0;
	private UInt16 SCE_MOVE_BUTTON_TRIANGLE = (1 << 4);
	private UInt16 SCE_MOVE_BUTTON_CIRCLE = (1 << 5);
	private UInt16 SCE_MOVE_BUTTON_CROSS = (1 << 6);
	private	UInt16 SCE_MOVE_BUTTON_SQUARE = (1 << 7);

	
	void Initialise()
	{
		System.Console.WriteLine("tracking initialise");

		if (PS4Camera.IsAttached(0) != 1)
		{
			// no camera ... we should retry every several seconds
			System.Console.WriteLine("no camera found");
			return;
		}

		PS4Camera.Init();

		MoveTracker.Init();

		CameraDeviceHandle = PS4Camera.Open(vars.SCE_USER_SERVICE_USER_ID_SYSTEM, 0, 0, IntPtr.Zero);
		if (CameraDeviceHandle < 0)
		{
			System.Console.WriteLine("camera open failed :0x{0:X}", CameraDeviceHandle);
		}

		System.Console.WriteLine("camera handle : 0x{0:X}", CameraDeviceHandle);

		SceCameraConfig cameraConfig = new SceCameraConfig();

		cameraConfig.configType = SceCameraConfigType.SCE_CAMERA_CONFIG_TYPE1;      // 1 is used for tracking
//		cameraConfig.configType = SceCameraConfigType.SCE_CAMERA_CONFIG_TYPE3;      // 3 is suitable for depth (as both channels have auto exposure enabled)

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

		// set special mode 1 for camera 1 that is to be used for the tracking
		PS4Camera.SetExposureGainMode(SceCameraAttributeExposureGainMode.MODE_0, SceCameraAttributeExposureGainMode.MODE_1);
		
		doTracking = true;
	}

	void Shutdown()
	{
		System.Console.WriteLine("tracking shutdown");
		videoTex.Update(IntPtr.Zero, 1280, 800, PS4ImageStream.Type.YUV422, 0);
		
		MoveTracker.Term();
		PS4Camera.Term();
		CameraDeviceHandle = -1;
		
	}
	
	void Start()
	{
		Initialise();

		// find the demonstration tracked 3d object
		trackerobject = GameObject.Find("Capsule");
		if (trackerobject == null)
		{
			throw new Exception("unable to find object form tracking demonstration");
		}		
		trackerobject.SetActive(false);
		trackedObjects = new GameObject[4];		// up to 4 move controller supported by the PS4
		for (int i=0; i<4; i++)
		{
			trackedObjects[i] = Instantiate(trackerobject);
		}
		
		videoTex = new UnityEngine.PS4.PS4ImageStream();
		videoTex.Create(1280, 800, PS4ImageStream.Type.YUV422, 0);

		depthTex = new UnityEngine.PS4.PS4ImageStream();
		depthTex.Create(320,200, PS4ImageStream.Type.R16, 0);

		GameObject BackgroundImage = GameObject.Find("BackgroundImageQuad");
		BackgroundImage.GetComponent<Renderer>().material.mainTexture = videoTex.GetTexture();

		// invert the camera texture to render it correctly
		BackgroundImage.GetComponent<Renderer>().material.mainTextureScale = new Vector2(1, -1);
		BackgroundImage.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, 1);

		BackgroundImage.GetComponent<Renderer>().material.SetVector("colorCoeff", new Vector4(1, 1, 1, 1));
	}

	void ProcessTrackedMoveController(int moveControllerHandle)
	{
		int result = MoveTracker.ReadState(moveControllerHandle, out MoveTrackerData);
		if (result != 0)
		{
			System.Console.WriteLine(String.Format("MoveTracker ReadState error:0x{0:X}", result));
		}
		else
		{
		
			if ((MoveTrackerData.flags & 1)!=0)	// Library has sufficient image data to track controller position. ... see SCE_MOVE_TRACKER_FLAG_POSITION_TRACKED in sdk move_tracker.h
			{
				if (trackedObjects[numTrackedControllers] == null)
					System.Console.WriteLine("trackerobject is null");
				
				trackedObjects[numTrackedControllers].SetActive(true);
				trackedObjects[numTrackedControllers].transform.position = MoveTrackerData.position * 100;			
				trackedObjects[numTrackedControllers].transform.rotation = MoveTrackerData.orientation;
				numTrackedControllers++;
			}

//			System.Console.WriteLine("analogT:" + MoveTrackerData.pad.analogT + " X:" + MoveTrackerData.position.x + " Y:" + MoveTrackerData.position.y + " Z:" + MoveTrackerData.position.z);

			if ((MoveTrackerData.pad.digitalButtons & SCE_MOVE_BUTTON_SQUARE) != 0)       // "[]"/Square ... or key 0x160
			{
				System.Console.WriteLine("calibrating move tracking");
				MoveTracker.Calibrate();
			}
		}
	}


	void Update()
	{

		int movebuttons = PS4Input.MoveGetButtons(0,0);	// get buttons from user 0 move controller 0

		if (((lastmovebuttons & SCE_MOVE_BUTTON_CROSS) != 0)&&((movebuttons & SCE_MOVE_BUTTON_CROSS)==0))// cross ...  initialise
			Initialise();

		if (((lastmovebuttons & SCE_MOVE_BUTTON_TRIANGLE) != 0)&&((movebuttons & SCE_MOVE_BUTTON_TRIANGLE)==0))	// tri just been released ...  toggle tracking handling
			doTracking = !doTracking;
					
			
		if (((lastmovebuttons & SCE_MOVE_BUTTON_CIRCLE) != 0)&&((movebuttons & SCE_MOVE_BUTTON_CIRCLE)==0)) // circle ...  terminate
			Shutdown();
		
		lastmovebuttons = movebuttons;
		
		if ((CameraDeviceHandle >= 0)&&(doTracking))
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

			 
			int[] controllerHandles = new int[6];
#if UNITY_2017_2_OR_NEWER
			int numActiveUsers = PS4Input.PadGetUsersHandles(6, controllerHandles);
#else			
			int numActiveUsers = PS4Input.PadGetUsersPadHandles(6, controllerHandles);
#endif			

			IntPtr controllerInputs;
			controllerInputs = PS4Input.MoveGetControllerInputForTracking();

			if ((movebuttons & SCE_MOVE_BUTTON_SQUARE) != 0)       // "[]"/Square ... or key 0x160
			{
				System.Console.WriteLine("calibrating move tracking (from PS4Input)");
				MoveTracker.Calibrate();
			}
			

			MoveTracker.QueueUpdate(cameraFrameHandle, controllerHandles, controllerInputs);

			// TODO: Investigate why this line stops MoveTracker.ReadState() from returning '3'
//			System.Console.WriteLine("numActiveUsers:" + numActiveUsers);

			numTrackedControllers=0;
			for (int i = 0; i < numActiveUsers; i++)
			{
#if UNITY_2017_2_OR_NEWER
				PS4Input.LoggedInUser userdetails = PS4Input.GetUsersDetails(i);
#else				
				PS4Input.LoggedInUser userdetails = PS4Input.PadGetUsersDetails(i);
#endif				
				ProcessTrackedMoveController(userdetails.move0Handle);
				ProcessTrackedMoveController(userdetails.move1Handle);
			}
		}
		else
		{
			// make sure we update the camera video texture so that it doesn't have stale data
			videoTex.Update(IntPtr.Zero, 1280, 800, PS4ImageStream.Type.YUV422, 0);
		}
	}





	void OnGUI()
	{
			int line = 1;
			int gap = 20;
			int leftmargin = 40;

			GUI.Label(new Rect(leftmargin, gap * line++, 500, 20), String.Format("SQUARE:calibrate TRI:toggle tracking  CIRCLE:shutdown CROSS:startup"));

			GUI.Label(new Rect(leftmargin, gap * line++, 500, 20), String.Format("Number of tracked Move controllers:{0}", numTrackedControllers));
			GUI.Label(new Rect(leftmargin, gap * line++, 500, 20), String.Format("Move: analogButton:{0}", MoveTrackerData.pad.analogT));
			GUI.Label(new Rect(leftmargin, gap * line++, 500, 20), String.Format("Move: timestamp:{0:X}", MoveTrackerData.timestamp));
			GUI.Label(new Rect(leftmargin, gap * line++, 500, 20), String.Format("Move: pos(m):({0},{1},{2})", MoveTrackerData.position.x, MoveTrackerData.position.y, MoveTrackerData.position.z ));
			GUI.Label(new Rect(leftmargin, gap * line++, 500, 20), String.Format("Move: rotation:({0},{1},{2},{3})", MoveTrackerData.orientation.x, MoveTrackerData.orientation.y, MoveTrackerData.orientation.z, MoveTrackerData.orientation.w ));
	}

}
