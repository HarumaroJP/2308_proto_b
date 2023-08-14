using System.Collections.Generic;
using Part;
using UnityEngine;

namespace Builder.System
{
    public class PartBinder : MonoBehaviour
    {
        [SerializeField] private PartBuilder partBuilder;
        private List<BindElement> currentBinds;
        private bool isBinding;
        private int currentBindKey;

        private void Start()
        {
            currentBinds = new List<BindElement>();

            partBuilder.OnPartAdded += OnPartAdded;
            partBuilder.OnPartRemoved += OnPartRemoved;
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

            isBinding = !isBinding;
            currentBindKey = mouseButtonKey;

            foreach (BindElement bindElement in currentBinds)
            {
                bindElement.Enabled = isBinding;
            }

            foreach (PartElement element in partBuilder.CurrentParts)
            {
                element.LockState = isBinding;
            }

            return isBinding;
        }

        private void OnBindSelected(StationaryPart part, BindElement bindElement)
        {
            part.ActionButton = currentBindKey;
            bindElement.ChangeBindState(currentBindKey);
        }
    }
}