using System;
using System.Collections.Generic;
using Builder.Core;
using Part;
using UniRx;
using UnityEngine;

namespace Builder.System
{
    public class PartBinder : MonoBehaviour
    {
        private List<BindElement> currentBinds;
        private bool isBinding;
        private int currentBindKey;

        private Subject<Unit> forceDisable;
        private PartBuilder partBuilder;
        public IObservable<Unit> ForceDisable;

        private void Awake()
        {
            currentBinds = new List<BindElement>();
            forceDisable = new Subject<Unit>().AddTo(this);
            ForceDisable = forceDisable;
        }

        private void Start()
        {
            partBuilder = ServiceLocator.Instance.Resolve<PartBuilder>();

            partBuilder.OnPartAdded.Subscribe(ev => OnPartAdded(ev.Value)).AddTo(this);
            partBuilder.OnPartRemoved.Subscribe(ev => OnPartRemoved(ev.Value)).AddTo(this);
            partBuilder.OnStart.Subscribe(_ =>
                {
                    SetBinderState(false);
                    forceDisable.OnNext(Unit.Default);
                })
                .AddTo(this);
        }

        private void OnPartAdded(PartElement element)
        {
            //パーツクリック時のイベント登録
            BindElement bindElement = element.GetComponent<BindElement>();

            if (bindElement != null)
            {
                bindElement.OnBindSelected += OnBindSelected;
                currentBinds.Add(bindElement);
            }
        }

        private void OnPartRemoved(PartElement element)
        {
            //パーツクリック時のイベント解除
            BindElement bindElement = element.GetComponent<BindElement>();


            if (bindElement != null)
            {
                bindElement.OnBindSelected -= OnBindSelected;
                currentBinds.Remove(element.GetComponent<BindElement>());
            }
        }

        public bool TrySwitchBindState(int mouseButtonKey)
        {
            //バインド中に他のバインドは受け付けない
            if (isBinding && currentBindKey != mouseButtonKey)
                return false;

            SetBinderState(!isBinding);
            currentBindKey = mouseButtonKey;

            return isBinding;
        }

        private void SetBinderState(bool state)
        {
            isBinding = state;

            foreach (BindElement bindElement in currentBinds)
            {
                bindElement.Enabled = state;
            }

            foreach (PartElement element in partBuilder.CurrentParts)
            {
                element.LockState = state;
            }
        }

        private void OnBindSelected(StationaryPart part, BindElement bindElement)
        {
            part.ActionButton = currentBindKey;
            bindElement.ChangeBindState(currentBindKey);
        }
    }
}