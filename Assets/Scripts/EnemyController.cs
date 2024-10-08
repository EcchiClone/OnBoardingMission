using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Enemy _enemyData;
    Vector2 _targetPos;

    Rigidbody2D _rb;
    Animator _animator;

    int _currentHp;
    bool _isMoving;
    float _initSpeed;
    float _initDistance;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        _enemyData = Managers.Instance.Flow.CurrentEnemy;
        _currentHp = _enemyData.Health;
        _animator = GetComponent<Animator>();

        // 등장 연출 관련
        _rb = GetComponent<Rigidbody2D>();
        _isMoving = true;
        _animator.SetBool("isMoving", _isMoving);
        _targetPos = new Vector2(4, transform.position.y);
        _initSpeed = _enemyData.Speed * 5;
        _initDistance = Vector2.Distance(transform.position, _targetPos);
    }

    private void Update()
    {
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        // _initSpeed 속도로 시작하여 느려지며 특정 X좌표까지 이동
        float distanceToTarget = Vector2.Distance(transform.position, Vector2.right * _targetPos.x + Vector2.up * transform.position.y);

        float speed = Mathf.Lerp(0, _initSpeed, distanceToTarget / _initDistance) + 1f;

        Vector2 direction = (_targetPos - (Vector2)transform.position).normalized;

        _rb.velocity = new Vector2(direction.x * speed, _rb.velocity.y);

        if (distanceToTarget < 0.1f)
        {
            _rb.velocity = Vector2.zero;
            State = EnemyState.Idle;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"))
        {
            if(State != EnemyState.Die)
                State = EnemyState.Damaged;
        }
    }


    EnemyState _state;
    public EnemyState State
    {
        get { return _state; }
        set
        {
            _state = value;
            switch (_state)
            {
                case EnemyState.Idle:
                    Idle();
                    break;
                case EnemyState.Walk:
                    Walk();
                    break;
                case EnemyState.Damaged:
                    Damaged();
                    break;
                case EnemyState.Die:
                    Die();
                    break;

            }
        }
    }
    void Idle()
    {
        _isMoving = false;
        _animator.SetBool("isMoving", _isMoving);
    }

    void Walk()
    {
        _isMoving = true;
        _animator.SetBool("isMoving", _isMoving);
    }

    void Damaged()
    {
        int damage = 100;

        // 일반 데미지
        if (_enemyData.Speed/5.0f < Random.Range(0f, 1f))
        {
            _animator.SetTrigger("Damaged");
            _currentHp = Mathf.Max(_currentHp - damage, 0);
            Managers.Instance.UI.UpdateHpBar(_enemyData.Health, _currentHp);
        }
        // 회피
        else
        {
            damage = 0;
        }

        Managers.Instance.UI.PopupDamageSkin(damage, transform);

        if (_currentHp <= 0)
            State = EnemyState.Die;
        else
            State = _isMoving ? EnemyState.Walk : EnemyState.Idle;
    }

    void Die()
    {
        _animator.SetTrigger("Dead");
    }

    public void DieOnAnimationEvent()
    {
        Managers.Instance.Flow.GenNextEnemy();
        Destroy(gameObject);
    }
    public Enemy GetEnemyData()
    {
        return _enemyData;
    }
    public int GetCurrentHp()
    {
        return _currentHp;
    }

}

public enum EnemyState
{
    Idle = 0,
    Walk = 1,
    Damaged = 2,
    Die = 3,
}