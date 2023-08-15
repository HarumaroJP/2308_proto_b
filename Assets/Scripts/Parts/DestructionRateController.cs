// --------------------------------------------------------- 
// DestructionRateController.cs 
// 
// CreateDay: 23/08/05
// Creator  : Ryuto Ohmori
// --------------------------------------------------------- 

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UniRx;
using DG.Tweening;

public class DestructionRateController : MonoBehaviour
{
    #region variable

    [SerializeField, Tooltip("プレイヤーの破壊率を表示するUI")]
    TextMeshProUGUI _playerRateText = default;

    [SerializeField, Tooltip("ブロックの破壊率を表示するUI")]
    TextMeshProUGUI _enemyBlockRateText = default;

    [SerializeField, Tooltip("敵の破壊率を表示するUI")]
    TextMeshProUGUI _enemyRateText = default;


    private int _enemyBlockMaxCount;
    private int _enemyBlockCurrentCount;

    private int _playerMaxCount;
    private int _playerCurrentCount;

    private float _enemyBlockCurrentRate;
    private float _playerCurrentRate;
    private float _enemyCurrentRate;

    private int _enemyMaxCount;
    private int _enemyCurrentCount;

    private bool _isShaking = false;

    private Transform _camera;

    #endregion

    #region property

    private int EnemyCurrentHP
    {
        get => _enemyBlockCurrentCount;

        set
        {
            _enemyBlockCurrentCount = value;
            _enemyBlockCurrentRate = (float)_enemyBlockCurrentCount / _enemyBlockMaxCount;
            _enemyHpRate.Value = _enemyBlockCurrentRate;
            Debug.Log(_enemyBlockCurrentRate);
        }
    }

    private int PlayerCurrentHP
    {
        get => _playerCurrentCount;
        set
        {
            _playerCurrentCount = value;
            _playerCurrentRate = (float)_playerCurrentCount / _playerMaxCount;
        }
    }

    private int EnemyHp
    {
        get => _enemyCurrentCount;

        set
        {
            _enemyCurrentCount = value;
            _enemyCurrentRate = (float)_enemyCurrentCount / _enemyBlockMaxCount;
            _enemyHpRate.Value = _enemyBlockCurrentRate;
#if UNITY_EDITOR
            Debug.Log("enemy rate: " + _enemyBlockCurrentRate);
#endif
        }
    }

    //Block
    public IReactiveProperty<Unit> OnEnemyDead => _onEnemyDead;
    private IReactiveProperty<Unit> _onEnemyDead = new ReactiveProperty<Unit>();

    //Enemy
    public IReactiveProperty<Unit> OnDead => _onDead;
    private IReactiveProperty<Unit> _onDead = new ReactiveProperty<Unit>();

    public IReactiveProperty<Unit> OnPlayerDead => _onPlayerDead;
    private IReactiveProperty<Unit> _onPlayerDead = new ReactiveProperty<Unit>();

    //Block
    public float EnemyCurrentRate => _enemyBlockCurrentRate;
    public float PlayerCurrentRate => _playerCurrentRate;
    public IReactiveProperty<float> EnemyHpRate => _enemyHpRate;

    private IReactiveProperty<float> _enemyHpRate = new ReactiveProperty<float>();

    //敵
    public float EnemyRate => _enemyCurrentRate;

    #endregion

    #region method

    private void Awake()
    {
        _camera = Camera.main.transform;
    }

    /// <summary>EnemyのMaxCountを初期化する関数</summary>
    /// <param name="enemyMaxCount">敵のパーツの最大数</param>
    public void InitEnemyPartsCount(int enemyMaxCount)
    {
        _enemyBlockMaxCount = enemyMaxCount;
        EnemyCurrentHP = enemyMaxCount;
    }

    /// <summary>PlayerのMaxCountを初期化する関数</summary>
    /// <param name="PlayerMaxCount">Playerのパーツの最大数</param>
    public void InitPlayerPartsCount(int PlayerMaxCount)
    {
        _playerMaxCount = PlayerMaxCount;
        PlayerCurrentHP = PlayerMaxCount;
    }


    public void InitEnemyCount(int enemyMaxCount)
    {
        _enemyMaxCount = enemyMaxCount;
        EnemyHp = enemyMaxCount;
    }


    /// <summary>Enemyの数を減らす</summary>
    public void DecrementEnemyParts()
    {
        EnemyCurrentHP = _enemyBlockCurrentCount - 1;


        if (!_isShaking)
        {
            _isShaking = true;
            _camera.DOShakePosition(0.4F, 0.3F);
            _camera.DOShakeRotation(0.4F, 0.3f).OnComplete(() => _isShaking = false);
        }

        if (_enemyBlockRateText)
        {
            _enemyBlockRateText.text = (_enemyBlockCurrentRate * 100).ToString();
        }

        if (EnemyCurrentHP <= 0)
        {
            _onEnemyDead.Value = Unit.Default;
            AudioManager.Instance.PlaySe(AudioClipName.BreakParts);
        }
    }

    public void DecrementEnemy()
    {
        EnemyHp = _enemyCurrentCount - 1;

        if (!_isShaking)
        {
            _isShaking = true;

            var temp = _camera.position;

            _camera.DOShakePosition(0.4F, 0.3F);
            _camera.DOShakeRotation(0.4F, 0.3F)
                .OnComplete(() =>
                {
                    _isShaking = false;
                    _camera.position = temp;
                });
        }

        if (_enemyRateText)
        {
            _enemyRateText.text = (_enemyCurrentRate * 100).ToString();
        }

        if (EnemyHp <= 0)
        {
            _onDead.Value = Unit.Default;
            AudioManager.Instance.PlaySe(AudioClipName.BreakParts);
        }
    }

    /// <summary>Playerの数を減らす</summary>
    public void DecrimentPlayerParts()
    {
        PlayerCurrentHP -= 1;

        if (!_isShaking)
        {
            _isShaking = true;

            var temp = _camera.position;
            if (StateMachine.Instance.CurrentSceneType.Value == SceneType.InGame)
            {
                if (StateMachine.Instance.CurrentSceneType.Value == SceneType.InGame)
                {
                    if (StateMachine.Instance.CurrentSceneType.Value == SceneType.InGame)
                    {
                        _camera.DOShakePosition(0.4F, 0.3F);
                        _camera.DOShakeRotation(0.4F, 0.3F)
                            .OnComplete(() =>
                            {
                                _isShaking = false;
                                _camera.position = temp;
                            });
                    }
                }
            }
        }

        if (_playerRateText)
        {
            _playerRateText.text = (_playerCurrentRate * 100).ToString();
        }

        if (PlayerCurrentHP <= 0)
        {
            _onPlayerDead.Value = Unit.Default;
            AudioManager.Instance.PlaySe(AudioClipName.BreakParts);
        }
    }

    #endregion
}