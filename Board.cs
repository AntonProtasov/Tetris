using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class Board
    {
        private readonly Cell[][] cells;

        private Tetromino currentTetromino;
        private readonly TetrominoManager manager;

        private readonly int boardHeight;
        private readonly int boardWidth;

        private int lines;
        private long points;

        public Board()
        {
            boardHeight = 24;
            boardWidth = 10;

            cells = new Cell[boardHeight][];
            manager = new TetrominoManager();

            currentTetromino = manager.GetRandomTetromino();

            InitFreeSpaces();
            InitBorders();

            AddTetrominoToBoard();

            lines = 0;
            points = 0;
        }

        private void InitFreeSpaces()
        {
            for(int i = 0; i < boardHeight; i++)
            {
                cells[i] = new Cell[boardWidth];
                for(int j = 1; j < boardWidth - 1; j++)
                    cells[i][j] = new Cell(CellKind.FreeSpace);
            }
        }

        private void InitBorders()
        {
            for(int i = 0; i < boardHeight; i++)
            {
                // Левая боковая граница.
                cells[i][0] = new Cell(CellKind.Border);

                // Правая боковая граница.
                cells[i][boardWidth - 1] = new Cell(CellKind.Border);
            }

            // Нижняя граница.
            for(int i = 0; i < boardWidth; i++)
                cells[boardHeight - 1][i] = new Cell(CellKind.Border);
        }

        public override string ToString()
        {
            Show();
            return String.Empty;
        }

        public void Show()
        {
            for(int i = 0; i < boardHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                for(int j = 0; j < boardWidth; j++)
                    Console.Write(cells[i][j]);
            }

            ShowPoints();
        }

        private void ShowPoints()
        {
            int delta = 2;
            Console.SetCursorPosition(2 * boardWidth + delta, boardHeight / 2);
            Console.WriteLine($"Lines: {lines}");
            Console.SetCursorPosition(2 * boardWidth + delta, boardHeight / 2 + delta);
            Console.WriteLine($"Points: {points}");
        }

        private void AddTetrominoToBoard()
        {
            foreach(Unit unit in currentTetromino.Units)
                cells[unit.Row][unit.Column].TransformToTetromino();
        }

        public void MoveTetromino(MoveDirection direction)
        {
            HideTetromino();
            switch(direction)
            {
                case MoveDirection.Down:
                    if(!CanMoveDown(1))
                        PlaceTetromino(instantly: false);
                    else
                    {
                        for(int i = 0; i < currentTetromino.Units.Length; i++)
                            currentTetromino.Units[i].Row += 1;
                    }
                    break;

                case MoveDirection.InstantlyDown:
                    PlaceTetromino(instantly: true);
                    break;

                case MoveDirection.Right:
                case MoveDirection.Left:
                    bool rigth = direction == MoveDirection.Right;
                    int offset = rigth ? 1 : -1;

                    if(ClashWithBlocksOrBorders(currentTetromino, offset))
                        break;

                    for(int i = 0; i < currentTetromino.Units.Length; i++)
                        currentTetromino.Units[i].Column = currentTetromino.Units[i].Column + offset;
                    break;

                case MoveDirection.Rotate:
                    if(currentTetromino.Kind == TetrominoKind.O)
                        break;

                    Tetromino rotated = manager.Rotate(currentTetromino);

                    if(!ClashWithBlocksOrBorders(rotated))
                        currentTetromino = rotated;
                    break;
            }
            AddTetrominoToBoard();
        }

        private bool HasFreeSpace(Cell[] cellRow)
        {
            foreach(Cell cell in cellRow)
            {
                if(cell.CellKind == CellKind.FreeSpace)
                    return true;
            }
            return false;
        }

        private void RemoveBlocks()
        {
            int combo = 0;
            for(int i = 0; i < boardHeight - 1; i++)
            {
                if(HasFreeSpace(cells[i]))
                    continue;

                lines++;
                combo++;

                for(int j = 1; j < boardWidth - 1; j++)
                    cells[i][j] = new Cell(CellKind.FreeSpace);

                for(int j = i - 1; j > 0; j--)
                {
                    for(int k = 1; k < boardWidth - 1; k++)
                    {
                        CellKind kind = cells[j][k].CellKind;
                        if(kind == CellKind.FreeSpace)
                            continue;

                        Cell cell = cells[j][k];
                        cells[j][k] = new Cell(CellKind.FreeSpace);
                        cells[j + 1][k] = cell;
                    }
                }
            }

            points += GetPoints(combo);
        }

        private const int LinePoints = 80;

        private int GetPoints(int combo)
        {
            return combo * combo * LinePoints;
        }

        private void HideTetromino()
        {
            foreach(Unit unit in currentTetromino.Units)
                cells[unit.Row][unit.Column].TransformToFreeSpace();
        }

        private bool ClashWithBlocksOrBorders(Tetromino tetromino, int offset = 0)
        {
            foreach(Unit unit in tetromino.Units)
            {
                if(unit.Column + offset < 0)
                    return true;

                if(unit.Column + offset > 23)
                    return true;

                if(cells[unit.Row][unit.Column + offset].IsBlockOrBorder)
                    return true;
            }
            return false;
        }

        private void PlaceTetromino(bool instantly)
        {
            if(instantly)
                MoveDownInstantly();

            foreach(Unit unit in currentTetromino.Units)
                cells[unit.Row][unit.Column].TransformToBlock();

            if(EndGame())
            {
                Game.EndGame();
                return;
            }

            RemoveBlocks();
            currentTetromino = manager.GetRandomTetromino();
        }

        public void MoveDownInstantly()
        {
            int step = 0;
            while(true)
            {
                if(!CanMoveDown(step + 1))
                {
                    for(int i = 0; i < currentTetromino.Units.Length; i++)
                        currentTetromino.Units[i].Row = currentTetromino.Units[i].Row + step;

                    break;
                }
                step++;
            }
        }

        private bool CanMoveDown(int step)
        {
            foreach(Unit unit in currentTetromino.Units)
            {
                if(cells[unit.Row + step][unit.Column].IsBlockOrBorder)
                    return false;
            }
            return true;
        }

        private bool EndGame()
        {
            foreach(Cell cell in cells[4])
            {
                if(cell.CellKind == CellKind.Block)
                    return true;
            }
            return false;
        }
    }
}