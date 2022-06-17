using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildAutomation
{
  public struct BuildTargetAndGroup
  {
    public BuildTargetGroup group;
    public BuildTarget target;
    public BuildTargetAndGroup(BuildTargetGroup group, BuildTarget target)
    {
      this.group = group;
      this.target = target;
    }
  }

  [MenuItem("My Project/Build/Build all")]
  public static void BuildAll()
  {
    string path = "build";
    BuildApplication(path);
  }
  public static void BuildApplication(string path)
  {
    string appName = "MediaWall.exe";
    PlayerSettings.productName = "MediaWall";
    PlayerSettings.applicationIdentifier = "com.myproject.application";
    PlayerSettings.defaultScreenWidth = 960;
    PlayerSettings.defaultScreenHeight = 960;
    PlayerSettings.defaultIsFullScreen = true;
    PlayerSettings.displayResolutionDialog = ResolutionDialogSetting.HiddenByDefault;
    //...

    BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
    buildPlayerOptions.scenes = new[] { "Assets/Scenes/MainScene.unity" };
    buildPlayerOptions.locationPathName = path;
    buildPlayerOptions.options = BuildOptions.None;

    BuildForTargetes(buildPlayerOptions, appName,
      new BuildTargetAndGroup(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64),
      new BuildTargetAndGroup(BuildTargetGroup.Standalone, BuildTarget.StandaloneLinux64));
  }

  public static void BuildForTargetes(BuildPlayerOptions options, string appName, params BuildTargetAndGroup[] targets)
  {
    string locationPathName = options.locationPathName;
    foreach (BuildTargetAndGroup target in targets)
    {

      // https://forum.unity.com/threads/cant-change-resolution-for-standalone-build.323931/
      PlayerSettings.SetApplicationIdentifier(target.group, PlayerSettings.applicationIdentifier);
      DeletePreference();

      options.targetGroup = target.group;
      options.target = target.target;
      options.locationPathName = locationPathName + "/" + target.target.ToString() + "/" + appName;
      //Debug.Log("building " + options.locationPathName);
      BuildPipeline.BuildPlayer(options);
    }
  }
  public static void DeletePreference()
  {
    if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXEditor)
    {
      FileUtil.DeleteFileOrDirectory("~/Library/Preferences/" + PlayerSettings.applicationIdentifier + ".plist");
    }
    else
    {
      Debug.LogWarning("Not sure how to delete Prefferences for this platform!");
      // https://forum.unity.com/threads/cant-change-resolution-for-standalone-build.323931/
    }
  }
}