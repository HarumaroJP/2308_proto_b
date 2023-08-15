// --------------------------------------------------------- 
// EnemyManager.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 

using UnityEngine;
using System.Collections;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;
using UniRx;

public class EnemyManager : Enemy
{
    //�U���̏��
    [SerializeField] AttackInfo _attackInfo = default;

    private List<GameObject> bullets = new List<GameObject>();
    private Rigidbody2D _rigidbody2D = default;

    DestructionRateController destructionRateController = default;

    private ResultActivator resultActivator;

    private bool canShot = false;

    private Player player;

    #region method

    private void Awake()
    {
        _enemyData._enemyPosition = this.transform.position;
        _enemyData._enemyRotation = this.transform.rotation;
        _enemyData._enemyState = EnemyState.enemy;

        StateMachine.Instance.CurrentSceneType
    .Subscribe(scene =>
    {
        if (scene == SceneType.InGame)
        {
            GetPlayer();
        }
    });

    }

    private void GetPlayer()=> player = GameObject.FindWithTag("Player").GetComponent<Player>();


    private void Start()
    {
        //���W�b�h�{�f�B2D���擾
        _rigidbody2D = this.GetComponent<Rigidbody2D>() ? this.GetComponent<Rigidbody2D>() : this.gameObject.AddComponent<Rigidbody2D>();

        CancellationToken ct = this.GetCancellationTokenOnDestroy();

        AttackLoopAsync(ct).Forget();

        destructionRateController = GameObject.FindAnyObjectByType<DestructionRateController>();


        resultActivator = FindObjectOfType<ResultActivator>();
        resultActivator.OnDefeat += OnDefeat;

    }

    public override void SendDamage(int damage)
    {
        _hp -= damage;

        //���S
        if (_hp <= 0)
        {
            if (destructionRateController != null) destructionRateController.DecrementEnemy();
            Destroy(this.gameObject);
        }
    }

    private async UniTask AttackLoopAsync(CancellationToken ct)
    {
        if (_attackInfo.bulletQuantity == 0) return;

        if (_attackInfo == default)
        {
            Debug.LogError(this.gameObject.name + "��AttackInfo���A�^�b�`����Ă��Ȃ��̂ōU�����܂���");
            return;
        }

        if (_attackInfo.bullet == null)
        {
            Debug.LogError(_attackInfo.name + "��bullet���A�^�b�`����Ă��Ȃ��̂ōU�����܂���");
            return;
        }

        while (!ct.IsCancellationRequested)
        {
            //�C���^�[�o��
            await UniTask.Delay(TimeSpan.FromSeconds(_attackInfo.interval));

            canShot = true;

            await AttackAsync(ct);
        }
    }

    private async UniTask AttackAsync(CancellationToken ct)
    {
        if (player is null)
        {
            return;
        }


        IReadOnlyList<StationaryPart> stationaryParts = player.Parts;

        int targetPart = UnityEngine.Random.Range(0, stationaryParts.Count);

        //���ˉ�
        int quantity = _attackInfo.bulletQuantity;

        //for(int i = 0; i < stationaryParts.Count; i++)
        //{
        //    if(stationaryParts[i] is TapeController)
        //    {
        //        targetPart = i;
        //    }
        //}

        Vector3 bulletTransform;
        try
        {
            bulletTransform = stationaryParts[targetPart].gameObject.transform.position;
        }
        catch
        {
            return;
        }

        //Vector3 bulletTransform = stationaryParts[targetPart].gameObject.transform.position;
        while (quantity > 0)
        {
            if (!ct.IsCancellationRequested && canShot)
            {
                GameObject bullet = Instantiate(_attackInfo.bullet, this.transform.position, this.transform.rotation);

                bullet.GetComponent<Rigidbody2D>().gravityScale = 0f;


                Vector3 toDirection = bulletTransform - bullet.transform.position;
                // �Ώە��։�]����
                bullet.transform.rotation = Quaternion.FromToRotation(Vector3.left, toDirection);

                bullets.Add(bullet);
            }

            quantity--;

            //���˃��[�g
            await UniTask.Delay(TimeSpan.FromSeconds(_attackInfo.bulletInterval));

            await UniTask.Yield(ct);
        }

        return;
    }

    private void OnDefeat() => canShot = false;

    #endregion
}