using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public enum CellKind
    {
        Block,
        Border,
        Tetromino,
        FreeSpace
    }

    public class Cell
    {
        public bool IsBlockOrBorder { get { return CellKind == CellKind.Block || CellKind == CellKind.Border; } }

        public CellKind CellKind { get; private set; }
        public override string ToString()
        {
            switch(CellKind)
            {
                case CellKind.Block:
                case CellKind.Tetromino:
                    return "■ ";
                case CellKind.Border:
                    return "* ";
                case CellKind.FreeSpace:
                    return "  ";
                default:
                    return "  ";
            }
        }

        public void TransformToTetromino()
        {
            CellKind = CellKind.Tetromino;
        }

        public void TransformToFreeSpace()
        {
            CellKind = CellKind.FreeSpace;
        }

        public void TransformToBlock()
        {
            CellKind = CellKind.Block;
        }

        public Cell(CellKind cellKind)
        {
            CellKind = cellKind;
        }
    }
}