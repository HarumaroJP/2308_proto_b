using System;
using Builder.System;
using UnityEngine;

namespace Part
{
    [Serializable]
    public class ConnectableDirection
    {
        [SerializeField] private Direction direction;

        public bool IsConnectable(Direction originDir)
        {
            return IsTrue(Reverse(originDir));
        }

        public bool IsTrue(Direction direction)
        {
            return (this.direction & direction) == direction;
        }

        Direction Reverse(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}