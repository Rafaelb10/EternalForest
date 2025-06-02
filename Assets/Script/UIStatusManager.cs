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

    public float LevelPoint { get => _levelPoint;}
    public float HpPoint { get => _hpPoint;}
    public float DefensePoint { get => _defensePoint;}
    public float StrenghtPoint { get => _strenghtPoint;}
    public float SpeedPoint { get => _speedPoint;}
    public float PointToPlace { get => _pointToPlace;}

    void Start()
    {
        SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(Path.Combine(Application.persistentDataPath, "SaveData.json")));

        _hpPoint = saveData._hp;
        _defensePoint = saveData._def;
        _strenghtPoint = saveData._strenght;
        _speedPoint = saveData._speed;

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

        _levelPoint = FindFirstObjectByType<Player>().Level;
        _level.text = LevelPoint.ToString();

        _pointToPlace = FindFirstObjectByType<Player>().Level - saveData._level + saveData._pointsXp;
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
                        _strenghtPoint = StrenghtPoint + 1;
                        _pointToPlace = PointToPlace - 1;
                        break;

                    case "Life":
                        _hpPoint = HpPoint + 1;
                        _pointToPlace = PointToPlace - 1;
                        break;

                    case "Speed":
                        _speedPoint = SpeedPoint + 1;
                        _pointToPlace = PointToPlace - 1;
                        break;

                    case "Defense":
                        _defensePoint = DefensePoint + 1;
                        _pointToPlace = PointToPlace - 1;
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
                    _strenghtPoint = StrenghtPoint - 1;
                    _pointToPlace = PointToPlace + 1;
                    break;

                case "Life":
                    _hpPoint = HpPoint - 1;
                    _pointToPlace = PointToPlace + 1;
                    break;

                case "Speed":
                    _speedPoint = SpeedPoint - 1;
                    _pointToPlace = PointToPlace + 1;
                    break;

                case "Defense":
                    _defensePoint = DefensePoint - 1;
                    _pointToPlace = PointToPlace + 1;
                    break;

                default:
                    break;
            }
        }

    }
}
