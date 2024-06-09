using UnityEngine;
using System.IO;

public static class SaveSystem
{
    public static void Save(int total_clicks, int total_time, int pairs, int score)
    {
        Results data = new Results(total_clicks, total_time, pairs, score);
        string json = JsonUtility.ToJson(data);
        string path = Application.persistentDataPath + "/save.alternova";
        Debug.Log("Saved data in: " + path);
        File.WriteAllText(path, json);
    }

    public static Results Load()
    {
        string path = Application.persistentDataPath + "/save.alternova";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Results data = JsonUtility.FromJson<Results>(json);
            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found or not exists in " + path);
            return null;
        }
    }
}
