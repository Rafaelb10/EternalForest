using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb;
    private Vector2 movement;

    private float _hpMax;
    private float _hp;
    private float _strenght;
    private float _def;
    private float _speed;

    private float _strenghtSword;
    private float _defArmor;
    private float _speedBoots;

    private float _money;

    private float _level;
    private float _xp;

    [SerializeField] private GameObject _statusMenu;
    [SerializeField] private GameObject _inventoryMenu;
    private bool _activeinvent = false;
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
    [SerializeField] private List<ItensData> _inventoryEquiped = new List<ItensData>();

    [SerializeField] private GameObject _playerHp;
    [SerializeField] private Image _hpImage;

    [SerializeField] private float _hpBarSpeed = 3f;
    [SerializeField] private ItensDataBase _itensDataBase;

    private float _targetFill = 1f;


    public float Level { get => _level; set => _level = value; }
    public float Xp { get => _xp; set => _xp = value; }
    public bool ChangeState { get => _changeState; set => _changeState = value; }
    public int State { get => _state; }
    public List<ItensData> Inventory { get => _inventory; set => _inventory = value; }
    public float Money { get => _money; set => _money = value; }
    public float Strenght { get => _strenght; set => _strenght = value; }

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

        if (SceneManager.GetActiveScene().name == "PlataformGame")
        {
            ChangeState = true;
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
        if (Xp >= 100)
        {
            Level = Level + 1;
            Xp = Xp - 100;
        }

        if (ChangeState == true)
        {
            _state = 1;
            ChangeState = false;

            if (_plataformFase == false)
            {
                _statusMenu = null;
            }

            FindAnyObjectByType<PlayerAttack>().State = _state;
        }

        if (_state == 1)
        {
            _playerHp.SetActive(true);

            _targetFill = Mathf.Clamp01(_hp / _hpMax);
            _hpImage.fillAmount = Mathf.Lerp(_hpImage.fillAmount, _targetFill, Time.deltaTime * _hpBarSpeed);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
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
        if (_state == 0 || _plataformFase == true)
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

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (_activeinvent == false)
                {
                    _inventoryMenu.SetActive(true);
                    FindFirstObjectByType<InventoryManager>().UpdateInventoryUI();
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    _activeinvent = true;
                }
                else
                {
                    _inventoryMenu.SetActive(false);
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    _activeinvent = false;
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

    public void GainXp(float xp)
    {
        Xp = Xp + xp;
    }
    public void GainCoin(float coin)
    {
        Money = Money + coin;  
    }

    public void GainHealth(float amount)
    {
        _hp += amount;
        if (_hp > _hpMax)
            _hp = _hpMax;

        _hpImage.fillAmount = _hp / _hpMax;
    }

    public void UpdateStatusPlayer()
    {
        SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(Path.Combine(Application.persistentDataPath, "SaveData.json")));

        _hpMax = 100 + saveData._hp * 10;
        _strenght = 100 + saveData._strenght * 0.5f + _strenghtSword;
        _def = 0 + saveData._def * 0.25f + _defArmor;
        _speed = 3+ saveData._speed * 0.10f + _speedBoots;

        _xp = saveData._xp;
        _level = saveData._level;

        if (ChangeState == false) 
        {

            if (saveData._itensInventoryname.Count != saveData._itensInventorycount.Count)
            {
                Debug.LogError("Dados de inventário salvos estão inconsistentes!");
                return;
            }

            for (int i = 0; i < saveData._itensInventoryname.Count; i++)
            {
                string itemName = saveData._itensInventoryname[i];
                int itemCount = saveData._itensInventorycount[i];

                ItensData baseItem = _itensDataBase.AllItems.FirstOrDefault(item => item.Name == itemName);

                if (baseItem != null)
                {
                    ItensData itemInstance = ScriptableObject.Instantiate(baseItem);
                    itemInstance.Count = itemCount;
                    AddItem(itemInstance);
                }
                else
                {
                    Debug.LogWarning($"Item '{itemName}' não encontrado no banco de dados.");
                }
            }
        }

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
            existingItem.Count = existingItem.Count + 1;
            Debug.Log($"Item '{newItem.Name}' já existe. Quantidade aumentada para {existingItem.Count}.");
        }
        else
        {
            _inventory.Add(newItem);
            Debug.Log($"Item '{newItem.Name}' adicionado ao inventário.");
        }
    }

    public void TakeDamage(float Damage)
    {
        _hp = _hp - Damage;

        if (_hp <= 0)
        {
            _state = 2;
        }
    }
}
