using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        int numCases = int.Parse(Console.ReadLine());
        var tasks = new Task<string>[numCases];
        for (int i = 0; i < numCases; i++)
        {
            var input = Console.ReadLine();
            var t = Task.Run(() =>
            {
                return SolveSudoku(input);
            });
            tasks[i] = t;
        }
        Task.WaitAll(tasks, 23000);
        foreach (var task in tasks)
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                Console.WriteLine('Y');
                Console.WriteLine(task.Result);
            }
            else
            {
                Console.WriteLine('N');
            }
        }
    }
    public static string SolveSudoku(string input)
    {
        input = input.Replace('.', '0');
        var board = new int[81];
        var set = new byte[11];
        for (int i = 0; i < 81; i++)
        {
            board[i] = 1 << (input[i] - 48);
            if (board[i] != 1) set[i / 8] += (byte)(1 << i % 8);
        }
        int ptr = 0;
        bool back = false;
        do
        {
            if ((set[ptr / 8] & (1 << ptr % 8)) != 0)
            {
                if (!back) ptr++;
                else ptr--;
                continue;
            }
            back = false;
            if (board[ptr] < 1 << 9)
                board[ptr] = board[ptr] << 1;
            else
            {
                board[ptr] = 1;
                ptr--;
                back = true;
                continue;
            }
            if (Check(board))
                ptr++;
        } while (ptr < 81);

        char[] c = new char[81];
        for(int i = 0; i < 81; i++)
        {
            int count = 0;
            while (board[i] != 1)
            {
                board[i] = board[i] >> 1;
                count++;
            }
            c[i] = (char)(count + 48);
        }
        return new string(c);
    }

    private static bool Check(int[] board)
    {
        for (int j = 0; j < 9; j++)
        {
            int b = 0;
            for (int i = 0; i < 9; i++)
            {
                if (board[j * 9 + i] == 1) continue;
                int a = b ^ board[j * 9 + i];
                if (b > a) return false;
                b = a;
            }
        }
        for (int j = 0; j < 9; j++)
        {
            int b = 0;
            for (int i = 0; i < 9; i++)
            {
                if (board[i * 9 + j] == 1) continue;
                int a = b ^ board[i * 9 + j];
                if (b > a) return false;
                b = a;
            }
        }
        for(int i = 0; i < 9; i++)
        {
            int b = 0;
            for (int j = 0; j < 9; j++)
            {
                if (board[boxes[i, j]] == 1) continue;
                int a = b ^ board[boxes[i, j]];
                if (b > a) return false;
                b = a;
            }
        }
        
        return true;
    }


    private static int[,] boxes = new int[9, 9] { 
    { 0,  1, 2, 9,10,11,18,19,20},
    { 3 , 4, 5,12,13,14,21,22,23,},
    { 6 , 7, 8,15,16,17,24,25,26},
    { 27,28,29,36,37,38,45,46,47},
    { 30,31,32,39,40,41,48,49,50},
    { 33,34,35,42,43,44,51,52,53},
    { 54,55,56,63,64,65,72,73,74},
    { 57,58,59,66,67,68,75,76,77},
    { 60,61,62,69,70,71,78,79,80} };
}