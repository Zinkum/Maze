using System;
using System.Collections.Generic;
using System.Text;

namespace Maze
{
    public enum VisualizationOption
    {
        Solid,
        BoxChars
    };

    public class Maze
    {
        private readonly short height, width;
        private readonly bool[,] horizontal, vertical;

        private Maze(short height, short width)
        {
            this.height = height;
            this.width = width;
            horizontal = new bool[height - 1, width];
            vertical = new bool[height, width - 1];
        }

        public static Maze Generate(short height, short width, int? seed = null)
        {
            Maze maze = new Maze(height, width);
            Random random = seed.HasValue ? new Random(seed.Value) : new Random();
            bool[,] visited = new bool[height, width];
            Point current;
            Stack<Point> stack = new Stack<Point>(2 * Math.Max(height, width));
            stack.Push(new Point(0, 0));
            StringBuilder directions = new StringBuilder();

            while (stack.Count > 0)
            {
                current = stack.Peek();
                directions.Clear();
                visited[current.x, current.y] = true;
                if (current.x - 1 >= 0 && !visited[current.x - 1, current.y])
                    directions.Append('U');
                if (current.y - 1 >= 0 && !visited[current.x, current.y - 1])
                    directions.Append('L');
                if (current.x + 1 < height && !visited[current.x + 1, current.y])
                    directions.Append('D');
                if (current.y + 1 < width && !visited[current.x, current.y + 1])
                    directions.Append('R');
                if (directions.Length == 0)
                    stack.Pop();
                else
                {
                    switch (directions[random.Next(directions.Length)])
                    {
                        case 'U':
                            stack.Push(new Point((short)(current.x - 1), current.y));
                            maze.horizontal[current.x - 1, current.y] = true;
                            break;
                        case 'L':
                            stack.Push(new Point(current.x, (short)(current.y - 1)));
                            maze.vertical[current.x, current.y - 1] = true;
                            break;
                        case 'D':
                            stack.Push(new Point((short)(current.x + 1), current.y));
                            maze.horizontal[current.x, current.y] = true;
                            break;
                        case 'R':
                            stack.Push(new Point(current.x, (short)(current.y + 1)));
                            maze.vertical[current.x, current.y] = true;
                            break;
                    }
                }
            }

            return maze;
        }

        public string ToString(VisualizationOption option = VisualizationOption.Solid) => option switch
        {
            VisualizationOption.Solid => AsSolid(),
            VisualizationOption.BoxChars => AsBox(),
            _ => throw new ArgumentException(nameof(option))
        };

        private string AsSolid()
        {
            StringBuilder maze = new StringBuilder(height * (width + 1) * 3 + 1), verline = new StringBuilder((width + 1) * 3), horline = new StringBuilder((width + 1) * 3);
            maze.Append("█  ");
            for (int j = 1; j < width; j++)
                maze.Append("███");
            maze.Append("█\r\n");
            for (int i = 0; i < height; i++)
            {
                verline.Clear();
                horline.Clear();
                for (int j = 0; j < width; j++)
                {
                    if (j < width - 1)
                        if (vertical[i, j])
                            verline.Append("   ");
                        else
                            verline.Append("█  ");
                    if (i < height - 1)
                        if (horizontal[i, j])
                            horline.Append("█  ");
                        else
                            horline.Append("███");
                }
                if (horline.Length > 0)
                    horline.Append("█\r\n");
                maze.Append("█  ").Append(verline).Append("█\r\n█  ").Append(verline).Append("█\r\n").Append(horline);
            }
            for (int j = 0; j < width - 1; j++)
                maze.Append("███");
            maze.Append("█  █");
            return maze.ToString();
        }

        private string AsBox()
        {
            const char up = '╙', upleft = '╝', left = '╕', downleft = '╗',
                down = '╓', downright = '╔', right = '╒', upright = '╚',
                hor = '═', ver = '║', cross = '╬',
                upt = '╩', leftt = '╣', downt = '╦', rightt = '╠';
            StringBuilder maze = new StringBuilder((width + 2) * height), tile = new StringBuilder(4);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    tile.Clear();
                    if (i - 1 >= 0 && horizontal[i - 1, j])
                        tile.Append('U');
                    if (j - 1 >= 0 && vertical[i, j - 1])
                        tile.Append('L');
                    if (i + 1 < height && horizontal[i, j])
                        tile.Append('D');
                    if (j + 1 < width && vertical[i, j])
                        tile.Append('R');
                    switch (tile.ToString())
                    {
                        case "U":
                            maze.Append(up);
                            break;
                        case "L":
                            maze.Append(left);
                            break;
                        case "D":
                            maze.Append(down);
                            break;
                        case "R":
                            maze.Append(right);
                            break;
                        case "UL":
                            maze.Append(upleft);
                            break;
                        case "UD":
                            maze.Append(ver);
                            break;
                        case "UR":
                            maze.Append(upright);
                            break;
                        case "LD":
                            maze.Append(downleft);
                            break;
                        case "LR":
                            maze.Append(hor);
                            break;
                        case "DR":
                            maze.Append(downright);
                            break;
                        case "ULD":
                            maze.Append(leftt);
                            break;
                        case "ULR":
                            maze.Append(upt);
                            break;
                        case "UDR":
                            maze.Append(rightt);
                            break;
                        case "LDR":
                            maze.Append(downt);
                            break;
                        case "ULDR":
                            maze.Append(cross);
                            break;
                        default:
                            maze.Append('#');
                            break;
                    }
                }
                maze.Append("\r\n");
            }
            return maze.ToString();
        }

        private class Point
        {
            public short x, y;
            public Point(short X, short Y) { x = X; y = Y; }
        }
    }
}
