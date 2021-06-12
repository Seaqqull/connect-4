using UnityEngine.UI;
using UnityEngine;


namespace Connect4.Utilities
{
    public class FlexGridLayout : LayoutGroup
    {
        [SerializeField] private bool _autoSize;
        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private Vector2 _cellMargin;
        [Header("Read-only")]
        [SerializeField] private Vector2 _cellSize;


        public override void SetLayoutVertical() { }

        public override void SetLayoutHorizontal() { }

        public override void CalculateLayoutInputVertical()
        {
            if(_autoSize)
            {
                float squaredSize = Mathf.Sqrt(transform.childCount);
                _gridSize.x = Mathf.CeilToInt(squaredSize);
                _gridSize.y = Mathf.CeilToInt(squaredSize);
            }

            float parentHeight = rectTransform.rect.height;
            float parentWidth = rectTransform.rect.width;

            _cellSize.y = (parentHeight / (float)_gridSize.y) -
                ((_cellMargin.y / (float)_gridSize.y) * (_gridSize.y - 1)) - // Margin
                (padding.top / (float) _gridSize.y) - (padding.bottom / (float)_gridSize.y); // Padding
            _cellSize.x = (parentWidth / (float)_gridSize.x) -
                ((_cellMargin.x / (float)_gridSize.x) * (_gridSize.x - 1)) - //Margin
                (padding.left / (float) _gridSize.x) - (padding.right / (float)_gridSize.x); // Padding


            int columnCount = 0;
            int rowCount = 0;

            for(int i = 0; i < rectChildren.Count; i++)
            {
                columnCount = i % _gridSize.x;
                rowCount = i / _gridSize.x;

                var item = rectChildren[i];

                float xPos = (_cellSize.x * columnCount) +
                    (_cellMargin.x * columnCount) + // Margin
                    padding.left; // Padding
                float yPos = (_cellSize.y * rowCount) +
                    (_cellMargin.y * rowCount) + // Margin
                    padding.top; // Padding

                SetChildAlongAxis(item, 0, xPos, _cellSize.x);
                SetChildAlongAxis(item, 1, yPos, _cellSize.y);
            }
        }

    }
}
