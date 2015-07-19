using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;

public class AkWwisePlatformBuilder
{
    public class EditorPlatformFolderComparer : IComparer<string>
    {
        // Return in inverse order, because we want to sort desending.
        public int Compare(string a, string b)
        {
            try
            {
                FileInfo InitInfoA = new FileInfo(Path.Combine(a, "Init.bnk"));
                FileInfo InitInfoB = new FileInfo(Path.Combine(b, "Init.bnk"));
                if (InitInfoA.Exists && InitInfoB.Exists)
                {
                    return (-1) * InitInfoA.LastWriteTime.CompareTo(InitInfoB.LastWriteTime);
                }
                else if (InitInfoA.Exists && !InitInfoB.Exists)
                {
                    return -1;
                }
                else if (!InitInfoA.Exists && InitInfoB.Exists)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
    }

    public static void BuildPlatformArray()
    {
        AkInitializer[] AkInitializers = UnityEngine.Object.FindObjectsOfType(typeof(AkInitializer)) as AkInitializer[];
        if (AkInitializers.Length > 0)
        {
            WwiseSettings Settings = WwiseSettings.LoadSettings();
            string WwiseProjectFullPath = AkUtilities.GetFullPath(Application.dataPath, Settings.WwiseProjectPath);
            AkInitializers[0].BankDestinationFolders = AkWwisePlatformBuilder.GetWwisePlatformNames(WwiseProjectFullPath);
            AkWwiseProjectInfo.GetData().SaveInitSettings(AkInitializers[0]);
        }
    }

    public static AkInitializer.PlatformFolderContainer[] GetWwisePlatformNames(string WwiseProjectPath)
    {
        AkInitializer.PlatformFolderContainer[] BankDestinationFolders = new AkInitializer.PlatformFolderContainer[AkInitializer.HighestRuntimePlatformNumber];
        if (WwiseProjectPath.Length == 0)
        {
            return BankDestinationFolders;
        }

		XmlDocument doc = new XmlDocument();
		XPathNavigator WwiseProjectNavigator;
        try
        {
			doc.Load(WwiseProjectPath);
			WwiseProjectNavigator = doc.CreateNavigator();
		}
		catch(Exception)
		{
			Debug.Log("WwiseUnity: Unable to read project file.");
			return BankDestinationFolders;
		}
		
		try
		{
            string RefPlatformPathExpression = "//Platforms";
            XPathExpression RefPlatformExpression = XPathExpression.Compile(RefPlatformPathExpression);
            XPathNavigator RefPathNav = WwiseProjectNavigator.SelectSingleNode(RefPlatformExpression);
            if (RefPathNav != null && RefPathNav.HasChildren)
            {
                RefPathNav.MoveToFirstChild();
                do
                {
                    if (RefPathNav.Name == "Platform")
                    {
                        string currentPlatformName = RefPathNav.GetAttribute("Name", "");
                        string currentRefPlatform = RefPathNav.GetAttribute("ReferencePlatform", "");
                        AddPlatformToList(ref BankDestinationFolders, currentPlatformName, currentRefPlatform, WwiseProjectNavigator, WwiseProjectPath);
                    }
                }
                while (RefPathNav.MoveToNext());
            }
        }
        catch (Exception)
        {
            // Left empty, simply return the empty list.
			Debug.Log("WwiseUnity: Wwise project version does not match the Wwise Unity integration version. Please migrate your Wwise project.");
        }

        // Sort the platforms
        for (int i = 0; i < BankDestinationFolders.Length; i++)
        {
            if (BankDestinationFolders[i] != null)
            {
                if (i == (int)RuntimePlatform.OSXEditor || i == (int)RuntimePlatform.WindowsEditor)
                {
                    // Sort by SoundBank generation date for Editor platforms
                    BankDestinationFolders[i].Folders.Sort(new EditorPlatformFolderComparer());
                }
                else
                {
                    // Sort alphabetically for non-editor platforms
                    BankDestinationFolders[i].Folders.Sort();
                }
            }
        }

        return BankDestinationFolders;
    }

    static void AddPlatformToList(ref AkInitializer.PlatformFolderContainer[] BankDestinationFolders, string PlatformName, string RefPlatform, XPathNavigator WwiseProjectNavigator, string WwiseProjectPath)
    {
        List<int> PlatformNumbers = new List<int>();
        switch(RefPlatform)
        {
            case "Android":
                PlatformNumbers.Add((int)RuntimePlatform.Android);
                break;
            case "iOS":
                PlatformNumbers.Add((int)RuntimePlatform.IPhonePlayer);
                break;
            case "Linux":
                PlatformNumbers.Add((int)RuntimePlatform.LinuxPlayer);
                break;
            case "Mac":
                PlatformNumbers.Add((int)RuntimePlatform.OSXPlayer);
                PlatformNumbers.Add((int)RuntimePlatform.OSXEditor);
                break;
            case "PS3":
                PlatformNumbers.Add((int)RuntimePlatform.PS3);
                break;
            case "PS4":
                PlatformNumbers.Add((int)RuntimePlatform.PS4);
                break;
            case "VitaHW":
            case "VitaSW":
                PlatformNumbers.Add((int)RuntimePlatform.PSP2);
                break;
#if !UNITY_5				
            case "WiiUSW":
                PlatformNumbers.Add(28); // RuntimePlatform.WiiU is only available in the special WiiU only build. Use its numerical value here...
                break;
#endif
            case "Windows":
                PlatformNumbers.Add((int)RuntimePlatform.WindowsPlayer);
                PlatformNumbers.Add((int)RuntimePlatform.WindowsEditor);
                
                // Windows Store uses the same banks as Windows
#if UNITY_5
                PlatformNumbers.Add((int)RuntimePlatform.WSAPlayerARM);
                PlatformNumbers.Add((int)RuntimePlatform.WSAPlayerX64);
                PlatformNumbers.Add((int)RuntimePlatform.WSAPlayerX86);
#else
                PlatformNumbers.Add((int)RuntimePlatform.MetroPlayerARM);
                PlatformNumbers.Add((int)RuntimePlatform.MetroPlayerX64);
                PlatformNumbers.Add((int)RuntimePlatform.MetroPlayerX86);
#endif
                break;
            case "WindowsPhone":
                PlatformNumbers.Add((int)RuntimePlatform.WP8Player);
                break;
            case "Xbox360":
                PlatformNumbers.Add((int)RuntimePlatform.XBOX360);
                break;
            case "XboxOne":
                PlatformNumbers.Add((int)RuntimePlatform.XboxOne);
                break;
            default:
                break;
        }

        foreach(int PlatformNumber in PlatformNumbers)
        {
            if (BankDestinationFolders[PlatformNumber] == null)
            {
                BankDestinationFolders[PlatformNumber] = new AkInitializer.PlatformFolderContainer();
            }

            // Editor platforms use the SoundbankPaths info from the .wproj directly
            if (PlatformNumber == (int)RuntimePlatform.OSXEditor || PlatformNumber == (int)RuntimePlatform.WindowsEditor)
            {
                AddEditorPlatformToList(ref BankDestinationFolders, PlatformNumber, PlatformName, WwiseProjectNavigator, WwiseProjectPath);
                continue;
            }

            BankDestinationFolders[PlatformNumber].Folders.Add(PlatformName);
        }
    }

    public static string GetGeneratedBankDirectory(string PlatformName)
    {
        WwiseSettings Settings = WwiseSettings.LoadSettings();
        string WwiseProjectFullPath = AkUtilities.GetFullPath(Application.dataPath, Settings.WwiseProjectPath);
        XmlDocument doc = new XmlDocument();
        doc.Load(WwiseProjectFullPath);
        XPathNavigator WwiseProjectNavigator = doc.CreateNavigator();

        return GetGeneratedBankDirectory(PlatformName, WwiseProjectNavigator, WwiseProjectFullPath);
    }

    public static string GetGeneratedBankDirectory(string PlatformName, XPathNavigator WwiseProjectNavigator, string WwiseProjectPath)
    {
		string CurrentPath = string.Empty;
		try
		{
			string BankFolderPathExpression = string.Format("//Property[@Name='SoundBankPaths']/ValueList/Value[@Platform='{0}']", PlatformName);
			XPathExpression BankFolderExpression = XPathExpression.Compile(BankFolderPathExpression);
			XPathNavigator BankFolderNode = WwiseProjectNavigator.SelectSingleNode(BankFolderExpression);
			if (BankFolderNode != null)
			{
				CurrentPath = BankFolderNode.Value;
				ConvertToPosixPath(ref CurrentPath);

				// Find the folder with the latest SoundBanks
				if (Path.GetPathRoot(CurrentPath) == "")
				{
					// Path is relative, make it full
					CurrentPath = AkUtilities.GetFullPath(Path.GetDirectoryName(WwiseProjectPath), CurrentPath);
				}
			}
		}
		catch (Exception)
		{
			// Left empty, return empty path.
			Debug.Log("WwiseUnity: Unable to get SoundBank paths from Wwise project for the " + PlatformName + " platform.");
		}

        return CurrentPath;
    }

    static void AddEditorPlatformToList(ref AkInitializer.PlatformFolderContainer[] BankDestinationFolders, int EditorPlatform, string PlatformName, XPathNavigator WwiseProjectNavigator, string WwiseProjectPath)
    {
        string CurrentPath = GetGeneratedBankDirectory(PlatformName, WwiseProjectNavigator, WwiseProjectPath);
        if (!string.IsNullOrEmpty(CurrentPath))
        {
            BankDestinationFolders[EditorPlatform].Folders.Add(CurrentPath);
        }
    }
	
	public static void ConvertToPosixPath(ref string path)
    {
#if !(UNITY_EDITOR_WIN || UNITY_XBOX360 || UNITY_XBOXONE || UNITY_METRO)
        path.Trim();
        path = path.Replace("\\", "/");
        path = path.TrimStart('\\');
#endif // #if !(UNITY_EDITOR_WIN || UNITY_XBOX360 || UNITY_XBOXONE || UNITY_METRO)
    }

}