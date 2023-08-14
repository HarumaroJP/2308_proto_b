// --------------------------------------------------------- 
// StageBlock.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class BlackBord : Enemy
{
    #region variable 

    [SerializeField]
    private GameObject _effect;
    private bool _canBreak = true;
    DestructionRateController destructionRateController = default;

    private EnemyCount enemyCount;
    #endregion
    #region property
    #endregion
    #region method

    private void Awake()
    {
        _enemyData._enemyPosition = this.transform.position;
        _enemyData._enemyRotation = this.transform.rotation;
        _enemyData._enemyState = EnemyState.blackBorad;


    }

    private void Start()
    {



        destructionRateController = GameObject.FindAnyObjectByType<DestructionRateController>();

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        float power = collision.relativeVelocity.x > collision.relativeVelocity.y ? collision.relativeVelocity.x : collision.relativeVelocity.y;

        _hp -= CollisionDamage((int)power);

        DestroyObject();

        if (collision.gameObject.TryGetComponent<EnemyManager>(out EnemyManager enemy))
        {
            enemy.SendDamage((int)power * 1000);
        }

    }

    ///// <summary>
    ///// オブジェクトにHPがあるなら
    ///// </summary>
    private void DestroyObject()
    {
        if (_canBreak)
        {
            if (_hp <= 0)
            {
                Instantiate(_effect, transform.position, transform.rotation);
                if (destructionRateController != null) destructionRateController.DecrementEnemyParts();
                Debug.Log("AAAAAAA");
                Destroy(this.gameObject);
                _canBreak = false;
            }
        }
    }

    protected virtual int CollisionDamage(int damage)
    {
        return damage / 4;
    }

    public override void SendDamage(int damage)
    {
        _hp -= damage;
        DestroyObject();
    }
    #endregion
}