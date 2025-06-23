using UnityEngine;
using System.IO;
using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SaveController : MonoBehaviour
{
    private string _saveLocation;

    public List<GameObject> _enemysInPlataform = new List<GameObject>();
    public List<GameObject> _enemysInGame = new List<GameObject>();

    void Start()
    {
        Resources.FindObjectsOfTypeAll<Monster>().ToList().ForEach(m=>_enemysInGame.Add(m.gameObject));

        _saveLocation = Path.Combine(Application.persistentDataPath, "SaveData.json");

        LoadGame();
    }

    public void SaveGame() 
    {
        Player player = FindAnyObjectByType<Player>();
        UIStatusManager statusManager = FindAnyObjectByType<UIStatusManager>();

        if (player == null || statusManager == null)
        {
            return;
        }

        List<string> itemNames = new List<string>();
        List<int> itemCount = new List<int>();

        if (player.Inventory != null)
        {
            foreach (ItensData item in player.Inventory)
            {
                itemNames.Add(item.Name);
                itemCount.Add(item.Count);
            }
        }

        SaveData saveData = new SaveData
        {
            _playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            _level = FindAnyObjectByType<Player>().Level,
            _xp = FindAnyObjectByType<Player>().Xp,
            _money = FindAnyObjectByType<Player>().Money,
            _hp = FindAnyObjectByType<UIStatusManager>().HpPoint,
            _strenght = FindAnyObjectByType<UIStatusManager>().StrenghtPoint,
            _def = FindAnyObjectByType<UIStatusManager>().DefensePoint,
            _speed = FindAnyObjectByType<UIStatusManager>().SpeedPoint,

            _pointsXp = FindAnyObjectByType<UIStatusManager>().PointToPlace,

            _enemy = FindAnyObjectByType<UIStatusManager>().Enemy,
            _player = GameObject.FindGameObjectWithTag("Player").name,

            _itensInventoryname = itemNames,
            _itensInventorycount = itemCount,

        };

        File.WriteAllText(_saveLocation, JsonUtility.ToJson(saveData));
        FindAnyObjectByType<Player>().UpdateStatusPlayer();
    }

    public void SaveCombate()
    {
        if (File.Exists(_saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_saveLocation));
            saveData._money = FindAnyObjectByType<Player>().Money;
            saveData._xp = FindAnyObjectByType<Player>().Xp;
            saveData._level = FindAnyObjectByType<Player>().Level;

            File.WriteAllText(_saveLocation, JsonUtility.ToJson(saveData));

        }
    }

    public void LoadGame()
    {
        if (File.Exists(_saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_saveLocation));
            GameObject.FindWithTag("Player").transform.position = saveData._playerPosition;
            FindAnyObjectByType<Player>().Level = saveData._level;
            FindAnyObjectByType<Player>().Xp = saveData._xp;
            FindAnyObjectByType<Player>().Money = saveData._money;
        }
        else    
        { 
            SaveGame();
        }
    }
}
