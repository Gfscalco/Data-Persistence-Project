using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public int[] highscore;
    public string[] names;
    public string playerName;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHighscore();
    }

    [System.Serializable]
    class SaveData
    {
        public int[] highscore;
        public string[] names;
    }

    public void SaveHighscore()
    {
        SaveData data = new SaveData();
        data.highscore = highscore;
        data.names = names;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighscore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highscore = data.highscore;
            names = data.names;
        }
        else
        {
            highscore = new int[5] { 0, 0, 0, 0, 0 };
            names = new string[5] {"name1", "name2", "name3", "name4", "name5" };
        }
    }
}
