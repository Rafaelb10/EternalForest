using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BossFinal : MonoBehaviour, IDamageable
{
    [SerializeField] private Transform _positionSpaw;

    [SerializeField] private GameObject _attackOneSpaw;
    [SerializeField] private List<Transform> _attackTwoSpaw;
    [SerializeField] private GameObject _attackthree;

    [SerializeField] private GameObject _bulletAttackOne;
    [SerializeField] private GameObject _bulletAttackTwo;
    [SerializeField] private GameObject _bulletCruz;

    private bool _playerInZone = false;
    private bool _isModeAttack = false;

    [SerializeField] private GameObject _enemyHp;
    [SerializeField] private Image _hpImage;

    [SerializeField] private float _hpBarSpeed = 3f;
    private float _targetFill = 1f;

    private float _hp = 400;
    private float _hpMax;

    private float _cooldownTime = 20f;
    private float _lastAttackTime = -Mathf.Infinity;

    private Vector2 _velocity = Vector2.zero;

    private Vector2 _randomDirection = Vector2.zero;
    private float _randomMoveTime = 0f;
    private bool _attackingCooldownSlap;

    private bool _isAttackingAnimation;
    private float _damage = 40f;

    public float Damage { get => _damage;}
    public bool PlayerInZone { get => _playerInZone; set => _playerInZone = value; }
    public DialogueData DialogueOne { get => _dialogueOne; set => _dialogueOne = value; }
    public DialogueData DialogueTwo { get => _dialogueTwo; set => _dialogueTwo = value; }
    public GameObject UiDialogue { get => _uiDialogue; set => _uiDialogue = value; }
    public bool IsTalking { get => _isTalking; set => _isTalking = value; }

    private int _state = 0;
    private int _dialogueIndex = 0;
    private bool _isTalking = false;

    [SerializeField] private DialogueData _dialogueOne;
    [SerializeField] private DialogueData _dialogueTwo;
    [SerializeField] private DialogueManager _dialogueManager;
    [SerializeField] private GameObject _uiDialogue;

    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D _rb;
    private bool _isPaused = false;

    private Vector2 movement;
    private Vector2 lastPosition;
    private float movimentoMinimo = 0.1f;
    private float tempoParado = 0f;
    private float tempoMinimoParado = 0.2f;
    private int ultimaDirecao = -1;

    private void Start()
    {
        _positionSpaw = gameObject.transform;
        _hpMax = _hp;
        anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        if (_state == 0)
        {
            _isModeAttack = false;
            _enemyHp.SetActive(false);
            if (_playerInZone == true && _isTalking == false && Input.GetKeyDown(KeyCode.Q))
            {
                StartDialogue();
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0) && _isTalking == true)
            {
                if (_dialogueOne._npc == DialogueData.TypeNpc.Npc)
                {
                    ShowNextDialogueLine();
                }
            }
        }
        else if (_state == 1)
        {
            _isModeAttack = true;
            _enemyHp.SetActive(true);
        }
        else if (_state == 2)
        {
            transform.position = _positionSpaw.position;
            _isModeAttack = false;
            _enemyHp.SetActive(false);
            if (_playerInZone == true && _isTalking == false && Input.GetKeyDown(KeyCode.Q))
            {
                StartDialogue();
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0) && _isTalking == true)
            {
                if (_dialogueTwo._npc == DialogueData.TypeNpc.Npc)
                {
                    ShowNextDialogueLine();
                }
            }
        }

        if (_isModeAttack == true)
        {
            if (_playerInZone == true)
            {
                Attack();
            }

            _targetFill = Mathf.Clamp01(_hp / _hpMax);
            _hpImage.fillAmount = Mathf.Lerp(_hpImage.fillAmount, _targetFill, Time.deltaTime * _hpBarSpeed);
        }

        animationController();
    }

    void FixedUpdate()
    {
        if (_isModeAttack == true)
        {
            Move();
        }
    }

    void StartDialogue()
    {
        if (_state == 0)
        {
            switch (_dialogueOne._npc)
            {
                case DialogueData.TypeNpc.Npc:
                    _uiDialogue.gameObject.SetActive(true);
                    _dialogueManager.SetName(_dialogueOne._nameCharacther);
                    _dialogueIndex = 0;
                    _isTalking = true;
                    ShowNextDialogueLine();
                    break;
            }
        }
        else if (_state == 2)
        {
            switch (_dialogueTwo._npc)
            {
                case DialogueData.TypeNpc.Npc:
                    _uiDialogue.gameObject.SetActive(true);
                    _dialogueManager.SetName(_dialogueTwo._nameCharacther);
                    _dialogueIndex = 0;
                    _isTalking = true;
                    ShowNextDialogueLine();
                    break;
            }
        }
    }

    private void ShowNextDialogueLine()
    {
        if (_state == 0)
        {
            if (_dialogueIndex < _dialogueOne._word.Length)
            {
                _dialogueManager.SetDialogue(_dialogueOne._word[_dialogueIndex]);
                _dialogueIndex++;
            }
            else
            {
                _isTalking = false;
                _uiDialogue.gameObject.SetActive(false);
                _state = _state + 1;
                FindFirstObjectByType<Player>().ChangeState = true;
            }
        }
        else if (_state == 2)
        {
            if (_dialogueIndex < _dialogueTwo._word.Length)
            {
                _dialogueManager.SetDialogue(_dialogueTwo._word[_dialogueIndex]);
                _dialogueIndex++;
            }
            else
            {
                _isTalking = false;
                _uiDialogue.gameObject.SetActive(false);
                UnityEngine.SceneManagement.SceneManager.LoadScene("VictoryScene");
            }
        }
    }

    void Attack()
    {
        if (Time.time < _lastAttackTime + _cooldownTime) return;

        float healthPercentage = _hp / _hpMax;
        _cooldownTime = (healthPercentage <= 0.5f) ? 2.5f : 5f;

        int randomAttack = Random.Range(1, 6);

        switch (randomAttack)
        {
            case 1:
                Debug.Log("1");
                AttackOne();
                break;
            case 2:
                Debug.Log("2");
                AttackTwo();
                break;
            case 3:
                Debug.Log("3");
                AttackThree();
                break;
            case 4:
                Debug.Log("4");
                AttackFour();
                break;
            case 5:
                Debug.Log("5");
                AttackFive();
                break;
        }

        _lastAttackTime = Time.time;
    }
    void AttackOne()
    {
        int bulletCount = Random.Range(10, 20);

        Vector2 spawnCenter = _attackOneSpaw.transform.position;
        Vector2 areaSize = Vector2.one;

        BoxCollider2D box = _attackOneSpaw.GetComponent<BoxCollider2D>();
        if (box != null)
        {
            areaSize = box.size;
        }

        for (int i = 0; i < bulletCount; i++)
        {
            float offsetX = Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
            float offsetY = Random.Range(-areaSize.y / 2f, areaSize.y / 2f);
            Vector3 spawnPos = spawnCenter + new Vector2(offsetX, offsetY);

            GameObject bullet = Instantiate(_bulletAttackOne, spawnPos, Quaternion.identity);
        }
    }
    void AttackTwo()
    {
        StartCoroutine(AttackCooldownTwo());
    }
    void AttackThree()
    {
        var light2D = _attackthree.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        if (light2D != null)
        {
            light2D.enabled = true;
        }
        StartCoroutine(AttackCooldownThree());
    }
    void AttackFour()
    {
        StartCoroutine(DashTowardsPlayer());
    }

    void AttackFive()
    {
        Transform target = FindAnyObjectByType<Player>().transform;
        GameObject cruz = Instantiate(_bulletCruz, gameObject.transform.position, Quaternion.identity);
        CruzTarget bt = cruz.GetComponent<CruzTarget>();
        if (bt != null)
        {
            bt.SetTarget(target);
        }
    }
    void Move()
    {
        Vector2 playerPos = FindAnyObjectByType<Player>().transform.position;
        Vector2 direction = (playerPos - (Vector2)transform.position).normalized;

        if (Vector2.Distance(transform.position, playerPos) > 1.5f)
        {
            Vector2 randomOffset = Random.insideUnitCircle * 0.3f;
            direction += randomOffset;
            direction.Normalize();
        }

        if (!_isPaused)
        {
            _rb.linearVelocity = direction * 5f;
        }

        if (Vector2.Distance(transform.position, playerPos) < 0.8f && !_isPaused)
        {
            StartCoroutine(PauseMovement());
        }
    }

    void animationController()
    {
        float absX = Mathf.Abs(movement.x);
        float absY = Mathf.Abs(movement.y);

        if (movement.magnitude < movimentoMinimo)
        {
            tempoParado += Time.deltaTime;

            if (tempoParado >= tempoMinimoParado && ultimaDirecao != 0)
            {
                anim.SetInteger("Direcao", 0);
                ultimaDirecao = 0;
            }

            return;
        }

        tempoParado = 0f;

        if (absX > absY)
        {
            int novaDirecao = 3;
            if (ultimaDirecao != novaDirecao)
            {
                anim.SetInteger("Direcao", novaDirecao);
                ultimaDirecao = novaDirecao;
            }

            spriteRenderer.flipX = movement.x < 0;
        }
        else
        {
            int novaDirecao = movement.y > 0 ? 2 : 1;
            if (ultimaDirecao != novaDirecao)
            {
                anim.SetInteger("Direcao", novaDirecao);
                ultimaDirecao = novaDirecao;
            }
        }
    }

    private IEnumerator PauseMovement()
    {
        _isPaused = true;
        _rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        _isPaused = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out var player))
        {
            if (_attackingCooldownSlap == false)
            {
                FindAnyObjectByType<Player>().TakeDamage(_damage);
                anim.SetTrigger("Attack");
                _isAttackingAnimation = true;
                StartCoroutine(WaitForAttackAnimationEnd());
                StartCoroutine(AttackCooldown());
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Player>(out var player))
        {
            if (_attackingCooldownSlap == false)
            {
                FindAnyObjectByType<Player>().TakeDamage(_damage);
                anim.SetTrigger("Attack");
                _isAttackingAnimation = true;
                StartCoroutine(WaitForAttackAnimationEnd());
                StartCoroutine(AttackCooldown());
            }
        }

    }

    private bool IsAnimationFinished(string animationName)
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        return !stateInfo.IsName(animationName) || stateInfo.normalizedTime >= 1f;
    }

    IEnumerator WaitForAttackAnimationEnd()
    {
        while (!IsAnimationFinished("Attack"))
        {
            yield return null;
        }

        _isAttackingAnimation = false;

        if (_playerInZone)
        {
            FindAnyObjectByType<Player>().TakeDamage(_damage);
            StartCoroutine(AttackCooldown());
        }
    }
    IEnumerator AttackCooldown()
    {
        _attackingCooldownSlap = true;
        yield return new WaitForSeconds(1);
        _attackingCooldownSlap = false;
    }

    public void TakeDamage(float Damage)
    {
        if (_state != 1) return;

        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(FlashRed());
        _hp = _hp - Damage;

        if (_hp <= 0)
        {
            _state = 2;
        }
    }

    private IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.color = Color.white;
        }
    }

    private IEnumerator AttackCooldownTwo()
    {
        for (int i = 0; i < 3; i++)
        {
            foreach (Transform spawnPoint in _attackTwoSpaw)
            {
                GameObject bullet = Instantiate(_bulletAttackTwo, spawnPoint.position, Quaternion.identity);

                Vector2 direction = (FindAnyObjectByType<Player>().transform.position - spawnPoint.position).normalized;

                bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * 5f; 
            }

            yield return new WaitForSeconds(3);
        }
    }

    IEnumerator AttackCooldownThree()
    {
        yield return new WaitForSeconds(20);
        var light2D = _attackthree.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        if (light2D != null)
        {
            light2D.enabled = false;
        }
    }

    private IEnumerator DashTowardsPlayer()
    {
        float originalDamage = _damage;
        _damage += 30f;

        Vector2 playerPos = FindAnyObjectByType<Player>().transform.position;
        Vector2 directionToPlayer = (playerPos - (Vector2)transform.position).normalized;

        float dashSpeed = 15f;
        float dashDuration = 0.3f; 

        float elapsedTime = 0f;

        _isPaused = true;

        while (elapsedTime < dashDuration)
        {
            _rb.linearVelocity = directionToPlayer * dashSpeed;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _rb.linearVelocity = Vector2.zero;
        _isPaused = false;
        _damage = originalDamage;
    }


}
