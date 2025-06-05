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
            _level = FindAnyObjectByType<Player>().Level,
            _xp = FindAnyObjectByType<Player>().Xp,
            _hp = FindAnyObjectByType<UIStatusManager>().HpPoint,
            _strenght = FindAnyObjectByType<UIStatusManager>().StrenghtPoint,
            _def = FindAnyObjectByType<UIStatusManager>().DefensePoint,
            _speed = FindAnyObjectByType<UIStatusManager>().SpeedPoint,

            _pointsXp = FindAnyObjectByType<UIStatusManager>().PointToPlace,

            _enemy = FindAnyObjectByType<UIStatusManager>().Enemy,
            _player = GameObject.FindGameObjectWithTag("Player").name

        };

        File.WriteAllText(_saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (File.Exists(_saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_saveLocation));
            GameObject.FindWithTag("Player").transform.position = saveData._playerPosition;
            FindAnyObjectByType<Player>().Level = saveData._level;
            FindAnyObjectByType<Player>().Xp = saveData._xp;

            FindAnyObjectByType<UIStatusManager>().HpPoint = saveData._xp;
            FindAnyObjectByType<UIStatusManager>().StrenghtPoint = saveData._xp;
            FindAnyObjectByType<UIStatusManager>().DefensePoint = saveData._xp;
            FindAnyObjectByType<UIStatusManager>().SpeedPoint = saveData._xp;
            FindAnyObjectByType<UIStatusManager>().PointToPlace = saveData._xp;
        }
        else    
        { 
            SaveGame();
        }
    }
}
