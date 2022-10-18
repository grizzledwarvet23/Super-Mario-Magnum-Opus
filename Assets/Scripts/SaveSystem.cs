using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class SaveSystem : MonoBehaviour
{
    public static SaveData save;

    public LevelMap[] levels;
    
    public void Awake()
    {
        SaveData data = Load();
        if (data == null) //If there is currently no save data present, then it will create a new one.
        {
            save = new SaveData();
            //DEFAULT VALUES:
            save.version = "1.0";
            save.level1.unlocked = true;
            save.level2.unlocked = false;
            save.level3.unlocked = false;
            save.level4.unlocked = false;
            save.level5.unlocked = false;

            save.level1.completed = false;
            save.level2.completed = false;
            save.level3.completed = false;
            save.level4.completed = false;
            save.level5.completed = false;

            Save();
        }
        else //If there is save data present, then it will retrieve that data
        {
            save = data;
        }

        if (!save.level1.unlocked) //if locked, for each
        {
            levels[0].icon.color = new Color(0.1f, 0.1f, 0.1f);
            levels[0].button.SetActive(false);
        }
        if(!save.level1.completed)
        {
            levels[0].checkMark.SetActive(false);
        }
        if (!save.level2.unlocked)
        {
            levels[1].icon.color = new Color(0.1f, 0.1f, 0.1f);
            levels[1].button.SetActive(false);
        }
        if (!save.level2.completed)
        {
            levels[1].checkMark.SetActive(false);
        }
        if (!save.level3.unlocked)
        {
            levels[2].icon.color = new Color(0.1f, 0.1f, 0.1f);
            levels[2].button.SetActive(false);
        }
        if (!save.level3.completed)
        {
            levels[2].checkMark.SetActive(false);
        }
        if (!save.level4.unlocked)
        {
            levels[3].icon.color = new Color(0.1f, 0.1f, 0.1f);
            levels[3].button.SetActive(false);
        }
        if (!save.level4.completed)
        {
            levels[3].checkMark.SetActive(false);
        }
        if (!save.level5.unlocked)
        {
            levels[4].icon.color = new Color(0, 0, 0, 0);
            levels[4].button.SetActive(false);
        }
        if (!save.level5.completed)
        {
            levels[4].checkMark.SetActive(false);
        }
    }


    public static void Save()
    {
        string json = JsonUtility.ToJson(save, true);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
    }

    public static SaveData Load()
    {
        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/save.json");
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            return data;
        }
        else
        {
            return null;
        }
    }

    public static void saveLevel(int levelNumber)
    {
        save = Load();
        if (save != null)
        {
            switch (levelNumber)
            {
                case 1:
                    save.level1.completed = true;
                    save.level2.unlocked = true;
                    break;
                case 2:
                    save.level2.completed = true;
                    save.level3.unlocked = true;
                    break;
                case 3:
                    save.level3.completed = true;
                    save.level4.unlocked = true;
                    break;
                case 4:
                    save.level4.completed = true;
                    save.level5.unlocked = true;
                    break;
                case 5:
                    save.level5.completed = true;
                    break;
            }
        }
        else
        {
            Debug.Log("SAVE FILE NOT FOUND");
        }
        Save();
    }
}


[System.Serializable]
public class SaveData
{
    public string version;
    public LevelData level1;
    public LevelData level2;
    public LevelData level3;
    public LevelData level4;
    public LevelData level5;

    public SaveData()
    {
        level1 = new LevelData();
        level2 = new LevelData();
        level3 = new LevelData();
        level4 = new LevelData();
        level5 = new LevelData();
    }
}
[System.Serializable]
public class LevelData
{
    public bool unlocked;
    public bool completed;
}

[System.Serializable]
public class LevelMap
{
    public Image icon;
    public GameObject button;
    public GameObject checkMark;
}
