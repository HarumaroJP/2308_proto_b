using System;
using System.Collections.Generic;
using System.Linq;
using Builder.System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Part
{
    public class PartElement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private Rigidbody2D[] rb;
        [SerializeField] private PartBuilder partBuilder;
        [SerializeField] private List<ConnectionDetector> detectors;
        [SerializeField] private PartInfo partInfo;

        public bool isDragging;
        private Camera cam;
        private Vector2 offset;
        public IReadOnlyList<Rigidbody2D> Rigidbody => rb;
        public bool LockState { get; set; }

        public bool IsInitialState { get; private set; } = true;
        public PartInfo PartInfo => partInfo;
        public event Action<PartElement> OnPartPutOut;

        private void Start()
        {
            cam = Camera.main;

            foreach (ConnectionDetector detector in detectors)
            {
                detector.OnDisconnected += RemoveIfDependent;
            }
        }

        private void Update()
        {
            if (isDragging && (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space)))
            {
                transform.localRotation *= Quaternion.Euler(0f, 0f, -90f);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (isDragging || LockState)
                return;


            previousSnapPos = Vector2.zero;

            if (IsInitialState)
            {
                Instantiate(gameObject, transform.parent);
            }

            else
            {
                OnPartPutOut?.Invoke(this);
            }

            isDragging = true;
            IsInitialState = false;
            foreach (Rigidbody2D rig in rb)
            {
                rig.isKinematic = false;
            }

            offset = transform.position - cam.ScreenToWorldPoint(eventData.position);

            InitDetectors();

            partBuilder.RemoveElement(this);
        }

        private Vector2 previousSnapPos;

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDragging || LockState)
                return;

            Vector2 pos = cam.ScreenToWorldPoint(eventData.position);
            Vector2 next = pos + offset;

            //盤面内に入っていたらスナップ
            if (InRange(pos + offset))
            {
                next = Snap(pos + offset, 0.5f);

                if (previousSnapPos != next)
                {
                    AudioManager.Instance.PlaySe(AudioClipName.GridSlide);
                    previousSnapPos = next;
                }
            }

            transform.position = next;
        }

        void InitDetectors()
        {
            foreach (ConnectionDetector detector in detectors)
            {
                detector.OnBeginDrag();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!isDragging || LockState)
                return;

            isDragging = false;

            foreach (ConnectionDetector detector in detectors)
            {
                detector.OnEndDrag();
            }

            bool outOfRange = !InRange(transform.position);
            bool isOverlap = detectors.Any(detector => detector.IsOverlap());

            //範囲外 or 重なってたら削除
            if (outOfRange || isOverlap)
            {
                Trash();
                return;
            }

            foreach (Rigidbody2D rig in rb)
            {
                rig.isKinematic = true;
            }

            foreach (ConnectionDetector detector in detectors)
            {
                detector.Detect();
            }

            partBuilder.AddElement(this);
        }

        /// <summary>
        /// 全てのJointが絶たれたらリセットする
        /// </summary>
        void RemoveIfDependent()
        {
            if (isDragging)
                return;

            if (detectors.Sum(detector => detector.ConnectionCount) == 0)
            {
                OnPartPutOut?.Invoke(this);
                partBuilder.RemoveElement(this);
                Trash();
            }
        }

        void Trash()
        {
            partBuilder.Trash(this);
        }

        bool InRange(Vector2 point)
        {
            Vector2 origin = partBuilder.transform.position;
            Vector2 gridSize = partBuilder.GridSize;
            Vector2 rightUp = origin + gridSize * 0.25f + partBuilder.RangeOffset;
            Vector2 leftDown = origin - gridSize * 0.25f + partBuilder.RangeOffset;

            bool inRightUp = point.x < rightUp.x && point.y < rightUp.y;
            bool inLeftDown = point.x > leftDown.x && point.y > leftDown.y;

            return inRightUp && inLeftDown;
        }

        Vector2 Snap(Vector2 point, float interval)
        {
            float x = Mathf.Round(point.x / interval) * interval;
            float y = Mathf.Round(point.y / interval) * interval;

            return new Vector2(x, y) + partBuilder.SnapOffset;
        }
    }
}