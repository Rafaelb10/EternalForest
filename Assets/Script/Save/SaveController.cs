using UnityEngine;
using System.IO;
using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class SaveController : MonoBehaviour
{
    private string _saveLocation;

    public List<MonsterPlataform> _enemysInPlataform = new List<MonsterPlataform>();
    public List<Monster> _enemysInGame = new List<Monster>();

    [System.Obsolete]
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            _enemysInGame = FindObjectsOfType<Monster>().ToList();
        }
        else if (SceneManager.GetActiveScene().name == "PlataformGame")
        {
            _enemysInPlataform = FindObjectsOfType<MonsterPlataform>().ToList();
        }


        _saveLocation = Path.Combine(Application.persistentDataPath, "SaveData.json");

        LoadGame();
    }

    public void SaveGame() 
    {

        List<int> stateMonsterGame = new List<int>();

        List<int> stateMonsterPlataform = new List<int>();

        if (_enemysInGame != null)
        {
            foreach (Monster monster in _enemysInGame)
            {
                stateMonsterGame.Add(monster.State);
            }
        }

        if (_enemysInPlataform != null)
        {
            foreach (MonsterPlataform monster in _enemysInPlataform)
            {
                stateMonsterPlataform.Add(monster.State);
            }
        }

        SaveData saveData = new SaveData
        {
                _scena = "Game",
                _playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
                _playerPositionTwo = new Vector3(0, 0, 0),
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

                _enemysStateGame = stateMonsterGame,
                _enemysStatePlataform = stateMonsterPlataform
         };

         File.WriteAllText(_saveLocation, JsonUtility.ToJson(saveData));
    }

    public void SaveBeforeCombate()
    {
        if (File.Exists(_saveLocation))
        {
            List<int> stateMonsterGame = new List<int>();

            List<int> stateMonsterPlataform = new List<int>();

            if (_enemysInGame != null)
            {
                foreach (Monster monster in _enemysInGame)
                {
                    stateMonsterGame.Add(monster.State);
                }
            }

            if (_enemysInPlataform != null)
            {
                foreach (MonsterPlataform monster in _enemysInPlataform)
                {
                    stateMonsterPlataform.Add(monster.State);
                }
            }

            if (SceneManager.GetActiveScene().name == "Game")
            {
                SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_saveLocation));
                saveData._enemy = FindAnyObjectByType<UIStatusManager>().Enemy;
                saveData._player = GameObject.FindGameObjectWithTag("Player").name;
                saveData._money = FindAnyObjectByType<Player>().Money;
                saveData._xp = FindAnyObjectByType<Player>().Xp;
                saveData._level = FindAnyObjectByType<Player>().Level;

                saveData._enemysStateGame = stateMonsterGame;
                File.WriteAllText(_saveLocation, JsonUtility.ToJson(saveData));
            }
            else if (SceneManager.GetActiveScene().name == "PlataformGame")
            {
                SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_saveLocation));
                saveData._money = FindAnyObjectByType<Player>().Money;
                saveData._xp = FindAnyObjectByType<Player>().Xp;
                saveData._level = FindAnyObjectByType<Player>().Level;

                saveData._enemysStatePlataform = stateMonsterPlataform;
                File.WriteAllText(_saveLocation, JsonUtility.ToJson(saveData));
            }
        }
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

    public void SaveInventory()
    {
        Player player = FindAnyObjectByType<Player>();
        if (player == null || player.Inventory == null) return;

        List<string> itemNames = new List<string>();
        List<int> itemCounts = new List<int>();

        foreach (var item in player.Inventory)
        {
            itemNames.Add(item.data.Name);
            itemCounts.Add(item.count);
        }

        if (File.Exists(_saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_saveLocation));
            saveData._itensInventoryname = itemNames;
            saveData._itensInventorycount = itemCounts;
            File.WriteAllText(_saveLocation, JsonUtility.ToJson(saveData));
        }
    }

    public void SavePlayerPosition()
    {
        if (File.Exists(_saveLocation))
        {
            if (SceneManager.GetActiveScene().name == "Game")
            {
                SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_saveLocation));
                saveData._playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

                File.WriteAllText(_saveLocation, JsonUtility.ToJson(saveData));
            }
            else if (SceneManager.GetActiveScene().name == "PlataformGame")
            {
                SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_saveLocation));
                saveData._playerPositionTwo = GameObject.FindGameObjectWithTag("Player").transform.position;

                File.WriteAllText(_saveLocation, JsonUtility.ToJson(saveData));
            }
        }
    }

    public void SaveState()
    {
        if (File.Exists(_saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_saveLocation));
            saveData._hp = FindAnyObjectByType<UIStatusManager>().HpPoint;
            saveData._strenght = FindAnyObjectByType<UIStatusManager>().StrenghtPoint;
            saveData._def = FindAnyObjectByType<UIStatusManager>().DefensePoint;
            saveData._speed = FindAnyObjectByType<UIStatusManager>().SpeedPoint;
            saveData._pointsXp = FindAnyObjectByType<UIStatusManager>().PointToPlace;

            File.WriteAllText(_saveLocation, JsonUtility.ToJson(saveData));
        }

        FindAnyObjectByType<Player>().UpdateStatusPlayer();
    }

    public void ChangeScena(string scenaname)
    {
        if (File.Exists(_saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_saveLocation));
            saveData._scena = scenaname;

            File.WriteAllText(_saveLocation, JsonUtility.ToJson(saveData));
        }

        FindAnyObjectByType<Player>().UpdateStatusPlayer();
    }


    public void LoadGame()
    {
        if (File.Exists(_saveLocation))
        {
            if (SceneManager.GetActiveScene().name == "Game")
            {
                SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_saveLocation));
                GameObject.FindWithTag("Player").transform.position = saveData._playerPosition;
                FindAnyObjectByType<Player>().Level = saveData._level;
                FindAnyObjectByType<Player>().Xp = saveData._xp;
                FindAnyObjectByType<Player>().Money = saveData._money;

                if (_enemysInGame != null && saveData._enemysStateGame != null)
                {
                    for (int i = 0; i < _enemysInGame.Count; i++)
                    {
                        if (i < saveData._enemysStateGame.Count)
                        {
                            _enemysInGame[i].State = saveData._enemysStateGame[i];
                        }
                    }
                }

                if (saveData._scena != "Game")
                {
                    SceneManager.LoadScene(saveData._scena);
                }
            }
            else if (SceneManager.GetActiveScene().name == "PlataformGame")
            {
                SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_saveLocation));
                GameObject.FindWithTag("Player").transform.position = saveData._playerPositionTwo;
                FindAnyObjectByType<Player>().Level = saveData._level;
                FindAnyObjectByType<Player>().Xp = saveData._xp;
                FindAnyObjectByType<Player>().Money = saveData._money;

                if (_enemysInPlataform != null && saveData._enemysInPlataform != null)
                {
                    for (int i = 0; i < _enemysInPlataform.Count; i++)
                    {
                        if (i < saveData._enemysInPlataform.Count)
                        {
                            _enemysInPlataform[i].State = saveData._enemysInPlataform[i];
                        }
                    }
                }

                if (saveData._scena != "PlataformGame")
                {
                    SceneManager.LoadScene(saveData._scena);
                }
            }

        }
        else    
        { 
            SaveGame();
        }
    }
}
