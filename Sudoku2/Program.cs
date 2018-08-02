using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku2
{
    class Program
    {
        static void Main(string[] args)
        {
            var m = new Matrix(".94167358315489627678253491456312879983574216721698534562941783839726145147835962");
            m.Search(0);
            var list = new List<Fill>();
            foreach (var t in m.O) 
            {
                if (t == null) continue;
                var n = t;
                do
                {
                    n = n.down;
                    if (!list.Contains(n.fill) && n.fill != null) list.Add(n.fill);
                } while (!n.header);
            }
         //h   foreach (var f in m.set) list.Add(f);
            foreach(var f in list) Console.WriteLine((f.number + 1) + " at (" + f.row + "," + f.col + ")");
        }

        private class Matrix
        {
            public Node head;
            public List<Node> columns = new List<Node>();
            public Node[] O = new Node[324];
            public int filled = 0;
            public List<Fill> fills = new List<Fill>();
            public List<Fill> set = new List<Fill>();
            public Matrix(string s)
            {
                for(int i = 0; i < 324; i++)
                {
                    columns.Add(new Node() {
                        header = true
                    });
                }
                for(int i = 0; i < columns.Count; i++)
                {
                    var c = columns[i];
                    c.up = c;
                    c.down = c;
                    c.col = c;
                    c.right = columns[i == columns.Count - 1 ? 0 : i + 1];
                    c.left  = columns[i == 0 ? columns.Count - 1 : i - 1];
                }
                for(int i = 0; i < 9; i++) //the row
                {
                    for(int j = 0; j < 9; j++) //the column
                    {
                        for(int k = 0; k < 9; k++) //the number being placed
                        {
                            Node n1 = new Node();
                            n1.up = columns[j * 9 + i];
                            n1.col = columns[j * 9 + i];
                            n1.col.size++;
                            n1.down = n1.up.down;
                            n1.down.up = n1;
                            n1.up.down = n1;

                            Node n2 = new Node();
                            n2.up = columns[j * 9 + i + 81];
                            n2.col = columns[j * 9 + i + 81];
                            n2.col.size++;
                            n2.down = n2.up.down;
                            n2.down.up = n2;
                            n2.up.down = n2;

                            Node n3 = new Node();
                            n3.up = columns[j * 9 + i + 162];
                            n3.col = columns[j * 9 + i + 162];
                            n3.col.size++;
                            n3.down = n3.up.down;
                            n3.down.up = n3;
                            n3.up.down = n3;

                            Node n4 = new Node();
                            n4.up = columns[j * 9 + i + 243];
                            n4.col = columns[j * 9 + i + 243];
                            n4.col.size++;
                            n4.down = n4.up.down;
                            n4.down.up = n4;
                            n4.up.down = n4;

                            n1.right = n2;
                            n2.right = n3;
                            n3.right = n4;
                            n4.right = n1;

                            n4.left = n3;
                            n3.left = n2;
                            n2.left = n1;
                            n1.left = n4;
                            Fill fill = new Fill()
                            {
                                number = k,
                                row = i,
                                col = j,
                                box = boxes[i, j],
                                firstNode = n1
                            };
                            fills.Add(fill);
                            n1.fill = fill;
                            n2.fill = fill;
                            n3.fill = fill;
                            n4.fill = fill;
                        }
                    }
                }
                head = columns[0];
                for (int j = 0; j < 9; j++)
                {
                    for(int i = 0; i < 9; i++)
                    {
                        if (s[i + j * 9] == '.') continue;
                        int num = int.Parse(s.Substring(i + j * 9, 1)) - 1;
                        var fill = fills.Single(q =>
                            q.row == j &&
                            q.col == i &&
                            q.number == num
                        );
                        var t = fill.firstNode;
                        set.Add(fill);
                        do
                        {
                            var t2 = t;
                            do
                            {
                                Cover(t2.col);
                                t2 = t2.right;
                            } while (t2 != t);
                            t = t.right;
                        } while (t != fill.firstNode);
                    }
                }
            }
            public void Search(int k = 0)
            {
                // if R[h] = h, return
                if (head.right == head)
                    return;

                //otherwise, choose a column c
                //in this case using the S metric
                var c = head.right;
                int lowestNum = c.size;
                var lowestNode = c;
                while(c != head)
                {
                    if (c.size < lowestNum)
                    {
                        lowestNum = c.size;
                        lowestNode = c;
                    }
                    c = c.right;
                }
                c = lowestNode;

                //Cover column C
                Cover(c);

                //for each r <- D[c], D[D[c]],... while r != c
                var r = c.down;
                while (r != c) 
                {
                    //set O(sub k) = r
                    O[k] = r;

                    //for each j <- R[r], R[R[r]], ... while j != r
                    var j = r.right;
                    while(j != r)
                    {
                        j.right.left = j.left;
                        j.left.right = j.right;
                        j = j.right;
                    }
                    
                    //recurse, k + 1
                    Search(k + 1);

                    //set r <- O (sub k) and c <- C[r]
                    r = O[k];
                    c = r.col;

                    //for each j <- L[r], L[L[r]], ... while j != r
                   // j = c.up;
                    var start = j.left;
                    while(j != start)
                    {
                        //uncover column j
                        Uncover(j);
                        j = j.left;
                    }
                    r = r.down;
                } 
                //uncover column C
                Uncover(c);
            }

            private void Uncover(Node c)
            {
                c.right.left = c;
                c.left.right = c;
                columns.Add(c);

                var start = c.col;
                var i = start.up;
                while (i != start) 
                {
                    var j = i.left;
                    while (j != i) 
                    {
                        c.col.size++;
                        j.down.up = j;
                        j.up.down = j;
                        j = j.left;
                    } 
                    i = i.up;
                } 
                if(c.right == head)
                {
                    head = c;
                }
            }

            private void Cover(Node c)
            {
                c.right.left = c.left;
                c.left.right = c.right;
                columns.Remove(c);

                var i = c.col.down;
                while(i != c.col)
                {
                    var j = i.right;
                    while(j != i)
                    {
                        j.up.down = j.down;
                        j.down.up = j.up;
                        j.col.size--;
                        j = j.right;
                    }
                    i = i.down;
                }
                if(c == head)
                {
                    head = c.right;
                }
            }
        }
        private class Node
        {
            public Node up;
            public Node down;
            public Node right;
            public Node left;
            public Node col;
            public int size;
            public bool header = false;
            public Fill fill;
            public int count;
            public static int countAll;
            public Node()
            {
                up = this;
                down = this;
                right = this;
                left = this;
                count = ++countAll;
            }
        }
        private class Fill
        {
            public int row;
            public int col;
            public int box;
            public int number;
            public Node firstNode;
        }
       
        private static int[,] boxes = new int[9, 9] {
    { 0,  0, 0, 1,1,1, 2,2,2},
    { 0 , 0, 0, 1,1,1, 2,2,2,},
    { 0 , 0, 0, 1,1,1, 2,2,2},
    { 3,3,3,4,4,4,5,5,5},
    { 3,3,3,4,4,4,5,5,5},
    { 3,3,3,4,4,4,5,5,5},
    { 6,6,6,7,7,7,8,8,8},
    { 6,6,6,7,7,7,8,8,8},
    { 6,6,6,7,7,7,8,8,8} };
    }
}
