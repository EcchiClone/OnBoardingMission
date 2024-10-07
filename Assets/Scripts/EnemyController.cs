using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Enemy _enemyData;

    int nowHp;

    private Rigidbody2D rb;

    private Vector2 targetPos;
    private float initSpeed;
    private float initDistance;

    private void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        _enemyData = Managers.Instance.Flow.CurrentEnemy;
        nowHp = _enemyData.Health;
        rb = GetComponent<Rigidbody2D>();

        // 등장 연출 관련
        targetPos = new Vector2(4, transform.position.y);
        initSpeed = _enemyData.Speed * 5;
        initDistance = Vector2.Distance(transform.position, targetPos);

    }

    private void Update()
    {
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        // initSpeed 속도로 시작하여 느려지며 특정 X좌표까지 이동
        float distanceToTarget = Vector2.Distance(transform.position, targetPos);

        float speed = Mathf.Lerp(0, initSpeed, distanceToTarget / initDistance);

        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;

        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

        if (distanceToTarget < 0.1f)
        {
            rb.velocity = Vector2.zero;
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
        nowHp = Mathf.Max(nowHp - damage, 0);
        Managers.Instance.UI.UpdateHpBar(_enemyData.Health, nowHp);
        if (nowHp <= 0)
            Die();
    }
    void Die()
    {
        Managers.Instance.Flow.GenNextEnemy();
        Destroy(gameObject);
    }

}
