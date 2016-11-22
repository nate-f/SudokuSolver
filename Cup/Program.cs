using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        char[][] array = new char[4][];
        for(int i = 0; i < array.Count(); i++)
        {
            array[i] = new char[4]
            {
                'W', 'G', 'G', 'O'
            };
        }
        Museum(array);

    }
    public static char[][] Museum(char[][] floor)
    {
        for(int i = 0; i < floor.Length; i++)
        {
            for(int j = 0; j < floor[i].Length; j++)
            {
                if (floor[i][j] != 'O') continue;
                var currentLocation = new Point(i, j);
                var nearestGuard = BFS(floor, currentLocation);
                var dist = Point.Distance(currentLocation, nearestGuard);
                floor[i][j] = (char)(dist + 48);
            }
        }
        return floor;
    }
    public static Point BFS(char[][] arr, Point p)
    {
        var locations = new Queue<Point>();
        locations.Enqueue(p);

        while (locations.Count != 0)
        {
            var current = locations.Dequeue();
            if (current.x < 0 || current.y < 0 || current.x > arr.Count() - 1|| current.y > arr[0].Count() - 1) continue;
            if (arr[current.x][current.y] == 'G') return current;
            else if (arr[current.x][current.y] == 'O')
            {
                locations.Enqueue(new Point(p.x + 1, p.y));
                locations.Enqueue(new Point(p.x - 1, p.y));
                locations.Enqueue(new Point(p.x, p.y + 1));
                locations.Enqueue(new Point(p.x, p.y - 1));
            }
        }
        return new Point(-1, -1);
    }
    public struct Point
    {
        public int x;
        public int y;
        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public static int Distance(Point a, Point b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }
    }
}