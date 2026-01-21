using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager
{

    private static SaveData saveData = new SaveData();


    [System.Serializable]
    public struct SaveData
    {
        public GameData GameData;
    }

    private static string SavePath()
    {
        return Application.persistentDataPath + "/OtherHalfSave" + ".oth";
    }

    public static void Save()
    {
        GameManager.Instance.Save(ref saveData.GameData);
        File.WriteAllText(SavePath(), JsonUtility.ToJson(saveData, true));
    }

    public static void Load() 
    {
	if (!File.Exists(SavePath())) return;	
        string saveContent = File.ReadAllText(SavePath());
        saveData = JsonUtility.FromJson<SaveData>(saveContent);
        GameManager.Instance.Load(saveData.GameData);
    }


}
