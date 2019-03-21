using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Tetris
{
    public enum MoveDirection
    {
        Down,
        InstantlyDown,
        Right,
        Left,
        Rotate
    }

    public class Game
    {
        private Timer timer;
        private Board board;

        public Game()
        {
            Init();
        }

        private void Init()
        {
            Console.CursorVisible = false;
            board = new Board();
            Console.WindowWidth = 66;
            Console.WindowHeight = 32;
            Console.Write(board);
            timer = new Timer
            {
                Interval = 1000,
                Enabled = true
            };
            timer.Elapsed += TimerTick;
        }

        private void TimerTick(object sender, ElapsedEventArgs e)
        {
            if(!gameState)
                return;

            board.MoveTetromino(MoveDirection.Down);
            Console.Write(board);
        }

        private static bool gameState;

        public static void EndGame()
        {
            gameState = false;
        }

        public void Start()
        {
            gameState = true;
            ConsoleKey consoleKey = ConsoleKey.Backspace;
            while(gameState)
            {
                switch(consoleKey)
                {
                    case ConsoleKey.UpArrow:
                        board.MoveTetromino(MoveDirection.Rotate);
                        break;

                    case ConsoleKey.LeftArrow:
                        board.MoveTetromino(MoveDirection.Left);
                        break;

                    case ConsoleKey.DownArrow:
                        board.MoveTetromino(MoveDirection.Down);
                        break;

                    case ConsoleKey.RightArrow:
                        board.MoveTetromino(MoveDirection.Right);
                        break;

                    case ConsoleKey.Spacebar:
                        board.MoveTetromino(MoveDirection.InstantlyDown);
                        break;
                }

                Console.Write(board);
                consoleKey = Console.ReadKey(true).Key;
            }

            Console.SetCursorPosition(0, 24);
            Console.WriteLine("Игра завершена.");
        }
    }
}