using Connect4.Managers;
using UnityEngine;
using System;


namespace Connect4.Game.Players
{
    public abstract class Player : MonoBehaviour
    {
        protected int _selectedColumn = -1;

        public bool Active {get; private set;}


        public void DoTurn()
        {
            if(_selectedColumn == -1)
                throw new Exception("Column not selected");
            if(!Active)
                return;

            RoundManager.Instance.DoTurn(_selectedColumn);
        }

        public virtual void EndTurn()
        {
            Active = false;
        }

        public virtual void BeginTurn()
        {
            Active = true;
        }

        public bool SelectColumn(int column)
        {
            if(column < 0 || column >= RoundManager.Instance.Cells.Count)
                throw new ArgumentOutOfRangeException("Wrong column index");

            var columnSelectable = RoundManager.Instance.IsColumnSelectable(column);
            RoundManager.Instance.MarkAsRegular(_selectedColumn);
            if(columnSelectable)
                RoundManager.Instance.MarkAsSelected(column);

            _selectedColumn = column;
            return columnSelectable;
        }

        public bool UnselectColumn(int column)
        {
            if(_selectedColumn != column)
                return false;

            RoundManager.Instance.MarkAsRegular(column);
            _selectedColumn = -1;
            return true;
        }
    }
}
