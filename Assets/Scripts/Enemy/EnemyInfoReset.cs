// --------------------------------------------------------- 
// EnemyInfoReset.cs 
// 
// CreateDay: 
// Creator  : 
// --------------------------------------------------------- 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;

public class EnemyInfoReset : MonoBehaviour
{
    #region variable 

    public EnemyInfo EnemyInfo;

    private Enemy[] _enemys = default;
    private List<Enemy> Enemys = new List<Enemy>();
    private bool _isStart = false;
    private struct EnemyInformation
    {
        public Enemy.EnemyData EnemyData;
        public GameObject EnemyObject;

        public EnemyInformation(Enemy.EnemyData enemyData, GameObject enemyObject)
        {
            EnemyData = enemyData;
            EnemyObject = enemyObject;
        }
    }

    private List<EnemyInformation> _enemyInformation = new List<EnemyInformation>();


    #endregion
    #region property
    #endregion
    #region method

    private void Start()
    {
    }

    public void Reset()
    {
        Enemy[] e = GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        for (int i = 0; i < e.Length; i++)
        {
            e[i].Reset();
        }
        for (int i = 0; i < _enemyInformation.Count; i++)
        {
            EnemyInformation enemyInformation = _enemyInformation[i];
            GameObject obj = Instantiate(enemyInformation.EnemyObject, enemyInformation.EnemyData._enemyPosition, enemyInformation.EnemyData._enemyRotation);
        }
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.K))
        {
            Reset();
        }

        if (!_isStart)
        {
            _enemys = GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        }
        if (_enemys.Length >0)
        {
            EnemyInfoStart();
            _isStart = true;
            return;
        }
    }

    private void EnemyInfoStart()
    {
        if (_isStart)
        {
            return;
        }
        //_enemys = GameObject.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        for (int i = 0; i < _enemys.Length; i++)
        {
            Enemys.Add(_enemys[i]);
        }

        for (int i = 0; i < Enemys.Count; i++)
        {
            for (int k = 0; k < EnemyInfo.enemySettings.Count; k++)
            {
                if (EnemyInfo.enemySettings[k].EnemyState == Enemys[i]._enemyData._enemyState)
                {
                    _enemyInformation.Add(new EnemyInformation(Enemys[i]._enemyData, EnemyInfo.enemySettings[k].EnemyObject));
                }
            }
        }
        _isStart = true;
    }

    public void EnemyRemoved(Enemy enemy)
    {
        Enemys.Remove(enemy);
    }


    #endregion
}