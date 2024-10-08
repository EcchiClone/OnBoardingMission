using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [SerializeField] int _damage = 100;
    [SerializeField] float _attackDelay = 1f;

    float _attackTimer;

    Rigidbody2D _rb;

    Vector2 _targetPos;
    float _initSpeed;
    float _initDistance;

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
        _animator = GetComponent<Animator>();

        // 등장 연출 관련
        _rb = GetComponent<Rigidbody2D>();
        _isMoving = true;
        _animator.SetBool("isMoving", true);
        _targetPos = new Vector2(-4, transform.position.y);
        _initSpeed = 5f;
        _initDistance = Vector2.Distance(transform.position, _targetPos);
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
            float distanceToTarget = Vector2.Distance(transform.position, Vector2.right * _targetPos.x + Vector2.up * transform.position.y);

            float speed = Mathf.Lerp(0, _initSpeed, distanceToTarget / _initDistance) + 0.2f;

            Vector2 direction = (_targetPos - (Vector2)transform.position).normalized;

            _rb.velocity = new Vector2(direction.x * speed, _rb.velocity.y);

            if (distanceToTarget < 0.1f)
            {
                _rb.velocity = Vector2.zero;
                _isMoving = false;
                _animator.SetBool("isMoving", false);
            }
        }
    }
    void Attack()
    {
        if(_attackTimer > _attackDelay)
        {
            int numColliders = Physics2D.OverlapCircleNonAlloc(transform.position, 9.5f, colliders, layerMask: LayerMask.GetMask("Enemy"));
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
        if(colliders[0] != null)
        {
            Instantiate(_attackEffectPrefab, colliders[0].transform.position, Quaternion.identity, HierachyCategory.parentsDict["Entity"].transform);
        }
    }

}

enum PlayerState
{
    Idle = 0,
    Attack = 1,
}