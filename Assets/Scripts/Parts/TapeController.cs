// --------------------------------------------------------- 
// TapeController.cs 
// 
// CreateDay: 23/08/04
// Creator  : Ryuto Ohmori
// --------------------------------------------------------- 

using UnityEngine;

public class TapeController : StationaryPart
{
    #region variable

    [SerializeField] private float _moveSpeed = 5;
    [SerializeField] private ParticleSystem _breakEffect = default;

    Rigidbody2D _rb = default;
    private ResultActivator resultActivator;

    private bool _isGround = false;

    #endregion

    #region property

    #endregion

    #region method

    private void Awake()
    {
        _maxHp = _hp;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (_isGround)
        {
            float h = 0f;

            if (Input.GetKey(KeyCode.A))
            {
                h = -1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                h = 1f;
            }


            _rb.AddForce(new Vector2(h, 0) * _moveSpeed);
        }
    }

    public override void Break()
    {
        if (_breakEffect)
        {
            var temp = Instantiate(_breakEffect);
            temp.transform.position = this.transform.position;
        }

       // _rate.DecrimentPlayerParts();

        Destroy(this.gameObject);
    }

    public override void OnStart() { }

    public override void SendDamage(int damage)
    {
        // hp�����炷����
        _hp -= damage;

        // hp��0�������ۂ̏���
        if (_hp <= 0)
        {
            Break();
        }
    }

    public override void Action() { }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGround = false;
        }
    }

    #endregion
}