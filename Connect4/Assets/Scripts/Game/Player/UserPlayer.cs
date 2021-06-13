using Connect4.Managers;


namespace Connect4.Game.Players
{
    public class UserPlayer : Player
    {
        public override void BeginTurn()
        {
            base.BeginTurn();

            RoundManager.Instance.MarkAsSelected(-1, _selectedColumn);
        }


        public void OnColumnSelect(int column)
        {
            SelectColumn(column);
        }

        public void OnColumnUnselect(int column)
        {
            UnselectColumn(column);
        }

    }
}
