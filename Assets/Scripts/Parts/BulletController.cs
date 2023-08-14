// --------------------------------------------------------- 
// BulletController.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 

using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour
{
    #region variable

    private Rigidbody2D _rb;

    [SerializeField] private float _speed = 10F;
    [SerializeField] private int _attack = 7;
    [SerializeField] private Vector2 dirOffset = default;
    [SerializeField] private SpriteRenderer spriteRenderer = default;
    [SerializeField] private ParticleSystem _attackEffect = default;

    [SerializeField] private Sprite leftSprite;
    [SerializeField] private Sprite rightSprite;

    private Transform _transform;

    // 前フレームのワールド位置
    private Vector3 _prevPosition;

    #endregion

    #region property

    #endregion

    #region method

    public void Initialize(Vector2 direction, int mouseButtonKey)
    {
        spriteRenderer.sprite = mouseButtonKey == 0 ? leftSprite : rightSprite;
        _rb = GetComponent<Rigidbody2D>();

        _rb.AddForce((direction + dirOffset).normalized * _speed, ForceMode2D.Impulse);
        Debug.Log(direction);
        transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);
        _prevPosition = transform.position;

    }

    private void Update()
    {
        Vector3 nowPosition = transform.position;

        Vector3 delta = nowPosition - _prevPosition;

        _prevPosition = transform.position;
        if(delta == Vector3.zero)
        {
            return;
        }
        transform.rotation = Quaternion.FromToRotation(Vector3.right,delta);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.SendDamage(_attack);

            if (_attackEffect)
            {
                Instantiate(_attackEffect, this.transform.position, this.transform.rotation).Play();
            }

            Destroy(this.gameObject);
        }
    }

    #endregion
}