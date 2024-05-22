using UnityEngine;

public class LoadSaveController
{
    private const string saveKey = "topDownShooterSave";

    public SaveData Load()
    {
        var saveData = PlayerPrefs.GetString(saveKey);
        return string.IsNullOrEmpty(saveData) ? new SaveData() : JsonUtility.FromJson<SaveData>(saveData);
    }

    public void Save(SaveData saveData)
    {
        var saveDataJson = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(saveKey, saveDataJson);
        PlayerPrefs.Save();
    }
}