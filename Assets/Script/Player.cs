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

    private bool _light;
    [SerializeField] private bool _plataformFase;

    private float _jumpForce = 8.5f;

    [SerializeField] private Transform _groundCheck;
    private float _groundCheckDistance = 0.6f;
    [SerializeField] private LayerMask _groundLayer;

    private float _moveInput;
    private bool _isGrounded;

    [SerializeField]
    private List<InventoryItem> _inventory = new List<InventoryItem>();
    [SerializeField]
    private List<InventoryItem> _inventoryEquiped = new List<InventoryItem>(new InventoryItem[3]);

    [SerializeField] private GameObject _playerHp;
    [SerializeField] private Image _hpImage;

    [SerializeField] private float _hpBarSpeed = 3f;
    [SerializeField] private ItensDataBase _itensDataBase;

    private float _targetFill = 1f;


    public float Level { get => _level; set => _level = value; }
    public float Xp { get => _xp; set => _xp = value; }
    public bool ChangeState { get => _changeState; set => _changeState = value; }
    public int State { get => _state; }
    public List<InventoryItem> Inventory { get => _inventory; set => _inventory = value; }
    public float Money { get => _money; set => _money = value; }
    public float Strenght { get => _strenght; set => _strenght = value; }
    public List<InventoryItem> InventoryEquiped { get => _inventoryEquiped; set => _inventoryEquiped = value; }

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

        OrganizeInventory();
        OpemMenu();
    }

    void OpemMenu()
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
                    _active = false;
                    if (_state != 1) 
                    {
                        Cursor.visible = false;
                        Cursor.lockState = CursorLockMode.Locked;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (_activeinvent == false)
                {
                    _inventoryMenu.SetActive(true);
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    _activeinvent = true;
                }
                else
                {
                    _inventoryMenu.SetActive(false);
                    _activeinvent = false;
                    if (_state != 1)
                    {
                        Cursor.visible = false;
                        Cursor.lockState = CursorLockMode.Locked;
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

    public void EquipamentStart()
    {
            for (int i = 0; i < _inventoryEquiped.Count; i++)
            {
                var item = _inventoryEquiped[i];
                if (item == null || item.data == null) continue;

                switch (i)
                {
                    case 0: // Espada
                        _strenghtSword = item.data.Streght;
                        _strenght = _strenght + _strenghtSword;
                        break;
                    case 1: // Armadura
                        _defArmor = item.data.Def;
                        _def = _def + _defArmor;
                        break;
                    case 2: // Bota
                        _speedBoots = item.data.Speed;
                        _speed = _speed + _speedBoots;
                        break;
                }
            }
    }

    public void StatusEquipament(bool add, ItensData newItem)
    {

        if (add == true)
        {
            if ((int)newItem.TypeEquipamente == 1)
            {
                _strenght =  _strenght + newItem.Streght; 
            }
            else if ((int)newItem.TypeEquipamente == 2)
            {
                _def =  _def + newItem.Def;
            }
            else if ((int)newItem.TypeEquipamente == 3)
            {
                _speed = _speed + newItem.Speed;
            }
        }
        else if (add == false)
        {
            if ((int)newItem.TypeEquipamente == 1)
            {
                _strenght = _strenght - newItem.Streght;
            }
            else if ((int)newItem.TypeEquipamente == 2)
            {
                _def = _def - newItem.Def;
            }
            else if ((int)newItem.TypeEquipamente == 3)
            {
                _speed = _speed - newItem.Speed;
            }
        }
    }

    public void UpdateStatusPlayer()
    {
        string path = Path.Combine(Application.persistentDataPath, "SaveData.json");
        if (!File.Exists(path)) return;

        SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));

        if (ChangeState == false)
        {
            _inventory.Clear();
            _inventoryEquiped = new List<InventoryItem> { null, null, null };

            if (saveData._itensInventoryname.Count == saveData._itensInventorycount.Count)
            {
                for (int i = 0; i < saveData._itensInventoryname.Count; i++)
                {
                    string itemName = saveData._itensInventoryname[i];
                    int itemCount = saveData._itensInventorycount[i];

                    var baseItem = _itensDataBase.AllItems.FirstOrDefault(item => item.Name == itemName);
                    if (baseItem != null)
                    {
                        ItensData itemInstance = ScriptableObject.Instantiate(baseItem);
                        itemInstance.Count = itemCount;
                        AddItem(itemInstance);
                    }
                }
            }

            for (int i = 0; i < saveData._itensInventoryEquiped.Count; i++)
            {
                string equippedName = saveData._itensInventoryEquiped[i];
                if (!string.IsNullOrEmpty(equippedName))
                {
                    var baseItem = _itensDataBase.AllItems.FirstOrDefault(item => item.Name == equippedName);
                    if (baseItem != null)
                    {
                        AddItem(baseItem);
                        AddItemEquipament(baseItem);
                    }
                }
            }
        }

        _hpMax = 100 + saveData._hp * 10;
        _strenght = 100 + saveData._strenght * 0.5f + _strenghtSword;
        _def = 0 + saveData._def * 0.25f + _defArmor;
        _speed = 3 + saveData._speed * 0.10f + _speedBoots;

        _xp = saveData._xp;
        _level = saveData._level;

        _light = saveData._light;

        if (_light == true)
        {
            var light2D = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            if (light2D != null)
            {
                light2D.enabled = true;
            }
        }

        EquipamentStart();
    }

    public void AddItem(ItensData newItem)
    {
        if (newItem == null) return;

        int countToAdd = newItem.Count > 0 ? newItem.Count : 1;

        var existing = _inventory.FirstOrDefault(i => i.data.Name == newItem.Name && !i.isEquipped);
        if (existing != null)
        {
            existing.count += countToAdd;
        }
        else
        {
            _inventory.Add(new InventoryItem(newItem, countToAdd));
        }

        FindAnyObjectByType<SaveController>().SaveInventory();
    }

    public void RemoveItem(ItensData itemToRemove)
    {
        if (itemToRemove == null) return;

        var existingItem = _inventory.FirstOrDefault(i => i.data.Name == itemToRemove.Name && !i.isEquipped);
        if (existingItem != null)
        {
            existingItem.count--;
            if (existingItem.count <= 0)
            {
                _inventory.Remove(existingItem);
            }
            else
            {
            }

            FindAnyObjectByType<SaveController>().SaveInventory();
        }
        else
        {
        }
    }

    public void AddItemEquipament(ItensData newItem)
    {
        if (newItem == null || newItem.Type != ItensData.TypeItem.Equipament) return;

        int slotIndex = (int)newItem.TypeEquipamente - 1;
        if (slotIndex < 0 || slotIndex >= _inventoryEquiped.Count) return;

        var currentEquipped = _inventoryEquiped[slotIndex];

        if (currentEquipped != null)
        {
            currentEquipped.isEquipped = false;

            StatusEquipament(false, currentEquipped.data);

            var existing = _inventory.FirstOrDefault(i => i.data.Name == currentEquipped.data.Name && !i.isEquipped);
            if (existing != null)
            {
                existing.count += currentEquipped.count;
            }
            else
            {
                _inventory.Add(new InventoryItem(currentEquipped.data, currentEquipped.count));
            }

            _inventoryEquiped[slotIndex] = null;
        }

        var itemInInventory = _inventory.FirstOrDefault(i => i.data.Name == newItem.Name && !i.isEquipped);
        if (itemInInventory != null)
        {
            itemInInventory.count--;
            if (itemInInventory.count <= 0)
            {
                _inventory.Remove(itemInInventory);
            }
        }

        InventoryItem equippedItem = new InventoryItem(newItem, 1) { isEquipped = true };
        _inventoryEquiped[slotIndex] = equippedItem;

        StatusEquipament(true, newItem);

        FindAnyObjectByType<SaveController>()?.SaveInventory();
    }

    public void RemoveItemEquipament(ItensData itemToRemove)
    {
        if (itemToRemove == null) return;

        int slotIndex = (int)itemToRemove.TypeEquipamente - 1;
        if (slotIndex < 0 || slotIndex >= _inventoryEquiped.Count) return;

        StatusEquipament(false, itemToRemove);
        var equipped = _inventoryEquiped[slotIndex];
        if (equipped != null && equipped.data.Name == itemToRemove.Name)
        {
            _inventoryEquiped[slotIndex] = null;

            var existing = _inventory.FirstOrDefault(i => i.data.Name == itemToRemove.Name && !i.isEquipped);
            if (existing != null)
            {
                existing.count++;
            }
            else
            {
                var newItem = new InventoryItem(itemToRemove, 1);
                _inventory.Add(newItem);
            }
            FindAnyObjectByType<SaveController>().SaveInventory();
            
        }
    }

    public void OrganizeInventory()
    {
        var grouped = _inventory
            .Where(item => item != null && item.data != null && !item.isEquipped && item.count > 0)
            .GroupBy(item => item.data.Name)
            .Select(group =>
            {
                int totalCount = group.Sum(i => i.count);
                return new InventoryItem(group.First().data, totalCount);
            })
            .ToList();

        var equippedItems = _inventory
            .Where(item => item != null && item.isEquipped)
            .ToList();

        _inventory = grouped.Concat(equippedItems).ToList();

        FindAnyObjectByType<SaveController>()?.SaveInventory();
    }

    public void TakeDamage(float Damage)
    {
        Damage = Damage - _def;

        if (Damage <= 0) 
        {
            Damage = 1;
        }

        _hp = _hp - Damage;

        if (_hp <= 0)
        {
            _state = 2;
        }
    }
}
