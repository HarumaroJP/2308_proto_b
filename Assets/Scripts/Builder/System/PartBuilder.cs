using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Part;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace Builder.System
{
    /// <summary>
    /// パーツ全体を保持、実行するクラス
    /// </summary>
    public class PartBuilder : MonoBehaviour
    {
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private Vector2 rangeOffset;
        [SerializeField] private Vector2 snapOffset;
        [SerializeField] private Transform stationaryBox;
        [SerializeField] private Transform trashBox;
        [SerializeField] private float trashDuration;
        [SerializeField] private Ease trashMoveEase;
        [SerializeField] private Ease trashScaleEase;

        private GameObject playerObject;
        private GameObject replicatedObject;

        /// <summary>
        /// グリッドのサイズ
        /// </summary>
        public Vector2Int GridSize => gridSize;

        /// <summary>
        /// ビルド時のオフセット
        /// </summary>
        public Vector2 RangeOffset => rangeOffset;

        public Vector2 SnapOffset => snapOffset;

        /// <summary>
        /// 全体のパーツ
        /// </summary>
        private ReactiveCollection<PartElement> partElements;

        public IReadOnlyReactiveCollection<PartElement> CurrentParts => partElements;

        public IObservable<CollectionAddEvent<PartElement>> OnPartAdded;
        public IObservable<CollectionRemoveEvent<PartElement>> OnPartRemoved;

        public event Action OnStart;
        public event Action OnRetry;


        public bool IsPlaying { get; private set; }

        public void Initialize()
        {
            partElements = new ReactiveCollection<PartElement>();
            OnPartAdded = partElements.ObserveAdd();
            OnPartRemoved = partElements.ObserveRemove();

            CreatePlayerObject();
        }

        void CreatePlayerObject()
        {
            playerObject = new GameObject("Player");
            playerObject.tag = "Player";
            playerObject.AddComponent<Player>();
        }

        public void Play()
        {
            if (IsPlaying && partElements.Count == 0)
                return;

            replicatedObject = Instantiate(playerObject);
            playerObject.SetActive(false);
            PartElement[] elements = replicatedObject.GetComponentsInChildren<PartElement>();

            foreach (PartElement element in elements)
            {
                foreach (Rigidbody2D rig in element.Rigidbody)
                {
                    rig.isKinematic = false;
                    rig.constraints = RigidbodyConstraints2D.None;
                    rig.gravityScale = 1f;
                }

                //始まったら動かせない
                element.LockState = true;
            }

            replicatedObject.GetComponent<Player>().Initialize();

            IsPlaying = true;

            gameObject.SetActive(false);
            OnStart?.Invoke();
        }

        public void Retry()
        {
            if (!IsPlaying)
                return;

            Destroy(replicatedObject);
            playerObject.SetActive(true);
            IsPlaying = false;

            gameObject.SetActive(true);
            OnRetry?.Invoke();
        }

        public void AddElement(PartElement partElement)
        {
            partElements.Add(partElement);
            partElement.transform.SetParent(playerObject.transform);
            AudioManager.Instance.PlaySe(AudioClipName.PartsBuild);
        }

        public void RemoveElement(PartElement partElement)
        {
            partElements.Remove(partElement);
            partElement.transform.parent = stationaryBox;
        }

        public async void Trash(PartElement partElement)
        {
            partElement.transform
                .DOLocalMove(trashBox.transform.position, trashDuration)
                .SetEase(trashMoveEase)
                .Play();

            await partElement.transform
                .DOScale(Vector2.zero, trashDuration)
                .SetEase(trashScaleEase, 2, 1)
                .Play()
                .AwaitForComplete();

            RemoveElement(partElement);
            Destroy(partElement.gameObject);
        }
    }
}