using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Enemy _enemyData;

    int _currentHp;

    private Rigidbody2D _rb;

    private Vector2 _targetPos;
    private float _initSpeed;
    private float _initDistance;

    private void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        _enemyData = Managers.Instance.Flow.CurrentEnemy;
        _currentHp = _enemyData.Health;
        _rb = GetComponent<Rigidbody2D>();

        // 등장 연출 관련
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
        float distanceToTarget = Vector2.Distance(transform.position, _targetPos);

        float speed = Mathf.Lerp(0, _initSpeed, distanceToTarget / _initDistance);

        Vector2 direction = (_targetPos - (Vector2)transform.position).normalized;

        _rb.velocity = new Vector2(direction.x * speed, _rb.velocity.y);

        if (distanceToTarget < 0.1f)
        {
            _rb.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerAttack")
        {
            GetDamage(100);
        }
    }
    public void GetDamage(int damage) // 테스트 끝나면 private로 바꾸기?
    {
        _currentHp = Mathf.Max(_currentHp - damage, 0);
        Managers.Instance.UI.UpdateHpBar(_enemyData.Health, _currentHp);
        if (_currentHp <= 0)
            Die();
    }
    void Die()
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
