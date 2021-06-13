using System.Collections.Generic;
using Connect4.Game.Players;
using Connect4.Game;
using UnityEngine;
using System.Linq;
using System;


namespace Connect4.Managers
{
    public class RoundManager : Manager<RoundManager>
    {
        [SerializeField] private RoundState _state;
        [SerializeField] private int _winCount = 4;
        [Header("Field")]
        [SerializeField] private GameObject _field;
        [SerializeField] private Vector2Int _fieldSize;
        [Header("Players")]
        [SerializeField] private Player _firstPlayer;
        [SerializeField] private Player _secondPlayer;

        private List<List<Cell>> _cells;
        private bool _finished;
        private int _turn;

        public IReadOnlyList<IReadOnlyList<Cell>> Cells
        {
            get => _cells;
        }


        protected override void Awake()
        {
            base.Awake();

            UpdateField();
        }

        private void Start()
        {
            BeginGame();
        }


        private void NextTurn()
        {
            if(_state.HasFlag(RoundState.FirstPlayerTurn))
            {
                _state &= ~RoundState.FirstPlayerTurn;
                _state |= RoundState.SecondPlayerTurn;

                _firstPlayer.EndTurn();
                _secondPlayer.BeginTurn();
            }
            else
            {
                _state &= ~RoundState.SecondPlayerTurn;
                _state |= RoundState.FirstPlayerTurn;

                _firstPlayer.BeginTurn();
                _secondPlayer.EndTurn();
            }
        }

        private void ResetField()
        {
            for(int i = 0; i < _cells.Count; i++)
            {
                for(int j = 0; j < _cells[i].Count; j++)
                {
                    _cells[i][j].State = CellType.None;
                    _cells[i][j].SetColor(ColorManager.Instance.EmptyColor);
                }
            }
        }

        private void UpdateField()
        {
            _cells = new List<List<Cell>>();

            // Use of id instead of Transform or RectTransform, because there can be the case
            // that items in LayoutGroup will just return position of the parent object
            // because their position wasn't calculated yet
            var columns = _field.GetComponentsInChildren<Cell>().
                GroupBy(cell => (cell.Id % _fieldSize.x)).
                Select(cellGroup => cellGroup.OrderBy(cell => (cell.Id / _fieldSize.y)));

            foreach(var column in columns)
            {
                _cells.Add(column.ToList());
            }
        }

        private Cell GetFreeCell(int column)
        {
            var currentColumn = _cells[column];
            int filledCount =  currentColumn.Count(cell =>
                (cell.State == CellType.FirstPlayer || cell.State == CellType.SecondPlayer)
            );

            return (filledCount == currentColumn.Count)? null : currentColumn[currentColumn.Count - filledCount - 1];
        }

        private List<Cell> GetWinCells(CellType cellType)
        {
            List<Cell> cells = null;
            if(((cells = CheckVertical(cellType)) == null) &&
                ((cells = CheckHorizontal(cellType)) == null) &&
                ((cells = CheckMainDiagonal(cellType)) == null) &&
                ((cells = CheckReverseDiagonal(cellType)) == null)
            )
            {
                return null;
            }
            return cells;
        }

        private List<Cell> CheckVertical(CellType sequenceType)
        {
            for(int i = 0; i < (_fieldSize.y - _winCount + 1); i++)
            {
                for(int j = 0; j < _fieldSize.x; j++)
                {
                    var selectedCells = new List<Cell>();
                    int winSequence = 0;

                    for(; winSequence < _winCount; winSequence++)
                    {
                        selectedCells.Add(_cells[j][i + winSequence]);
                        if(selectedCells[selectedCells.Count - 1].State != sequenceType)
                            break;
                    }
                    if(winSequence ==  _winCount)
                        return selectedCells;
                }
            }

            return null;
        }

        private List<Cell> CheckHorizontal(CellType sequenceType)
        {
            for(int i = 0; i < _fieldSize.y; i++)
            {
                for(int j = 0; j < (_fieldSize.x - _winCount + 1); j++)
                {
                    var selectedCells = new List<Cell>();
                    int winSequence = 0;

                    for(; winSequence < _winCount; winSequence++)
                    {
                        selectedCells.Add(_cells[j + winSequence][i]);
                        if(selectedCells[selectedCells.Count - 1].State != sequenceType)
                            break;
                    }
                    if(winSequence ==  _winCount)
                        return selectedCells;
                }
            }

            return null;
        }

        private List<Cell> CheckMainDiagonal(CellType sequenceType)
        {
            for(int i = (_winCount - 1); i < _fieldSize.y; i++)
            {
                for(int j = 0; j < (_fieldSize.x - _winCount + 1); j++)
                {
                    var selectedCells = new List<Cell>();
                    int winSequence = 0;

                    for(; winSequence < _winCount; winSequence++)
                    {
                        selectedCells.Add(_cells[j + winSequence][i - winSequence]);
                        if(selectedCells[selectedCells.Count - 1].State != sequenceType)
                            break;
                    }
                    if(winSequence ==  _winCount)
                        return selectedCells;
                }
            }

            return null;
        }

        private List<Cell> CheckReverseDiagonal(CellType sequenceType)
        {
            for(int i = 0; i < (_fieldSize.y - _winCount + 1); i++)
            {
                for(int j = 0; j < (_fieldSize.x - _winCount + 1); j++)
                {
                    var selectedCells = new List<Cell>();
                    int winSequence = 0;

                    for(; winSequence < _winCount; winSequence++)
                    {
                        selectedCells.Add(_cells[j + winSequence][i + winSequence]);
                        if(selectedCells[selectedCells.Count - 1].State != sequenceType)
                            break;
                    }
                    if(winSequence ==  _winCount)
                        return selectedCells;
                }
            }

            return null;
        }


        public void BeginGame()
        {
            ResetField();

            _state = RoundState.Playing | RoundState.FirstPlayerTurn;
            _firstPlayer.BeginTurn();
        }

        public void ResetGame()
        {
            ResetField();

            _state = RoundState.Playing | RoundState.FirstPlayerTurn;
            _firstPlayer.BeginTurn();
            _secondPlayer.EndTurn();

            _finished = false;
            _turn = 0;
        }


        public void OpenMenu()
        {
            _state &= ~RoundState.Playing;
            _state |= RoundState.Menu;
        }

        public void CloseMenu()
        {
            _state &= ~RoundState.Menu;
            _state |= RoundState.Playing;
        }


        public bool DoTurn(int column)
        {
            if(column < 0 || column >= _cells.Count)
                throw new ArgumentOutOfRangeException("Wrong column index");

            if(!_state.HasFlag(RoundState.Playing))
                return false;

            var selectedCell = GetFreeCell(column);
            if(selectedCell == null)
                return false;


            List<Cell> winCells;

            // Update cell state
            if(_state.HasFlag(RoundState.FirstPlayerTurn))
            {
                selectedCell.SetColor(ColorManager.Instance.FirstPlayerColor);
                selectedCell.State = CellType.FirstPlayer;

                winCells = GetWinCells(CellType.FirstPlayer);
            }
            else
            {
                selectedCell.SetColor(ColorManager.Instance.SecondPlayerColor);
                selectedCell.State = CellType.SecondPlayer;

                winCells = GetWinCells(CellType.SecondPlayer);
            }


            // If all cells are filled, end of the game
            // otherwise next turn
            if((winCells != null) ||
                (++_turn == (_fieldSize.x * _fieldSize.y)))
            {
                _finished = true;
            }
            else
            {
                NextTurn();
            }

            if(_finished)
            {
                if(_firstPlayer.Active)
                {
                    _state = RoundState.Win;
                    MenuManager.Instance.ShowWinMenu();
                }
                else
                {
                    _state = RoundState.Loose;
                    MenuManager.Instance.ShowLooseMenu();
                }

                if((winCells != null))
                {
                    foreach(var cell in winCells)
                    {
                        cell.SetColor(ColorManager.Instance.WinSequenceColor);
                    }
                }
            }

            return true;
        }

        public bool IsColumnSelectable(int column)
        {
            int filledCount =  _cells[column].Count(cell =>
                (cell.State == CellType.FirstPlayer || cell.State == CellType.SecondPlayer)
            );
            return !(filledCount == _cells[column].Count);
        }

        public void MarkAsSelected(int oldColumn, int newColumn)
        {
            if(oldColumn != -1)
            {
                var oldCell = GetFreeCell(oldColumn);
                oldCell?.SetColor(ColorManager.Instance.EmptyColor);
            }

            if(newColumn != -1)
            {
                var newCell = GetFreeCell(newColumn);
                newCell?.SetColor(ColorManager.Instance.PredictionColor);
            }
        }
    }
}
