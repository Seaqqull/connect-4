using UnityEngine;


namespace Connect4.Managers
{
    public class ColorManager : Manager<ColorManager>
    {
        [SerializeField] private Color _emptyColor;
        [SerializeField] private Color _predictionColor;
        [SerializeField] private Color _firstPlayerColor;
        [SerializeField] private Color _secondPlayerColor;

        public Color SecondPlayerColor
        {
            get => _secondPlayerColor;
        }
        public Color FirstPlayerColor
        {
            get => _firstPlayerColor;
        }
        public Color PredictionColor
        {
            get => _predictionColor;
        }
        public Color EmptyColor
        {
            get => _emptyColor;
        }

    }
}
