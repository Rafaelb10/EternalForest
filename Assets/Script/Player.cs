using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 movement;

    public float _hpMax;
    public float _hp;
    public float _strenght;
    public float _def;
    public float _speed;

    private float _level;
    private float _xp;

    [SerializeField] private GameObject _statusMenu;
    private bool _active = false;

    private int _state = 0;
    private bool _changeState;

    public float Level { get => _level; set => _level = value; }
    public float Xp { get => _xp; set => _xp = value; }
    public bool ChangeState { get => _changeState; set => _changeState = value; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        UpdateStatusPlayer();
        _hp = _hpMax;

        if (_state == 1)
        {
            _statusMenu = null;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (ChangeState == true)
        {
            _state = 1;
            ChangeState = false;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.Normalize();
        OpemMenu();
    }

    void OpemMenu()
    {
        if (_state == 0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_active == true)
                {
                    _statusMenu.SetActive(true);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    _active = false;
                }
                else
                {
                    _statusMenu.SetActive(false);
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    _active = true;
                }
            }
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * _speed * Time.fixedDeltaTime);
    }

    private void GainXp(float xp)
    {
        Xp = Xp + xp;

        if (Xp >= 100)
        {
            Level = Level + 1;
            Xp = Xp - 100;
        }
    }

    private void UpdateStatusPlayer()
    {
        SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(Path.Combine(Application.persistentDataPath, "SaveData.json")));

        _hpMax = 100 + saveData._hp * 10;
        _strenght = 1 + saveData._strenght * 0.5f;
        _def = 0 + saveData._def * 0.25f;
        _speed = 3+ saveData._speed * 0.10f;
    }

}
