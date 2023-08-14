// --------------------------------------------------------- 
// EnemyBullet.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 

using System;
using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour
{
    #region variable

    private Rigidbody2D _rb;
    [SerializeField] private float _speed = 10F;
    [SerializeField] private int _attack = 7;
    [SerializeField] private ParticleSystem _attackEffect = default;

    private ResultActivator resultActivator;

    #endregion

    #region property

    #endregion

    #region method

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _rb.AddForce(gameObject.transform.rotation * new Vector3(-_speed, 0, 0), ForceMode2D.Impulse);

        StartCoroutine(BulletLife());

        resultActivator = FindObjectOfType<ResultActivator>();
        resultActivator.OnDefeat += OnDefeat;
    }


    private void OnDefeat()
    {
        resultActivator.OnDefeat -= OnDefeat;
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        resultActivator.OnDefeat -= OnDefeat;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        //{
        //    enemy.SendDamage(_attack);

        //    if (_attackEffect is not null)
        //    {
        //        Instantiate(_attackEffect, this.transform.position, this.transform.rotation).Play();
        //    }

        //    Destroy(this);
        //}


        if (collision.TryGetComponent<IDamage>(out IDamage damage) && !collision.TryGetComponent<Enemy>(out Enemy stageBlock))
        {
            damage.SendDamage(1);
            Destroy(this.gameObject);
        }
    }

    private IEnumerator BulletLife()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }

    #endregion
}