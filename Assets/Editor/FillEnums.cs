using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class FillEnums 
{
    
    
    
    [MenuItem("OWG/Fill SFX Enums")]
    static void FillSfxEnums()
    {
        string audioPath = Application.dataPath + "/Audio/SFX/";
        List<string> fileEntries = Directory.GetFiles(audioPath).ToList();

        for (int i = 0; i < fileEntries.Count; i++)
        {
            if (fileEntries[i].Contains(".meta") == true)
            {
                fileEntries.Remove(fileEntries[i]);
            }
        }
        
        for (int i = 0; i < fileEntries.Count; i++)
        {
            fileEntries[i] = fileEntries[i].Replace(audioPath, "").Replace(".mp3", "");
        }

        FillEnum("AudioEnums", fileEntries, "//S-S", "//S-E");
    }
    
    
    [MenuItem("OWG/Create Scriptable Objects")]
    static void CreateScriptableObjects()
    {
        string audioPath = Application.dataPath + "/Audio/SFX/";
        List<string> fileEntries = Directory.GetFiles(audioPath).ToList();

        for (int i = 0; i < fileEntries.Count; i++)
        {
            if (fileEntries[i].Contains(".meta") == true)
            {
                fileEntries.Remove(fileEntries[i]);
            }
        }
        
        for (int i = 0; i < fileEntries.Count; i++)
        {
            fileEntries[i] = fileEntries[i].Replace(audioPath, "").Replace(".mp3", "");
        }

        CreatSO(fileEntries, "Assets/ScriptableObjects/SFX/", "Assets/Audio/SFX/");
        
        AssetDatabase.Refresh();
    }


    private static void CreatSO(List<string> fileEntries, string SOPath, string DOPath)
    {
        for (int i = 0; i < fileEntries.Count; i++)
        {
            var exists = AssetDatabase.LoadAssetAtPath(SOPath + fileEntries[i] + ".asset", typeof(SoundShell));

            if (exists == null)
            {   
                string scriptablePath = SOPath + fileEntries[i] + ".asset";
                
                SoundShell soundShell = ScriptableObject.CreateInstance<SoundShell>();
                AudioClip ac =  AssetDatabase.LoadAssetAtPath(DOPath + fileEntries[i] + ".mp3", typeof(AudioClip)) as AudioClip;
                soundShell.AudioCLip = ac;
                
                AssetDatabase.CreateAsset(soundShell, scriptablePath);
                AssetDatabase.SaveAssets();
                
            }
        }
    }

    private static  void FillEnum(string fileName, List<string> enumNames, string startSymbol, string endSymbol)
    {
        string[] audioEnumsScript = AssetDatabase.FindAssets(fileName, new[] {"Assets/Scripts/Generated"});

        foreach (string guid2 in audioEnumsScript)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid2);
            string contents = File.ReadAllText(path);

            
            string s = contents;
            int start = s.IndexOf(startSymbol, StringComparison.Ordinal);
            int stop = s.IndexOf(endSymbol, StringComparison.Ordinal);

            int diff = stop - start;
            string substringToReplace = s.Substring(start, diff + endSymbol.Length);


            string enums = startSymbol + Environment.NewLine;

            enums += "None, ";
            for (int i = 0; i < enumNames.Count; i++)
            {
                enums += enumNames[i];

                if (i < enumNames.Count - 1)
                {
                    enums += ", ";
                }
            }

            enums += Environment.NewLine;
            enums += endSymbol;

            string toReplace = s.Replace(substringToReplace, enums);
            File.WriteAllText(path ,toReplace);
        }
    }
}
