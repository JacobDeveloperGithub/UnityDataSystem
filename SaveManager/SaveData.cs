using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataPoint{
    public string key;
    public string value;

    public DataPoint(string key, string value){
        this.key = key;
        this.value = value;
    }
}

[System.Serializable]
public class DataContainer{
    public List<DataPoint> dataPoints = new List<DataPoint>();
}

public static class SaveData{
    [SerializeField] private static DataContainer StoredData = null;

    public static void StoreData(string key, object data){
        if(StoredData == null)
            LoadAllSavedData();
        if(HasData(key))
            StoredData.dataPoints.RemoveAll(e => e.key == key);
        StoredData.dataPoints.Add(new DataPoint(key, DataSerializer.SerializeData(data)));
        Save();
    }

    public static bool HasData(string key){
        if(StoredData == null)
            LoadAllSavedData();
        return StoredData.dataPoints.Any(e => e.key == key);
    }

    public static T ReadData<T>(string key){
        if(StoredData == null)
            LoadAllSavedData();
        return DataSerializer.DeserializeData<T>(StoredData.dataPoints.Find(e => e.key == key)?.value);
    }

    public static void Save(){
        #if UNITY_WEBGL && !UNITY_EDITOR
            PlayerPrefs.SetString("SaveData", DataSerializer.SerializeData(StoredData));
            PlayerPrefs.Save();
        #else
            string path = Path.Combine(Application.persistentDataPath, "Save.json");
            File.WriteAllText(path, DataSerializer.SerializeData(StoredData));
        #endif
    }

    public static void ClearSaveData(){
        #if UNITY_WEBGL && !UNITY_EDITOR
            PlayerPrefs.DeleteKey("SaveData");
            PlayerPrefs.Save();
        #else
            string path = Path.Combine(Application.persistentDataPath, "Save.json");
            File.Delete(path);
        #endif
    }

    public static void LoadAllSavedData(){
        #if UNITY_WEBGL && !UNITY_EDITOR
            if (PlayerPrefs.HasKey("SaveData")) {
                StoredData = DataSerializer.DeserializeData<DataContainer>(PlayerPrefs.GetString("SaveData"));
            } else {
                ClearSaveData();
                StoredData = new DataContainer();
            }
        #else
            string path = Path.Combine(Application.persistentDataPath, "Save.json");
            try{
                StoredData = DataSerializer.DeserializeData<DataContainer>(File.ReadAllText(path));
            } catch{
                ClearSaveData();
                StoredData = new DataContainer();
            }
        #endif
    }
}


