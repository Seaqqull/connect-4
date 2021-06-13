using Connect4.Managers;
using System.Linq;
using UnityEngine;


namespace Connect4.Game.Players
{
    public class AIPlayer : Player
    {
        private void FindFreeColumn()
        {
            var freeColumns = RoundManager.Instance.Cells.
                Select((column, i) => RoundManager.Instance.IsColumnSelectable(i)? i : -1).
                Where(i => i != -1);
            _selectedColumn = freeColumns.ElementAt(Random.Range(0, freeColumns.Count()));

            DoTurn();
        }


        public override void BeginTurn()
        {
            base.BeginTurn();

            FindFreeColumn();
        }

        public override void EndTurn()
        {
            base.EndTurn();

            _selectedColumn = -1;
        }
    }
}
