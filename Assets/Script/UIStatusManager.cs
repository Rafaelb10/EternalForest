using TMPro;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;

public class UIStatusManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private TextMeshProUGUI _hp;
    [SerializeField] private TextMeshProUGUI _defense;
    [SerializeField] private TextMeshProUGUI _strenght;
    [SerializeField] private TextMeshProUGUI _speed;

    [SerializeField] private TextMeshProUGUI _point;

    private float _levelPoint;
    private float _hpPoint;
    private float _defensePoint;
    private float _strenghtPoint;
    private float _speedPoint;

    private float _pointToPlace;

    private string _enemy;

    public float LevelPoint { get => _levelPoint; set => _levelPoint = value; }
    public float HpPoint { get => _hpPoint; set => _hpPoint = value; }
    public float DefensePoint { get => _defensePoint; set => _defensePoint = value; }
    public float StrenghtPoint { get => _strenghtPoint; set => _strenghtPoint = value; }
    public float SpeedPoint { get => _speedPoint; set => _speedPoint = value; }
    public float PointToPlace { get => _pointToPlace; set => _pointToPlace = value; }
    public string Enemy { get => _enemy; set => _enemy = value; }

    void Start()
    {
        SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(Path.Combine(Application.persistentDataPath, "SaveData.json")));

        HpPoint = saveData._hp;
        DefensePoint = saveData._def;
        StrenghtPoint = saveData._strenght;
        SpeedPoint = saveData._speed;
        Enemy = null;

        UpdatePoints();
    }

    private void Update()
    {

        _hp.text = HpPoint.ToString();
        _defense.text = DefensePoint.ToString();
        _strenght.text = StrenghtPoint.ToString();
        _speed.text = SpeedPoint.ToString();
    }

    void UpdatePoints()
    {
        SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(Path.Combine(Application.persistentDataPath, "SaveData.json")));

        LevelPoint = FindFirstObjectByType<Player>().Level;
        _level.text = LevelPoint.ToString();

        PointToPlace = FindFirstObjectByType<Player>().Level - saveData._level + saveData._pointsXp;
        _point.text = PointToPlace.ToString();
    }

    public void GainStatus(TextMeshProUGUI statusToGain)
    {
            if (PointToPlace > 0)
            {
                string statusText = statusToGain.text;

                switch (statusText)
                {
                    case "Strength":
                        StrenghtPoint = StrenghtPoint + 1;
                        PointToPlace = PointToPlace - 1;
                        break;

                    case "Life":
                        HpPoint = HpPoint + 1;
                        PointToPlace = PointToPlace - 1;
                        break;

                    case "Speed":
                        SpeedPoint = SpeedPoint + 1;
                        PointToPlace = PointToPlace - 1;
                        break;

                    case "Defense":
                        DefensePoint = DefensePoint + 1;
                        PointToPlace = PointToPlace - 1;
                        break;

                    default:
                        break;
                }
            }
    }

    public void LoseStatus(TextMeshProUGUI statusToLose)
    {
        if (PointToPlace > 0)
        {
            string statusText = statusToLose.text;

            switch (statusText)
            {
                case "Strength":
                    StrenghtPoint = StrenghtPoint - 1;
                    PointToPlace = PointToPlace + 1;
                    break;

                case "Life":
                    HpPoint = HpPoint - 1;
                    PointToPlace = PointToPlace + 1;
                    break;

                case "Speed":
                    SpeedPoint = SpeedPoint - 1;
                    PointToPlace = PointToPlace + 1;
                    break;

                case "Defense":
                    DefensePoint = DefensePoint - 1;
                    PointToPlace = PointToPlace + 1;
                    break;

                default:
                    break;
            }
        }

    }
}
