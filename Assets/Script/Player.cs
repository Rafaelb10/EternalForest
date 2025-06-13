using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    private float _money;

    private float _level;
    private float _xp;

    [SerializeField] private GameObject _statusMenu;
    private bool _active = false;

    private int _state = 0;
    private bool _changeState;
    [SerializeField] private bool _plataformFase;

    private float _jumpForce = 8.5f;

    [SerializeField] private Transform _groundCheck;
    private float _groundCheckDistance = 0.6f;
    [SerializeField] private LayerMask _groundLayer;

    private float _moveInput;
    private bool _isGrounded;

    [SerializeField] private List<ItensData> _inventory = new List<ItensData>();


    public float Level { get => _level; set => _level = value; }
    public float Xp { get => _xp; set => _xp = value; }
    public bool ChangeState { get => _changeState; set => _changeState = value; }
    public int State { get => _state; }
    public List<ItensData> Inventory { get => _inventory; set => _inventory = value; }
    public float Money { get => _money; set => _money = value; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        UpdateStatusPlayer(); //https://youtu.be/slT_ArW60Xs?si=Ju77HrJwE_Q2zd9G
        _hp = _hpMax;

        if (_state == 0)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (_plataformFase == true)
        {
            rb.gravityScale = 1.0f;
        }
        else
        {
            rb.gravityScale = 0f;
        }

    }

    void Update()
    {
        if (ChangeState == true)
        {
            _state = 1;
            ChangeState = false;

            _statusMenu = null;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            FindAnyObjectByType<PlayerAttack>().State = _state;
        }

        if (_plataformFase == false)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            movement.Normalize();
        }
        else
        {
            Move();
        }

        OpemMenu();
    }

    void OpemMenu()
    {
        if (_state == 0)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_active == false)
                {
                    _statusMenu.SetActive(true);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    _active = true;
                }
                else
                {
                    _statusMenu.SetActive(false);
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    _active = false;
                }
            }
        }
    }

    void Move()
    {
        _moveInput = Input.GetAxisRaw("Horizontal");

        _isGrounded = Physics2D.Raycast(_groundCheck.position, Vector2.down, _groundCheckDistance, _groundLayer);

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, _jumpForce);
        }
    }

    void FixedUpdate()
    {
        if (_plataformFase == false)
        {
            rb.MovePosition(rb.position + movement * _speed * Time.fixedDeltaTime);
        }
        else
        {
            rb.linearVelocity = new Vector2(_moveInput * _speed, rb.linearVelocity.y);
        }
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

    public void AddItem(ItensData newItem)
    {
        if (newItem == null)
        {
            return;
        }

        var existingItem = _inventory.FirstOrDefault(i => i.Name == newItem.Name);

        if (existingItem != null)
        {
            existingItem.Count += newItem.Count;
            Debug.Log($"Item '{newItem.Name}' já existe. Quantidade aumentada para {existingItem.Count}.");
        }
        else
        {
            _inventory.Add(newItem);
            Debug.Log($"Item '{newItem.Name}' adicionado ao inventário.");
        }
    }

}
