using UnityEngine;

namespace Part
{
    [CreateAssetMenu(menuName = "PartInfo")]
    public class PartInfo : ScriptableObject
    {
        [SerializeField] private int cost;
        [SerializeField] private Vector2Int size;

        public int Cost => cost;
        public Vector2Int Size => size;
    }
}