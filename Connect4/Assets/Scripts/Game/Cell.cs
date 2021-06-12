using UnityEngine.UI;
using UnityEngine;


namespace Connect4.Game
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private int _id;
        [SerializeField] private CellType _state;

        public CellType State
        {
            get => _state;
            set => _state = value;
        }
        public Image Image {get; private set;}
        public int Id {get => _id;}


        private void Awake()
        {
            Image = GetComponent<Image>();
        }
    }
}
