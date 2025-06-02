using UnityEngine;
using System.IO;
using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;

public class SaveController : MonoBehaviour
{
    private string _saveLocation;

    void Start()
    {
        _saveLocation = Path.Combine(Application.persistentDataPath, "SaveData.json");

        LoadGame();
    }

    public void SaveGame() 
    {
        SaveData saveData = new SaveData
        {
            _playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            _level = FindObjectOfType<Player>().Level,
            _xp = FindObjectOfType<Player>().Xp,
            _hp = FindObjectOfType<UIStatusManager>().HpPoint,
            _strenght = FindObjectOfType<UIStatusManager>().StrenghtPoint,
            _def = FindObjectOfType<UIStatusManager>().DefensePoint,
            _speed = FindObjectOfType<UIStatusManager>().SpeedPoint,

            _pointsXp = FindObjectOfType<UIStatusManager>().PointToPlace,

        };

        File.WriteAllText(_saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (File.Exists(_saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_saveLocation));
            GameObject.FindWithTag("Player").transform.position = saveData._playerPosition;
        }
        else    
        { 
            SaveGame();
        }
    }
}
