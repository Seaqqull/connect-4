using System;


namespace Connect4.Game
{
    public enum CellType : byte { None, FirstPlayer, SecondPlayer, Prediction }
    [Flags]
    public enum RoundState : byte { None, Menu, Playing, FirstPlayerTurn = 4, SecondPlayerTurn = 8, TurnAction = 16, Win = 32, Loose = 64}
}