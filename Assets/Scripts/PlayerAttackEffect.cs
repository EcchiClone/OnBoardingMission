using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackEffect : MonoBehaviour
{

    public void ActiveColliderOnAnimation()
    {
        if(TryGetComponent(out BoxCollider2D col))
        {
            col.enabled = true;
        }
    }
    public void DestoryOnAnimation()
    {
        Destroy(gameObject);
    }
}
