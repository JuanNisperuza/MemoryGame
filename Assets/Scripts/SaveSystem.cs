using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static void Save(int total_clicks, int total_time, int pairs, int score)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.alternova";
        FileStream stream = new FileStream(path, FileMode.Create);
        Results data = new Results(total_clicks, total_time, pairs, score);
        Debug.Log("Saved data: " + data);
        bf.Serialize(stream, data);
        stream.Close();
    }

    public static Results Load()
    {
        string path = Application.persistentDataPath + "/save.alternova";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            return formatter.Deserialize(stream) as Results;
        }
        else
        {
            Debug.LogWarning("Save file not found or not exists in " + path);
            return null;
        }
    }
}
