using UnityEngine.UI;
using UnityEngine;


namespace Connect4.Game
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private int _id;
        [SerializeField] private CellType _state;

        private Image _image;

        public CellType State
        {
            get => _state;
            set => _state = value;
        }
        public int Id {get => _id;}


        private void Awake()
        {
            _image = GetComponent<Image>();
        }


        public void SetColor(Color color)
        {
            _image.color = color;
        }
    }
}
