using Connect4.Menus;
using UnityEngine;


namespace Connect4.Managers
{
    public class MenuManager : Manager<MenuManager>
    {
        [SerializeField] private EndMenu _endMenu;


        public void ShowWinMenu()
        {
            _endMenu.ShowWinMenu();
            _endMenu.gameObject.SetActive(true);
        }

        public void ShowLooseMenu()
        {
            _endMenu.ShowLooseMenu();
            _endMenu.gameObject.SetActive(true);
        }


        public void HideEndMenu()
        {
            _endMenu.gameObject.SetActive(false);
        }
    }
}
