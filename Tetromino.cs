using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public enum TetrominoKind { I, O, T, J, L, S, Z }

    public struct Unit
    {
        public Unit(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public int Row { get; set; }
        public int Column { get; set; }
    }

    public class Tetromino
    {
        public Unit[] Units = new Unit[4];
        public TetrominoKind Kind { get; set; }

        public Tetromino GetCopy()
        {
            Tetromino copy = new Tetromino();
            copy.Kind = this.Kind;
            for(int i = 0; i < Units.Length; i++)
                copy.Units[i] = this.Units[i];

            return copy;
        }
    }
}