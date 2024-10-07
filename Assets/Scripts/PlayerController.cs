using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    Collider2D[] colliders = new Collider2D[10];
    GameObject _attackEffectPrefab;

    Animator _animator;

    private void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        // 등장 연출 관련
        _isMoving = true;
        _animator.SetBool("isMoving", true);
        targetPos = new Vector2(-4, transform.position.y);
        initSpeed = 5f;
        initDistance = Vector2.Distance(transform.position, targetPos);
        _attackEffectPrefab = Resources.Load<GameObject>(DataIO.Paths["PlayerAttackEffect"]);

    }

    private void Update()
    {
        MoveToTarget();
        Attack();
    }

    private void MoveToTarget()
    {
        _attackTimer = Mathf.Min(_attackTimer + Time.deltaTime, _attackDelay * 1.1f);

        if (_isMoving)
        {
            // _initSpeed 속도로 시작하여 느려지며 특정 X좌표까지 이동
            float distanceToTarget = Vector2.Distance(transform.position, Vector2.right * targetPos.x + Vector2.up * transform.position.y);

            float speed = Mathf.Lerp(0, initSpeed, distanceToTarget / initDistance);

            Vector2 direction = (targetPos - (Vector2)transform.position).normalized;

            rb.velocity = new Vector2(direction.x * speed + 0.2f, rb.velocity.y);

            if (distanceToTarget < 0.1f)
            {
                rb.velocity = Vector2.zero;
                _isMoving = false;
                _animator.SetBool("isMoving", false);
            }
        }
    }
    void Attack()
    {
        if(_attackTimer > _attackDelay)
        {
            int numColliders = Physics2D.OverlapCircleNonAlloc(transform.position, 9f, colliders, layerMask: LayerMask.GetMask("Enemy"));
            if (numColliders > 0)
            {
                StartCoroutine(IAttackWithDelayEffect());

                _attackTimer -= _attackDelay;
            }
        }
    }
    IEnumerator IAttackWithDelayEffect()
    {
        _animator.SetTrigger("doAttack");
        yield return new WaitForSeconds(0.5f);
        Instantiate(_attackEffectPrefab, colliders[0].transform.position, Quaternion.identity, HierachyCategory.parentsDict["Entity"].transform);
    }

}

enum PlayerState
{
    Idle = 0,
    Attacking = 1,
}