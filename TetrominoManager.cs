using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class TetrominoManager
    {
        private readonly System.Random random;

        public TetrominoManager()
        {
            random = new Random();
            InitDefaultTetrominos();
        }

        private readonly Tetromino[] defaultTetrominos = new Tetromino[7];

        private void InitDefaultTetrominos()
        {
            Tetromino tetrominoI = new Tetromino();
            tetrominoI.Kind = TetrominoKind.I;
            tetrominoI.Units[0] = new Unit(0, 4);
            tetrominoI.Units[1] = new Unit(1, 4);
            tetrominoI.Units[2] = new Unit(2, 4);
            tetrominoI.Units[3] = new Unit(3, 4);

            Tetromino tetrominoO = new Tetromino();
            tetrominoO.Kind = TetrominoKind.O;
            tetrominoO.Units[0] = new Unit(0, 4);
            tetrominoO.Units[1] = new Unit(0, 5);
            tetrominoO.Units[2] = new Unit(1, 4);
            tetrominoO.Units[3] = new Unit(1, 5);

            Tetromino tetrominoT = new Tetromino();
            tetrominoT.Kind = TetrominoKind.T;
            tetrominoT.Units[0] = new Unit(0, 4);
            tetrominoT.Units[1] = new Unit(1, 3);
            tetrominoT.Units[2] = new Unit(1, 4);
            tetrominoT.Units[3] = new Unit(1, 5);

            Tetromino tetrominoZ = new Tetromino();
            tetrominoZ.Kind = TetrominoKind.Z;
            tetrominoZ.Units[0] = new Unit(0, 5);
            tetrominoZ.Units[1] = new Unit(1, 5);
            tetrominoZ.Units[2] = new Unit(1, 4);
            tetrominoZ.Units[3] = new Unit(2, 4);

            Tetromino tetrominoS = new Tetromino();
            tetrominoS.Kind = TetrominoKind.S;
            tetrominoS.Units[0] = new Unit(0, 4);
            tetrominoS.Units[1] = new Unit(1, 4);
            tetrominoS.Units[2] = new Unit(1, 5);
            tetrominoS.Units[3] = new Unit(2, 5);

            Tetromino tetrominoJ = new Tetromino();
            tetrominoJ.Kind = TetrominoKind.J;
            tetrominoJ.Units[0] = new Unit(0, 5);
            tetrominoJ.Units[1] = new Unit(1, 5);
            tetrominoJ.Units[2] = new Unit(2, 5);
            tetrominoJ.Units[3] = new Unit(2, 4);

            Tetromino tetrominoL = new Tetromino();
            tetrominoL.Kind = TetrominoKind.L;
            tetrominoL.Units[0] = new Unit(0, 4);
            tetrominoL.Units[1] = new Unit(1, 4);
            tetrominoL.Units[2] = new Unit(2, 4);
            tetrominoL.Units[3] = new Unit(2, 5);

            defaultTetrominos[0] = tetrominoI;
            defaultTetrominos[1] = tetrominoO;
            defaultTetrominos[2] = tetrominoT;
            defaultTetrominos[3] = tetrominoZ;
            defaultTetrominos[4] = tetrominoS;
            defaultTetrominos[5] = tetrominoJ;
            defaultTetrominos[6] = tetrominoL;
        }

        public Tetromino GetRandomTetromino()
        {
            int index = random.Next(7);
            Tetromino nextTetromino = defaultTetrominos[index];
            return nextTetromino.GetCopy();
        }

        public Tetromino Rotate(Tetromino tetromino)
        {
            return RotateByMatrix(tetromino);
        }

        private Tetromino RotateByMatrix(Tetromino tetromino)
        {
            Tetromino rotated = new Tetromino();

            for(int i = 0; i < rotated.Units.Length; i++)
                rotated.Units[i] = Rotate(tetromino.Units[i], tetromino.Units[2]);

            rotated.Kind = tetromino.Kind;
            return rotated;
        }

        private Unit Rotate(Unit unit, Unit centralUnit)
        {
            int[][] rMatrix = { new[] { 0, 1 }, new[] { -1, 0 } };

            int[] diff = { unit.Row - centralUnit.Row, unit.Column - centralUnit.Column };

            int[] multiplication = { rMatrix[0][0] * diff[0] + rMatrix[0][1] * diff[1], rMatrix[1][0] * diff[0] + rMatrix[1][1] * diff[1] };

            int[] sum = { centralUnit.Row + multiplication[0], centralUnit.Column + multiplication[1] };

            return new Unit { Row = sum[0], Column = sum[1] };
        }
    }
}