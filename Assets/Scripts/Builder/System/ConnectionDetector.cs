using System;
using System.Collections.Generic;
using System.Linq;
using Part;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Builder.System
{
    [Flags]
    public enum Direction
    {
        Up = 1 << 0,
        Down = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3
    }

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
        [SerializeField] private Vector2 fixOffset;
        [SerializeField] private ConnectableDirection connectableDir;

        public int ConnectionCount => connections.Count;
        public event Action OnDisconnected;

        private readonly List<Direction> worldDirectionInfo = new List<Direction>()
        {
            Direction.Right, Direction.Down, Direction.Left, Direction.Up
        };

        private readonly List<Connection> connections = new List<Connection>();
        private readonly Collider2D[] detectBuffer = new Collider2D[8];

        private const float avoidance = 0.1f;
        private float currentRotation;
        private int currentRotationIndex;
        private bool isDragging;

        void Update()
        {
            if (!isDragging)
                return;

            if (Input.GetMouseButtonDown(1))
            {
                currentRotation += 90f;
                currentRotation = Mathf.Repeat(currentRotation, 360f);
                currentRotationIndex = (int)Mathf.Repeat(currentRotationIndex + 1, 4);
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

        public bool IsConnectable(Direction direction)
        {
            return connectableDir.IsConnectable(direction);
        }

        public bool Detect()
        {
            //上下左右にPartがあった場合にJointをアタッチする
            int rightCount = connectableDir.IsTrue(Direction.Right) ? DetectRange(GetDetectRangeRight()) : 0;
            Connect(rightCount, Direction.Right);

            int leftCount = connectableDir.IsTrue(Direction.Left) ? DetectRange(GetDetectRangeLeft()) : 0;
            Connect(leftCount, Direction.Left);

            int upCount = connectableDir.IsTrue(Direction.Up) ? DetectRange(GetDetectRangeUp()) : 0;
            Connect(upCount, Direction.Up);

            int downCount = connectableDir.IsTrue(Direction.Down) ? DetectRange(GetDetectRangeDown()) : 0;
            Connect(downCount, Direction.Down);

            return rightCount + leftCount + upCount + downCount > 0;
        }

        Direction ConvertToWorldDir(Direction direction)
        {
            int index = worldDirectionInfo.IndexOf(direction);

            return worldDirectionInfo[(int)Mathf.Repeat(currentRotationIndex + index, 4)];
        }

        void Connect(int count, Direction direction)
        {
            if (count == 0)
                return;

            for (int i = 0; i < count; i++)
            {
                GameObject part = detectBuffer[i].transform.gameObject;

                if (part.TryGetComponent(out ConnectionDetector detector))
                {
                    Direction worldDir = ConvertToWorldDir(direction);

                    if (part == gameObject || !detector.IsConnectable(worldDir))
                        continue;

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
            Vector3 offset = GetOverlapRightOffset();

            float radius = partInfo.Size.x * 0.5f + unit;
            float x = Mathf.Cos(-currentRotation * Mathf.Deg2Rad) * radius;
            float y = Mathf.Sin(-currentRotation * Mathf.Deg2Rad) * radius;

            return (transform.position + new Vector3(x, y) + offset, halfExtents);
        }

        (Vector2 origin, Vector2 halfExtents) GetDetectRangeUp()
        {
            Vector2 halfExtents = GetExtentsUp();
            Vector3 offset = GetOverlapUpOffset();

            float radius = partInfo.Size.y * 0.5f + unit;
            float x = Mathf.Cos((-currentRotation + 90f) * Mathf.Deg2Rad) * radius;
            float y = Mathf.Sin((-currentRotation + 90f) * Mathf.Deg2Rad) * radius;

            return (transform.position + new Vector3(x, y) + offset, halfExtents);
        }

        (Vector2 origin, Vector2 halfExtents) GetDetectRangeLeft()
        {
            Vector2 halfExtents = GetExtentsRight();
            Vector3 offset = GetOverlapRightOffset();

            float radius = -(0.5f + unit);
            float x = Mathf.Cos(-currentRotation * Mathf.Deg2Rad) * radius;
            float y = Mathf.Sin(-currentRotation * Mathf.Deg2Rad) * radius;

            return (transform.position + new Vector3(x, y) + offset, halfExtents);
        }

        (Vector2 origin, Vector2 halfExtents) GetDetectRangeDown()
        {
            Vector2 halfExtents = GetExtentsUp();
            Vector3 offset = GetOverlapUpOffset();

            float radius = -(0.5f + unit);
            float x = Mathf.Cos((-currentRotation + 90f) * Mathf.Deg2Rad) * radius;
            float y = Mathf.Sin((-currentRotation + 90f) * Mathf.Deg2Rad) * radius;

            return (transform.position + new Vector3(x, y) + offset, halfExtents);
        }

        private Collider2D[] overlapBuffer = new Collider2D[4];

        public bool IsOverlap()
        {
            Vector3 detectOffset = GetOverlapOffset();
            Vector2 size = GetOverlapSize();

            int count = Physics2D.OverlapBoxNonAlloc(transform.position + detectOffset, size, 0f, overlapBuffer);

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
            Vector2 upOffset = GetOverlapUpOffset();
            Vector2 rightOffset = GetOverlapRightOffset();

            return upOffset + rightOffset;
        }

        Vector2 GetOverlapUpOffset()
        {
            float radius = partInfo.Size.x - 1f;
            float rad = -currentRotation * Mathf.Deg2Rad;
            float x = Mathf.Cos(rad) * radius;
            float y = Mathf.Sin(rad) * radius;

            return new Vector2(x, y) * fixOffset.y * 0.25f;
        }

        Vector2 GetOverlapRightOffset()
        {
            float radius = partInfo.Size.x - 1f;
            float rad = (-currentRotation + 90f) * Mathf.Deg2Rad;
            float x = Mathf.Cos(rad) * radius;
            float y = Mathf.Sin(rad) * radius;

            return new Vector2(x, y) * fixOffset.x * 0.25f;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Vector2 fixOffset = this.fixOffset;

            if (connectableDir.IsTrue(Direction.Up))
            {
                var range = GetDetectRangeUp();
                Gizmos.DrawWireCube(range.origin, range.halfExtents);
            }

            if (connectableDir.IsTrue(Direction.Left))
            {
                var range = GetDetectRangeLeft();
                Gizmos.DrawWireCube(range.origin, range.halfExtents);
            }

            if (connectableDir.IsTrue(Direction.Down))
            {
                var range = GetDetectRangeDown();
                Gizmos.DrawWireCube(range.origin, range.halfExtents);
            }

            if (connectableDir.IsTrue(Direction.Right))
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
            Gizmos.DrawWireCube(transform.position + detectOffset, size);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + detectOffset);
        }
    }
}