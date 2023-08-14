using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Part
{
    public class BindElement : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private StationaryPart part;
        public bool Enabled { get; set; }

        [SerializeField] private SpriteRenderer[] target;
        [SerializeField] private Sprite[] rightSprite;
        [SerializeField] private Sprite[] leftSprite;

        public event Action<StationaryPart, BindElement> OnBindSelected;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!Enabled)
                return;

            OnBindSelected?.Invoke(part, this);
        }

        public void ChangeBindState(int mouseButtonKey)
        {
            for (int i = 0; i < target.Length; i++)
            {
                target[i].sprite = mouseButtonKey == 0 ? leftSprite[i] : rightSprite[i];
            }
        }
    }
}