using System.Collections.Generic;
using System.IO;
using System.Linq;
using CI.QuickSave;
using UnityEngine;
using UnityEngine.SceneManagement;
using Eiko.YaSDK.Data;
using Sources.Data;

public static class PlayerData
{
    private static List<int> OpenedSkins = new ();
    
    public static int Level = 1;
    public static int Skin;
    public static int SkinOpenProgress;

    private static bool _loaded;
    
    public static bool SoundStatus = true;
    public static bool VibrationStatus = true;
    
    public static void TryLoad(SkinsAsset skins)
    {
        if(_loaded)
            return;

        #if UNITY_EDITOR
        var d = QuickSaveWriter.Create("Player");
        d.Delete("Level");
        d.Delete("Skins");
        d.Delete("Skin");
        d.Commit();
        #endif

        
        var reader = QuickSaveReader.Create("Player");
        Level = 1;
        if(Prefs.GetInt("LevelComplete")>0)
            Level = Prefs.GetInt("LevelComplete");
        //OpenedSkins = reader.ReadOrDefault("Skins", new List<int>());
        Skin = reader.ReadOrDefault("Skin", skins.Skins.First().GetHashCode());
        
        var sceneName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(Level - 1));
        Debug.Log(sceneName);
       // if(sceneName != null && sceneName.Contains("Level"))
         //   SceneManager.LoadScene(sceneName);
        //else
          //  SceneManager.LoadScene("Level Generator");
        
    }

    public static void Save()
    {
        var writer = QuickSaveWriter.Create("Player");
        Prefs.SetInt("LevelComplete",Level);
        writer.Write("Skins", OpenedSkins);
        writer.Write("Skin", Skin);
        writer.Commit();
        
        Debug.Log("Save");
    }
    
    public static void OpenSkin(SkinAsset skin)
    {
        if(IsOpened(skin))
            return;
        
        OpenedSkins.Add(skin.GetHashCode());
    }
    
    public static bool IsOpened(SkinAsset skin) => OpenedSkins.Contains(skin.GetHashCode()) || skin.AlwaysOpened;
    
}