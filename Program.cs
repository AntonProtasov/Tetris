﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Tetris";
            Game game = new Game();
            game.Start();
            Console.ReadLine();
        }
    }
}