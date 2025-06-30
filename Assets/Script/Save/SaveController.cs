using UnityEngine;
using System.IO;
using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEditor.Overlays;

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

        if (SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "DaethScreen" && SceneManager.GetActiveScene().name != "HistoryScreen" && SceneManager.GetActiveScene().name != "BattleScena" && SceneManager.GetActiveScene().name != "TheFinalBattle" && SceneManager.GetActiveScene().name != "VictoryScene")
        {
            LoadGame();
        }
        
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
            _scena = "HistoryScreen",
            _playerPosition = new Vector3(45, 39, 0),
            _playerPositionTwo = Vector3.zero,
            _level = 0,
            _xp = 0,
            _money = 0,
            _hp = 0,
            _strenght = 0,
            _def = 0,
            _speed = 0,
            _pointsXp = 0,
            _enemy = "",
            _player = "",
            _light = false,
            _enemysStateGame = new List<int>(),
            _enemysStatePlataform = new List<int>(),
        };

         File.WriteAllText(_saveLocation, JsonUtility.ToJson(saveData));
    }

    public void RestartSave()
    {

        SaveData saveData = new SaveData
        {
            _scena = "HistoryScreen",
            _playerPosition = new Vector3(45, 39, 0),
            _playerPositionTwo = Vector3.zero,
            _level = 0,
            _xp = 0,
            _money = 0,
            _hp = 0,
            _strenght = 0,
            _def = 0,
            _speed = 0,
            _pointsXp = 0,
            _enemy = "",
            _player = "",
            _light = false,
            _enemysStateGame = new List<int>(),
            _enemysStatePlataform = new List<int>(),
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
        List<string> equippedNames = new List<string>();

        foreach (var item in player.Inventory)
        {
            if (!item.isEquipped)
            {
                itemNames.Add(item.data.Name);
                itemCounts.Add(item.count);
            }
        }

        foreach (var equipped in player.InventoryEquiped)
        {
            if (equipped != null)
            {
                equippedNames.Add(equipped.data.Name);
            }
            else
            {
                equippedNames.Add("");
            }
        }

        if (File.Exists(_saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_saveLocation));
            saveData._itensInventoryname = itemNames;
            saveData._itensInventorycount = itemCounts;
            saveData._itensInventoryEquiped = equippedNames;
            saveData._money = player.Money;

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

    public void ChangeLightPlayer()
    {
        if (File.Exists(_saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_saveLocation));
            saveData._light = true;

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

        if (FindAnyObjectByType<Player>() != null)
        {
            FindAnyObjectByType<Player>().UpdateStatusPlayer();
        }
    }


    public void LoadGame()
    {
        if (File.Exists(_saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_saveLocation));

            if (saveData._scena != SceneManager.GetActiveScene().name && SceneManager.GetActiveScene().name != "BattleScena" && SceneManager.GetActiveScene().name != "TheFinalBattle")
            {
                SceneManager.LoadScene(saveData._scena);
            }

            if (SceneManager.GetActiveScene().name != "PlataformGame")
            {
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

            }
            else
            {
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
            }

        }
        else    
        { 
            SaveGame();
        }
    }
}
