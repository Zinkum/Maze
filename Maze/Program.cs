using System;

namespace Maze
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write(Maze.Generate(24,32).ToString());
        }
    }
}
