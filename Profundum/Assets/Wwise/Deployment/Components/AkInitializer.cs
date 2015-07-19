#if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.
//////////////////////////////////////////////////////////////////////
//
// Copyright (c) 2012 Audiokinetic Inc. / All Rights Reserved
//
//////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
#pragma warning disable 0219, 0414

[AddComponentMenu("Wwise/AkInitializer")]
/// This script deals with initialization, and frame updates of the Wwise audio engine.  
/// It is marked as \c DontDestroyOnLoad so it stays active for the life of the game, 
/// not only one scene. You can, and probably should, modify this script to change the 
/// initialization parameters for the sound engine. A few are already exposed in the property inspector.
/// It must be present on one Game Object at the beginning of the game to initialize the audio properly.
/// It must be executed BEFORE any other MonoBehaviors that use AkSoundEngine.
/// \sa
/// - \ref workingwithsdks_initialization
/// - AK::SoundEngine::Init()
/// - AK::SoundEngine::Term()
/// - AkCallbackManager
[RequireComponent(typeof(AkTerminator))]
public class AkInitializer : MonoBehaviour
{
    public readonly static string c_DefaultBasePath = Path.Combine("Audio", "GeneratedSoundBanks");
    ///Path for the soundbanks. This must contain one sub folder per platform, with the same as in the Wwise project.
    public string basePath = c_DefaultBasePath;

    public const string c_Language = "English(US)";
    /// Language sub-folder. 
    public string language = c_Language;

    public const int c_DefaultPoolSize = 4096;
    ///Default Pool size.  This contains the meta data for your audio project.  Default size is 4 MB, but you should adjust for your needs.
    public int defaultPoolSize = c_DefaultPoolSize;

    public const int c_LowerPoolSize = 2048;
    ///Lower Pool size.  This contains the audio processing buffers and DSP data.  Default size is 2 MB, but you should adjust for your needs.
    public int lowerPoolSize = c_LowerPoolSize;

    public const int c_StreamingPoolSize = 1024;
    ///Streaming Pool size.  This contains the streaming buffers.  Default size is 1 MB, but you should adjust for your needs.
    public int streamingPoolSize = c_StreamingPoolSize;

    public const float c_MemoryCutoffThreshold = 0.95f;
    ///This setting will trigger the killing of sounds when the memory is reaching 95% of capacity.  Lowest priority sounds are killed.
    public float memoryCutoffThreshold = c_MemoryCutoffThreshold;

    // Need the +1 at the end for WiiU.....
    public readonly static int HighestRuntimePlatformNumber = (int)Enum.GetValues(typeof(RuntimePlatform)).GetValue(Enum.GetValues(typeof(RuntimePlatform)).Length - 1) + 1;

    [System.Serializable]
    public class PlatformFolderContainer
    {
        public List<string> Folders = new List<string>();
    };

    /// List of SoundBank sub-folders per RuntimePlatform (indexable using Application.platform).
    public PlatformFolderContainer[] BankDestinationFolders = new PlatformFolderContainer[HighestRuntimePlatformNumber];

    public static AkInitializer GetInstance()
    {
#if UNITY_EDITOR
        if (EditorApplication.isPlaying)
        {
            return ms_Instance;
        }
        else
        {
            AkInitializer initializer = (AkInitializer)FindObjectOfType(typeof(AkInitializer));
            return initializer;
        }
#else
        return ms_Instance;
#endif
    }

    public static string GetBasePath()
    {
        AkInitializer instance = GetInstance();
        if (instance != null)
        {
            return instance.basePath;
        }
        else
        {
            return string.Empty;
        }
    }

    public static string GetCurrentLanguage()
    {
        return ms_Instance.language;
    }

    void Awake()
    {
        if (ms_Instance != null)
        {
            //Don't init twice
            //Check if there are 2 objects with this script.  If yes, remove this component.
            if (ms_Instance != this)
                UnityEngine.Object.DestroyImmediate(this.gameObject);
            return;
        }

        Debug.Log("WwiseUnity: Initialize sound engine ...");

        //Use default properties for most SoundEngine subsystem.  
        //The game programmer should modify these when needed.  See the Wwise SDK documentation for the initialization.
        //These settings may very well change for each target platform.
        AkMemSettings memSettings = new AkMemSettings();
        memSettings.uMaxNumPools = 20;

        AkDeviceSettings deviceSettings = new AkDeviceSettings();
        AkSoundEngine.GetDefaultDeviceSettings(deviceSettings);

        AkStreamMgrSettings streamingSettings = new AkStreamMgrSettings();
        streamingSettings.uMemorySize = (uint)streamingPoolSize * 1024;

        AkInitSettings initSettings = new AkInitSettings();
        AkSoundEngine.GetDefaultInitSettings(initSettings);
        initSettings.uDefaultPoolSize = (uint)defaultPoolSize * 1024;

        AkPlatformInitSettings platformSettings = new AkPlatformInitSettings();
        AkSoundEngine.GetDefaultPlatformInitSettings(platformSettings);
        platformSettings.uLEngineDefaultPoolSize = (uint)lowerPoolSize * 1024;
        platformSettings.fLEngineDefaultPoolRatioThreshold = memoryCutoffThreshold;

        AkMusicSettings musicSettings = new AkMusicSettings();
        AkSoundEngine.GetDefaultMusicSettings(musicSettings);

// Unity 5 only, UNity 4 doesn't provide a way to access the product name at runtime.
#if UNITY_5
#if UNITY_EDITOR
        AkSoundEngine.SetGameName(Application.productName + " (Editor)");
#else
		AkSoundEngine.SetGameName(Application.productName);
#endif
#endif

        AKRESULT result = AkSoundEngine.Init(memSettings, streamingSettings, deviceSettings, initSettings, platformSettings, musicSettings);
        if (result != AKRESULT.AK_Success)
        {
            Debug.LogError("WwiseUnity: Failed to initialize the sound engine. Abort.");
            return; //AkSoundEngine.Init should have logged more details.
        }

        ms_Instance = this;

        if (!ValidateAndSetBasePath())
        {
            return;
        }

        AkSoundEngine.SetCurrentLanguage(language);

        result = AkCallbackManager.Init();
        if (result != AKRESULT.AK_Success)
        {
            Debug.LogError("WwiseUnity: Failed to initialize Callback Manager. Terminate sound engine.");
            AkSoundEngine.Term();
            ms_Instance = null;
            return;
        }

        AkBankManager.Reset ();

        Debug.Log("WwiseUnity: Sound engine initialized.");

        //The sound engine should not be destroyed once it is initialized.
        DontDestroyOnLoad(this);

#if UNITY_EDITOR
        //Redirect Wwise error messages into Unity console.
        AkCallbackManager.SetMonitoringCallback(ErrorLevel.ErrorLevel_All, CopyMonitoringInConsole);
#endif

        //Load the init bank right away.  Errors will be logged automatically.
        uint BankID;
        result = AkSoundEngine.LoadBank("Init.bnk", AkSoundEngine.AK_DEFAULT_POOL_ID, out BankID);
        if (result != AKRESULT.AK_Success)
        {
            Debug.LogError("WwiseUnity: Failed load Init.bnk with result: " + result.ToString());
        }
    }


    /// Returns the full base path
    public static string GetPlatformBasePath()
    {
        AkInitializer instance = GetInstance();
        string basePathToReturn = string.Empty;
		try
		{
			if (instance != null )
			{
				List<string> PossibleBaseFolders = instance.BankDestinationFolders[(int)Application.platform].Folders;
#if UNITY_EDITOR
				// Also look in the StreamingAssets folder, in case the banks in the Wwise destination folder
				// are not found.
				int AdditionalPlatformIndex = 0;
#if UNITY_EDITOR_WIN
				AdditionalPlatformIndex = (int)RuntimePlatform.WindowsPlayer;
#elif UNITY_EDITOR_OSX
				AdditionalPlatformIndex = (int)RuntimePlatform.OSXPlayer;
#endif
				List<string> AdditionalFolders = instance.BankDestinationFolders[AdditionalPlatformIndex].Folders;
				foreach (string AdditionalPath in AdditionalFolders)
				{
					string FullPath = Path.Combine(Path.Combine(Application.streamingAssetsPath, instance.basePath), AdditionalPath);
					PossibleBaseFolders.Add(FullPath);
				}
#endif
				foreach (string baseFolder in PossibleBaseFolders)
				{
					string InitBankPath = Path.Combine(baseFolder, "Init.bnk");
#if !UNITY_EDITOR
					// For the Editor, the BankDestinationFolders contains a full path, but for the other platforms, it only contains the sub-folder.
					// We need to get the full path to the banks in Streaming Assets.
#if UNITY_ANDROID
					basePathToReturn = Path.Combine(instance.basePath, baseFolder) + "/";
					basePathToReturn = basePathToReturn.Replace('\\', '/');
#else
					InitBankPath = Path.Combine(Path.Combine(Application.streamingAssetsPath, instance.basePath), InitBankPath);
#endif
#endif
					// Sanitize slashes
#if UNITY_METRO && !UNITY_EDITOR
            	    // Metro does not have Path.DirectorySeperatorChar...
            	    InitBankPath = InitBankPath.Replace('/', '\\');
#else
					if (Path.DirectorySeparatorChar == '/')
					{
						InitBankPath = InitBankPath.Replace('\\', Path.DirectorySeparatorChar);
					}
					else
					{
						InitBankPath = InitBankPath.Replace('/', Path.DirectorySeparatorChar);
					}
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
					// Can't use System.IO on Android, we need to assume SoundBanks are there...
					break;
#else
					if (File.Exists(InitBankPath))
					{
						basePathToReturn = InitBankPath.Remove(InitBankPath.IndexOf("Init.bnk"), "Init.bnk".Length);
						break;
					}
#endif
				}
			}
		}
		catch(Exception)
		{
			basePathToReturn = string.Empty;
		}

        return basePathToReturn;
    }

    // This looks for a valid SoundBank path for all Custom platforms for the current base platform.
    // If you are using custom platforms, you probably will need to modify the code to this function.
    private bool ValidateAndSetBasePath()
    {
        string basePathToSet = GetPlatformBasePath();
        if (basePathToSet == string.Empty)
        {
#if !UNITY_EDITOR
            Debug.LogError("WwiseUnity: Could not locate the SoundBanks. Did you make sure to copy them to the StreamingAssets folder?");
#else
            Debug.LogError("WwiseUnity: Could not locate the SoundBanks. Did you make sure to generate them?");
#endif
            return false;
        }

        Debug.Log("WwiseUnity: Setting base SoundBank path to " + basePathToSet);
        AkSoundEngine.SetBasePath(basePathToSet);

        return true;
    }

    void OnDestroy()
    {
        if (ms_Instance == this)
        {
            AkCallbackManager.SetMonitoringCallback(0, null);
            ms_Instance = null;
        }
        // Do nothing. AkTerminator handles sound engine termination.
    }

    void OnEnable()
    {
        //The sound engine was not terminated normally.  Make this instance the one that will manage
        //the updates and termination.
        //This happen when Unity resets everything when a script changes.
        if (ms_Instance == null && AkSoundEngine.IsInitialized())
        {
            ms_Instance = this;
#if UNITY_EDITOR
            //Redirect Wwise error messages into Unity console.
            AkCallbackManager.SetMonitoringCallback(ErrorLevel.ErrorLevel_All, CopyMonitoringInConsole);
#endif
        }
    }

    //Use LateUpdate instead of Update() to ensure all gameobjects positions, listener positions, environements, RTPC, etc are set before finishing the audio frame.
    void LateUpdate()
    {
        //Execute callbacks that occured in last frame (not the current update)     
        if (ms_Instance != null)
        {
            AkCallbackManager.PostCallbacks();
            AkBankManager.DoUnloadBanks();
            AkSoundEngine.RenderAudio();
        }
    }

#if UNITY_EDITOR

    void OnDisable()
    {
        // Unregister the callback that redirects the output to the Unity console.  If not done early enough (meaning, later than Disable), AkInitializer will leak.
        if (ms_Instance != null && AkSoundEngine.IsInitialized())
            AkCallbackManager.SetMonitoringCallback(0, null);
    }


    void CopyMonitoringInConsole(ErrorCode in_errorCode, ErrorLevel in_errorLevel, uint in_playingID, IntPtr in_gameObjID, string in_msg)
    {
        string msg = "Wwise: " + in_msg;
        if (in_gameObjID != (IntPtr)AkSoundEngine.AK_INVALID_GAME_OBJECT)
        {
            GameObject obj = EditorUtility.InstanceIDToObject((int)in_gameObjID) as GameObject;
            string name = obj != null ? obj.ToString() : in_gameObjID.ToString();
            msg += "(Object: " + name + ")";
        }

        if (in_errorLevel == ErrorLevel.ErrorLevel_Error)
            Debug.LogError(msg);
        else
            Debug.Log(msg);
    }
#endif

    //
    // Private members
    //

    private static AkInitializer ms_Instance;

#if !UNITY_EDITOR && !UNITY_WIIU
	//On the WiiU, it seems Unity has a bug and never calls OnApplicationFocus(true).  This leaves us in "suspended mode".  So commented out for now.
	void OnApplicationPause(bool pauseStatus) 
	{
		if (ms_Instance != null)
		{
			if ( !pauseStatus )
			{
				AkSoundEngine.WakeupFromSuspend();
			}
			else
			{
				AkSoundEngine.Suspend();             
			}
			AkSoundEngine.RenderAudio();
		}        
	}
	
    void OnApplicationFocus(bool focus)
    {
        if (ms_Instance != null)
        {
            if ( focus )
            {
                AkSoundEngine.WakeupFromSuspend();
            }
            else
            {
				AkSoundEngine.Suspend();             
            }
            AkSoundEngine.RenderAudio();
        }
    }
#endif

}
#endif // #if ! (UNITY_DASHBOARD_WIDGET || UNITY_WEBPLAYER || UNITY_WII || UNITY_NACL || UNITY_FLASH || UNITY_BLACKBERRY) // Disable under unsupported platforms.