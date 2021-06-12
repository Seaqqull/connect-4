using UnityEngine;


namespace Connect4.Menus
{
    public class EndMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _winMenu;
        [SerializeField] private GameObject _looseMenu;


        public void ShowWinMenu()
        {
            _looseMenu.SetActive(false);
            _winMenu.SetActive(true);
        }

        public void ShowLooseMenu()
        {
            _looseMenu.SetActive(true);
            _winMenu.SetActive(false);
        }
    }
}
