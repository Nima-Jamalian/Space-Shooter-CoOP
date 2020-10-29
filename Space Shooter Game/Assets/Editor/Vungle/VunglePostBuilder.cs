using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

public class VunglePostBuilder
{
	private static string PostBuildDirectoryKey { get { return "VunglePostBuildPath-" + PlayerSettings.productName; } }
	private static string PostBuildDirectory
	{
		get
		{
			return EditorPrefs.GetString(PostBuildDirectoryKey);
		}
		set
		{
			EditorPrefs.SetString(PostBuildDirectoryKey, value);
		}
	}

	[PostProcessBuild(800)]
	private static void OnPostProcessBuildPlayer(BuildTarget target, string pathToBuiltProject)
	{
		switch (target)
		{
			case BuildTarget.iOS:
				PostBuildDirectory = pathToBuiltProject;
				PostProcessIosBuild(pathToBuiltProject);
				break;
		}
	}

	private static void PostProcessIosBuild(string pathToBuiltProject)
	{
		UnityEditor.iOS.Xcode.PBXProject project = new UnityEditor.iOS.Xcode.PBXProject();
		string pbxPath = UnityEditor.iOS.Xcode.PBXProject.GetPBXProjectPath(pathToBuiltProject);
		project.ReadFromFile(pbxPath);

#if UNITY_2019_3_OR_NEWER
		string targetId = project.GetUnityFrameworkTargetGuid();
#else
		string targetId = project.TargetGuidByName(UnityEditor.iOS.Xcode.PBXProject.GetUnityTargetName());
#endif

		project.AddFrameworkToProject(targetId, "AdSupport.framework", false);
		project.AddFrameworkToProject(targetId, "CoreTelephony.framework", false);
		project.AddFrameworkToProject(targetId, "StoreKit.framework", false);
		project.AddFrameworkToProject(targetId, "WebKit.framework", false);

		project.AddFileToBuild(targetId, project.AddFile("usr/lib/libsqlite3.dylib", "Frameworks/libsqlite3.dylib", UnityEditor.iOS.Xcode.PBXSourceTree.Sdk));
		project.AddFileToBuild(targetId, project.AddFile("usr/lib/libz.1.1.3.dylib", "Frameworks/libz.1.1.3.dylib", UnityEditor.iOS.Xcode.PBXSourceTree.Sdk));

		project.AddBuildProperty(targetId, "OTHER_LDFLAGS", "-ObjC");

		project.WriteToFile(pbxPath);

		Debug.Log("Vungle iOS post processor completed.");
	}

	[UnityEditor.MenuItem("Tools/Vungle/Open Documentation Website...")]
	static void DocumentationSite()
	{
		UnityEditor.Help.BrowseURL("https://support.vungle.com/hc/en-us/articles/360003455452-Get-Started-with-Vungle-SDK-v-6-Unity#add-the-vungle-unity-plugin-to-your-unity-project-0-0");
	}

	[UnityEditor.MenuItem("Tools/Vungle/Run iOS Post Processor")]
	static void RunPostBuilder()
	{
		OnPostProcessBuildPlayer(BuildTarget.iOS, PostBuildDirectory);
	}

	[UnityEditor.MenuItem("Tools/Vungle/Run iOS Post Processor", true)]
	static bool ValidateRunPostBuilder()
	{
		var iPhoneProjectPath = PostBuildDirectory;
		if (iPhoneProjectPath == null || !Directory.Exists(iPhoneProjectPath))
			return false;

		var projectFile = Path.Combine(iPhoneProjectPath, "Unity-iPhone.xcodeproj/project.pbxproj");
		if (!File.Exists(projectFile))
			return false;

		return true;
	}

	//https://docs.unity3d.com/ScriptReference/EditorUserBuildSettings.SwitchActiveBuildTarget.html
	[MenuItem("Tools/Vungle/Switch Platform - Android")]
	public static void PerformSwitchAndroid()
	{
		// Switch to Android build.
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
	}

	[MenuItem("Tools/Vungle/Switch Platform - iOS")]
	public static void PerformSwitchiOS()
	{
		// Switch to iOS build.
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);
	}

	[MenuItem("Tools/Vungle/Switch Platform - Windows")]
	public static void PerformSwitchWindows()
	{
		// Switch to UWP build.
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WSA, BuildTarget.WSAPlayer);
	}
}
