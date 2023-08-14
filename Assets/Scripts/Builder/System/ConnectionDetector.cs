using System;
using System.Collections.Generic;
using Part;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Builder.System
{
    [Serializable]
    class DetectDirection
    {
        public bool Up;
        public bool Down;
        public bool Left;
        public bool Right;
    }

    public class ConnectionDetector : MonoBehaviour
    {
        [SerializeField] private PartInfo partInfo;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float detectWidth;
        [SerializeField] private float unit = 0.5f;
        [SerializeField] private Vector3 fixOffset;
        [SerializeField] private DetectDirection direction;

        public int ConnectionCount => connections.Count;
        public event Action OnDisconnected;

        private readonly List<Connection> connections = new List<Connection>();
        private readonly Collider2D[] detectBuffer = new Collider2D[8];

        private const float avoidance = 0.1f;
        private float currentRotation;
        private bool isDragging;

        void Update()
        {
            if (!isDragging)
                return;

            if (Input.GetMouseButtonDown(1))
            {
                currentRotation += 90f;
                currentRotation = Mathf.Repeat(currentRotation, 360f);
            }
        }

        public void OnBeginDrag()
        {
            //イテレート中のリスト変更を防ぐためにコピー
            List<Connection> copy = new List<Connection>(connections);

            foreach (Connection connection in copy)
            {
                connection.Disconnect();
            }

            connections.Clear();
            isDragging = true;
        }

        public void OnEndDrag()
        {
            isDragging = false;
        }

        public bool Detect()
        {
            //上下左右にPartがあった場合にJointをアタッチする
            int rightCount = direction.Right ? DetectRange(GetDetectRangeRight()) : 0;
            Connect(rightCount);

            int leftCount = direction.Left ? DetectRange(GetDetectRangeLeft()) : 0;
            Connect(leftCount);

            int upCount = direction.Up ? DetectRange(GetDetectRangeUp()) : 0;
            Connect(upCount);

            int downCount = direction.Down ? DetectRange(GetDetectRangeDown()) : 0;
            Connect(downCount);

            return rightCount + leftCount + upCount + downCount > 0;
        }

        void Connect(int count)
        {
            if (count == 0)
                return;

            for (int i = 0; i < count; i++)
            {
                GameObject part = detectBuffer[i].transform.gameObject;

                if (part.transform.parent == null)
                    return;

                GameObject parent = part.transform.parent.gameObject;

                PartElement element = default;
                if (part.TryGetComponent(out element) || parent.TryGetComponent(out element))
                {
                    if (part == gameObject || parent == gameObject)
                    {
                        continue;
                    }

                    //相手側にJointをアタッチ
                    FixedJoint2D targetJoint = part.AddComponent<FixedJoint2D>();
                    targetJoint.connectedBody = rb;

                    //自分側にJointをアタッチ
                    FixedJoint2D selfJoint = gameObject.AddComponent<FixedJoint2D>();
                    selfJoint.connectedBody = part.GetComponent<Rigidbody2D>();

                    CreateConnection(part, selfJoint, targetJoint);
                }
            }
        }

        void CreateConnection(GameObject partObj, FixedJoint2D selfJoint, FixedJoint2D targetJoint)
        {
            Connection connection = new Connection(selfJoint, targetJoint);
            ConnectionDetector detector = partObj.GetComponent<ConnectionDetector>();
            AddConnection(connection);
            detector.AddConnection(connection);
        }

        private void AddConnection(Connection connection)
        {
            connections.Add(connection);
            connection.OnDisconnected += () =>
            {
                connections.Remove(connection);
                OnDisconnected?.Invoke();
            };
        }

        int DetectRange((Vector2 origin, Vector2 halfExtents) range)
        {
            return Physics2D.OverlapBoxNonAlloc(range.origin, range.halfExtents, 0f, detectBuffer);
        }

        float ToOffset(float origin)
        {
            return origin * 0.5f * unit + detectWidth * 0.5f + avoidance;
        }

        Vector3 GetSpriteOffset()
        {
            return (Vector2)(partInfo.Size - Vector2Int.one) * 0.25f;
        }

        Vector2 GetExtentsUp()
        {
            bool isFirstRot = currentRotation == 0f || currentRotation == 180f;

            if (isFirstRot)
            {
                return new Vector2(partInfo.Size.x * 0.5f - avoidance, detectWidth * 0.5f);
            }
            else
            {
                return new Vector2(detectWidth * 0.5f, partInfo.Size.x * 0.5f - avoidance);
            }
        }

        Vector2 GetExtentsRight()
        {
            bool isFirstRot = currentRotation == 0f || currentRotation == 180f;

            if (isFirstRot)
            {
                return new Vector2(detectWidth * 0.5f, partInfo.Size.y * 0.5f - avoidance);
            }
            else
            {
                return new Vector2(partInfo.Size.y * 0.5f - avoidance, detectWidth * 0.5f);
            }
        }

        (Vector2 origin, Vector2 halfExtents) GetDetectRangeRight()
        {
            Vector2 halfExtents = GetExtentsRight();

            float radius = partInfo.Size.x * 0.5f;
            float x = Mathf.Cos(-currentRotation * Mathf.Deg2Rad) * radius;
            float y = Mathf.Sin(-currentRotation * Mathf.Deg2Rad) * radius;

            return (transform.position + new Vector3(x, y), halfExtents);
        }

        (Vector2 origin, Vector2 halfExtents) GetDetectRangeUp()
        {
            Vector2 origin = (Vector2)transform.position + GetOverlapOffset();
            Vector2 halfExtents = GetExtentsUp();

            Vector2 offset;

            if (currentRotation == 0f || currentRotation == 180f)
            {
                offset = new Vector3(0f, ToOffset(partInfo.Size.y));
            }
            else
            {
                offset = new Vector3(ToOffset(partInfo.Size.x) - 0.5f, 0f);
            }

            return (origin + offset, halfExtents);
        }

        (Vector2 origin, Vector2 halfExtents) GetDetectRangeLeft()
        {
            Vector2 halfExtents = GetExtentsRight();

            float radius = -0.5f;
            float x = Mathf.Cos(-currentRotation * Mathf.Deg2Rad) * radius;
            float y = Mathf.Sin(-currentRotation * Mathf.Deg2Rad) * radius;

            return (transform.position + new Vector3(x, y), halfExtents);
        }

        (Vector2 origin, Vector2 halfExtents) GetDetectRangeDown()
        {
            Vector2 origin = (Vector2)transform.position + GetOverlapOffset();
            Vector2 halfExtents = GetExtentsUp();

            Vector2 offset;

            if (currentRotation == 0f || currentRotation == 180f)
            {
                offset = new Vector3(0f, -ToOffset(partInfo.Size.y));
            }
            else
            {
                offset = new Vector3(-ToOffset(partInfo.Size.x) + 0.5f, 0f);
            }

            return (origin + offset, halfExtents);
        }

        private Collider2D[] overlapBuffer = new Collider2D[4];

        public bool IsOverlap()
        {
            Vector3 detectOffset = GetOverlapOffset();
            Vector2 size = GetOverlapSize();

            int count = Physics2D.OverlapBoxNonAlloc(transform.position + detectOffset + fixOffset, size, 0f, overlapBuffer);

            return count > 1;
        }

        Vector2 GetOverlapSize()
        {
            Vector2 size = default;
            if (currentRotation == 0f || currentRotation == 180f)
            {
                size = new Vector2(partInfo.Size.x * 0.5f - 0.3f, partInfo.Size.y * 0.5f - 0.3f);
            }
            else
            {
                size = new Vector2(partInfo.Size.y * 0.5f - 0.3f, partInfo.Size.x * 0.5f - 0.3f);
            }

            return size;
        }

        Vector2 GetOverlapOffset()
        {
            float radius = partInfo.Size.x - 1f;
            float x = Mathf.Cos(-currentRotation * Mathf.Deg2Rad) * radius;
            float y = Mathf.Sin(-currentRotation * Mathf.Deg2Rad) * radius;

            return new Vector2(x, y) * 0.25f;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            if (direction.Up)
            {
                var range = GetDetectRangeUp();
                Gizmos.DrawWireCube(range.origin, range.halfExtents);
            }

            if (direction.Left)
            {
                var range = GetDetectRangeLeft();
                Gizmos.DrawWireCube(range.origin, range.halfExtents);
            }

            if (direction.Down)
            {
                var range = GetDetectRangeDown();
                Gizmos.DrawWireCube(range.origin, range.halfExtents);
            }

            if (direction.Right)
            {
                var range = GetDetectRangeRight();
                Gizmos.DrawWireCube(range.origin, range.halfExtents);
            }

            DrawOverlap();
        }

        //重なり判定用のデバッグ
        private void DrawOverlap()
        {
            Vector3 detectOffset = GetOverlapOffset();
            Vector2 size = GetOverlapSize();

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + detectOffset + fixOffset, size);
        }
    }
}