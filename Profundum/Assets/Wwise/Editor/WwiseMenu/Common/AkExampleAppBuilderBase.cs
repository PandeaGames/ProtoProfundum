#if UNITY_EDITOR
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2013 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

/// @brief This is an exemple that shows the steps to create a custom build for Unity applications that use Wwise.
/// The build can be started by selecting a target platform and adding scenes to the build in the build settings (File->Build Settings) 
/// and by clicking on "File->Build Unity-Wwise project" on the menu bar.
/// The steps to build a Unity-Wwise project are as follow:
/// 1) 	Copy the soundbank of the current target platform from the Wwise project to the specified folder in the unity project. 
/// 2) 	Build all the scenes defined in unity's build settings.
/// 3) 	Delete the soundbank folder from the Unity project. This step is needed to prevent future builds for other platforms from using that bank.

public class AkExampleAppBuilderBase : MonoBehaviour
{
	//[MenuItem("File/Build Unity-Wwise Project")] 
	public static bool Build()
	{
		//Choose app name and location
		string appPath = EditorUtility.SaveFilePanel (	"Build Unity-Wwise project", 										//window title
		                                              	Application.dataPath.Remove(Application.dataPath.LastIndexOf('/')), //Default app location (unity project root directory)
		                                              	"Unity_Wwise_app", 													//Default app name
		                                              	getPlatFormExtension()												//app extension (depends on target platform)
		                                             );
		//check if the build was cancelled
		bool isUserCancelledBuild = appPath == "";
		if (isUserCancelledBuild)
		{
			UnityEngine.Debug.Log("Wwise: User cancelled the build.");
			return false;
		}

		//get Wwise platform string (the string isn't the same as unity for some platforms)
		RuntimePlatform PlatformToBuild = BuildTargetToRuntimePlatform(EditorUserBuildSettings.activeBuildTarget);

		//get soundbank location in the Wwise project for the current platform target
        List<string> platformNames = AkInitializer.GetInstance().BankDestinationFolders[(int)PlatformToBuild].Folders;
        List<string> DestinationFolders = new List<string>();
        foreach (string platformName in platformNames)
        {
            string sourceSoundBankFolder = AkWwisePlatformBuilder.GetGeneratedBankDirectory(platformName);

            //get soundbank destination in the Unity project for the current platform target
			string PathInStreamingAssets = Path.Combine(WwiseSetupWizard.Settings.SoundbankPath, platformName);
#if UNITY_EDITOR_OSX
			PathInStreamingAssets = PathInStreamingAssets.Replace('\\', '/');
#endif			
            string destinationSoundBankFolder = Path.Combine(Application.streamingAssetsPath, 								//Soundbank must be inside the StreamingAssets folder
                                                            PathInStreamingAssets
                                                                );

            DestinationFolders.Add(destinationSoundBankFolder);

            //Copy the soundbank from the Wwise project to the unity project (Inside the StreamingAssets folder as defined in Window->Wwise Settings)
            if (!AkUtilities.DirectoryCopy(sourceSoundBankFolder, 		//source folder
                                                destinationSoundBankFolder, //destination
                                                true						//copy subfolders
                                            )
               )
            {
                UnityEngine.Debug.LogError("Wwise: The soundbank folder for the " + platformName + " platform doesn't exist. Make sure it was generated in your Wwise project");
                return false;
            }
        }

		//Get all the scenes to build as defined in File->Build Settings
		string[] scenes = new string[EditorBuildSettings.scenes.Length]; 		
		for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)  
		{
			scenes[i] = EditorBuildSettings.scenes[i].path; 
		}

		//Build the app
		BuildPipeline.BuildPlayer	(	scenes,										//scenes to build 
		                           		appPath, 									//Location of the app to create
		                           		EditorUserBuildSettings.activeBuildTarget,	//Platform for which to build the app 
		                           		BuildOptions.None
		                           	);
		
		//Delete the soundbank from the unity project so they dont get copied in the game folder of fututre builds
        foreach (string destinationSoundBankFolder in DestinationFolders)
        {
		    Directory.Delete (destinationSoundBankFolder, true); 
        }
		
		return true;
	}


    private static RuntimePlatform BuildTargetToRuntimePlatform(BuildTarget TargetToBuild)
	{
        switch (TargetToBuild)
        {
            case BuildTarget.Android:
                return RuntimePlatform.Android;
#if UNITY_5
            case BuildTarget.iOS:
                return RuntimePlatform.IPhonePlayer;
#endif
#if !UNITY_5
            case BuildTarget.iPhone:
                return RuntimePlatform.IPhonePlayer;
#endif
#if !UNITY_5
            case BuildTarget.MetroPlayer:
                // There are multiple RuntimePlatforms for Metro, but they will all contain the same info in the platform array.
                return RuntimePlatform.MetroPlayerARM;
#endif
            case BuildTarget.PS3:
                return RuntimePlatform.PS3;
            case BuildTarget.PS4:
                return RuntimePlatform.PS4;
            case BuildTarget.PSP2:
                return RuntimePlatform.PSP2;
            case BuildTarget.StandaloneLinux:
            case BuildTarget.StandaloneLinux64:
            case BuildTarget.StandaloneLinuxUniversal:
                return RuntimePlatform.LinuxPlayer;
            case BuildTarget.StandaloneOSXIntel:
            case BuildTarget.StandaloneOSXIntel64:
            case BuildTarget.StandaloneOSXUniversal:
                return RuntimePlatform.OSXPlayer;
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return RuntimePlatform.WindowsPlayer;
            case BuildTarget.WP8Player:
                return RuntimePlatform.WP8Player;
#if UNITY_5
            case BuildTarget.WSAPlayer:
                // There are multiple RuntimePlatforms for WSA, but they will all contain the same info in the platform array.
                return RuntimePlatform.WSAPlayerARM;
#endif
            case BuildTarget.XBOX360:
                return RuntimePlatform.XBOX360;
            case BuildTarget.XboxOne:
                return RuntimePlatform.XboxOne;
            default:
                UnityEngine.Debug.LogError("Wwise Build: Build Target " + TargetToBuild.ToString() + " is not compatible with the Wwise integration");
                return (RuntimePlatform)(-1);
        }
	}

	private static string getPlatFormExtension()
	{
		string unityPlatormString = EditorUserBuildSettings.activeBuildTarget.ToString ();

		if(	unityPlatormString == BuildTarget.StandaloneWindows.ToString()
		   	||
		   	unityPlatormString == BuildTarget.StandaloneWindows64.ToString()
		   )
			return "exe";
		
		else if(	unityPlatormString == BuildTarget.StandaloneOSXIntel.ToString() 
		        	|| 
		        	unityPlatormString == BuildTarget.StandaloneOSXIntel64.ToString()
		        	||
		        	unityPlatormString == BuildTarget.StandaloneOSXUniversal.ToString()
		        )
			return "app";
		
#if UNITY_5
		else if(unityPlatormString == BuildTarget.iOS.ToString())
#else
		else if(unityPlatormString == BuildTarget.iPhone.ToString())
#endif
			return "ipa";
		
		else if(unityPlatormString == BuildTarget.XBOX360.ToString())
			return "XEX";
		else if(unityPlatormString == BuildTarget.Android.ToString())
			return "apk";
		else if(unityPlatormString == BuildTarget.PS3.ToString())
			return "self";		

		return "";
	}
	
}


#endif // #if UNITY_EDITOR