using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private List<StationaryPart> parts = new List<StationaryPart>();
    public IReadOnlyList<StationaryPart> Parts => parts;

    private DestructionRateController _rateController;

    private bool initialized = false;

    public void Initialize()
    {
        _rateController = FindAnyObjectByType<DestructionRateController>();
        parts = GetComponentsInChildren<StationaryPart>().ToList();
        foreach (StationaryPart part in parts)
        {
            part.OnStart();

            part.DestructionRateController = _rateController;

            part.OnDestroyCallback += () =>
            {
                if (part.ActionButton == 0)
                {
                    _leftMouse -= part.Action;
                }
                else
                {
                    _rightMouse -= part.Action;
                }

                _rateController.DecrimentPlayerParts();

                parts.Remove(part);
            };
        }

        _rateController.InitPlayerPartsCount(parts.Count);

        UpdateMouseParts(parts);
        initialized = true;
    }

    private Action _leftMouse;
    private Action _rightMouse;

    private void Update()
    {
        if (!initialized)
            return;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftArrow)) // 左クリック
        {
            _leftMouse?.Invoke();
        }
        else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.RightArrow)) // 左クリック
        {
            _rightMouse?.Invoke();
        }
    }

    private void UpdateMouseParts(List<StationaryPart> parts)
    {
        _leftMouse = null;
        _rightMouse = null;

        foreach (var n in parts)
        {
            if (n.ActionButton == 0)
            {
                _leftMouse += n.Action;
            }
            else
            {
                _rightMouse += n.Action;
            }
        }
    }
}