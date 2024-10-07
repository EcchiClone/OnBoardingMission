using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] int _damage = 100;
    [SerializeField] float _attackDelay = 1f;

    float _attackTimer;

    private Rigidbody2D rb;

    private Vector2 targetPos;
    private float initSpeed;
    private float initDistance;

    bool _isMoving;

    private void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();

        // 등장 연출 관련
        _isMoving = true;
        targetPos = new Vector2(-4, transform.position.y);
        initSpeed = 5f;
        initDistance = Vector2.Distance(transform.position, targetPos);

    }

    private void Update()
    {
        MoveToTarget();
        Attack();
    }

    private void MoveToTarget()
    {
        _attackTimer = Mathf.Min(_attackTimer + Time.deltaTime, _attackDelay * 1.05f);

        if (_isMoving)
        {
            // initSpeed 속도로 시작하여 느려지며 특정 X좌표까지 이동
            float distanceToTarget = Vector2.Distance(transform.position, targetPos);

            float speed = Mathf.Lerp(0, initSpeed, distanceToTarget / initDistance);

            Vector2 direction = (targetPos - (Vector2)transform.position).normalized;

            rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);

            if (distanceToTarget < 0.1f)
            {
                rb.velocity = Vector2.zero;
                _isMoving = false;
            }
        }
    }
    void Attack()
    {

    }

}
