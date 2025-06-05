using UnityEngine;
using System.IO;
using Unity.VisualScripting;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerSpaw;
    [SerializeField] private GameObject _enemySpaw;

    [SerializeField] private GameObject _mapOne;
    [SerializeField] private GameObject _mapTwo;
    [SerializeField] private GameObject _mapThree;

    private void Start()
    {
        SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(Path.Combine(Application.persistentDataPath, "SaveData.json")));

        GameObject monster = Resources.Load<GameObject>("Prefabs/" + saveData._enemy);
        Instantiate(monster, _enemySpaw.transform);
        FindAnyObjectByType<Monster>().ChangeState = true;

        GameObject player = Resources.Load<GameObject>("Prefabs/" + saveData._player);

        Instantiate(player, _playerSpaw.transform);
        FindAnyObjectByType<Player>().ChangeState = true;
        
    }
}
