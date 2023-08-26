using Builder.System;
using UnityEngine;

namespace Builder.Debugger
{
    public class GridDebugger : MonoBehaviour
    {
        [SerializeField] private PartBuilder partBuilder;
        [SerializeField] private float unitSize;
        [SerializeField] private GameObject grid;
        [SerializeField] private Color firstColor;
        [SerializeField] private Color secondColor;

        void Start()
        {
            Vector2Int gridSize = partBuilder.GridSize;
            Vector3 offset = new Vector3(gridSize.x, gridSize.y, transform.position.z) * unitSize * 0.5f;

            for (int row = 0; row < gridSize.x; row++)
            {
                for (int col = 0; col < gridSize.y; col++)
                {
                    GameObject obj = Instantiate(grid, transform);
                    obj.transform.localScale = new Vector3(unitSize, unitSize, unitSize);
                    obj.transform.localPosition = new Vector3(row * unitSize, col * unitSize, 0f) - offset;

                    SpriteRenderer rend = obj.GetComponent<SpriteRenderer>();
                    rend.color = (row + col) % 2 != 0 ? firstColor : secondColor;
                }
            }
        }
    }
}