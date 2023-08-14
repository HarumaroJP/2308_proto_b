// --------------------------------------------------------- 
// PlayerController.cs 
// 
// CreateDay: 23/08/04
// Creator  : Ryuto Ohmori
// --------------------------------------------------------- 
using System;
using UnityEngine;
using System.Collections.Generic;
public class PlayerController : MonoBehaviour
{
    #region variable 
    [SerializeField]
    private List<StationaryPart> _parts = new List<StationaryPart>();

    private Action _leftMouse;
    private Action _rightMouse;

    private List<TapeController> _tape = new List<TapeController>();

    #endregion
    #region property

    public List<StationaryPart> Parts
    {
        get => _parts;
        set
        {
            _parts = value;
            UpdateMouseParts(_parts);
        }
    }

    #endregion
    #region method
 
    private void Awake()
    {
        foreach (Transform n in this.transform)
        {
            _parts.Add(n.gameObject.GetComponent<StationaryPart>());
        }
    }
 
    private void Start ()
    {
        UpdateMouseParts(_parts);
    }

    private void Update ()
    {
        if (Input.GetMouseButtonDown(0)) // 左クリック
        {
            _leftMouse.Invoke();
        }
        else if (Input.GetMouseButtonDown(1)) // 左クリック
        {
            _rightMouse();
        } 
    }

    private void UpdateMouseParts(List<StationaryPart> parts)
    {
        _leftMouse = null;
        _rightMouse = null;

        foreach(var n in parts)
        {
            if (n.ActionButton == 0)
            {
                _leftMouse += n.Action;
            }
            else
            {
                _rightMouse += n.Action;
            }

            if (n is TapeController)
            {
                _tape.Add((TapeController)n);
            }
        }
    }
    #endregion
}