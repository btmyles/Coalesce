using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveLoad : MonoBehaviour
{
    string filePath;
    public Circle circle; // Used to instantiate circles on load

    public void Start()
    {
        filePath = Application.persistentDataPath + "/savedata.json";
    }

    public bool LoadGame()
    {
        filePath = Application.persistentDataPath + "/savedata.json";

        if(File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                SaveData loadData = JsonUtility.FromJson<SaveData>(jsonData);
                Debug.Log("Loaded JSON: " + jsonData);

                Data.score = loadData.score;
                Data.maxLevel = loadData.maxLevel;
                string[] circles = loadData.allCircles;

                string[] split;
                int x, y, level;
                Circle[] circlearray = new Circle[circles.Length];
                for (int i=0; i<circles.Length; i++)
                {
                    split = circles[i].Split('_');
                    x = int.Parse(split[1].Substring(1));
                    y = int.Parse(split[2].Substring(1));
                    level = int.Parse(split[3].Substring(1));

                    // TODO : INstantiate an array of circles.'S'
                    circlearray[i] = Instantiate(circle, new Vector3(x+1,y+0.5f,0), Quaternion.identity);
                    Debug.Log("Instantiated circle: "+circlearray[i]);
                    circlearray[i].SetLevel(level);
                    // Debug.Log("Adding circle: "+circlearray[i]);
                    circlearray[i].AddToGrid();
                }


                return true;
            }
            else
            {
                Debug.Log("File doesn't exist: "+ filePath);
                return false;
            }
    }

    public void SaveGame()
    {
        string[] allCircles = new string[Circle.allCircles.Count];
        for(int i=0; i<Circle.allCircles.Count; i++)
            allCircles[i] = Circle.allCircles[i].ToString();

        SaveData saveData = new SaveData(
            Data.score
            , Data.maxLevel
            , allCircles
        );

        string jsonData = JsonUtility.ToJson(saveData);
        Debug.Log("Saving json: "+jsonData);
        File.WriteAllText(filePath, jsonData);
    }

    public void DeleteSave()
    {
        File.Delete(filePath);
    }

    [Serializable]
    private class SaveData
    {
        public int score;
        public int maxLevel;
        public string[] allCircles;

        public SaveData(int score, int maxLevel, string[] allCircles)
        {
            this.score = score;
            this.maxLevel = maxLevel;
            this.allCircles = allCircles;
        }
    }

}
