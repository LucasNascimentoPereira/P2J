using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveGame(GameManager gameManager)//all the managers and stuff
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/BlopFlipper.ml";
        FileStream stream = new FileStream(path, FileMode.Create);

        GameData gameData = new GameData(gameManager);//all the managers and stuff

        formatter.Serialize(stream, gameData);
        stream.Close();
    }

    public static GameData LoadGame()
    {
        string path = Application.persistentDataPath + "/BlopFlipper.ml";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            GameData gameData = formatter.Deserialize(stream) as GameData;
            stream.Close();
            return gameData;
        }
        else
        {
            //Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
