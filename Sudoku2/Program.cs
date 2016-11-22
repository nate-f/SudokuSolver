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
            var m = new Matrix("52...6.........7.13...........4..8..6......5...........418.........3..2...87.....");
        }

        private class Matrix
        {
            public List<Column> cols = new List<Column>();
            public List<Row> rows = new List<Row>();

            public List<Node> solution = new List<Node>();
            public Row[] removed = new Row[324];
            public Matrix(string s)
            {
                for(int i = 0; i < 324; i++)
                {
                    cols.Add(new Column());
                }
                for(int i = 0; i < 9; i++) //the row
                {
                    for(int j = 0; j < 9; j++) //the column
                    {
                        for(int k = 0; k < 9; k++) //the number being placed
                        {
                            Node n1 = new Node();
                            n1.up = cols[j * 9 + i];
                            n1.col = cols[j * 9 + i];
                            n1.down = n1.up.down;
                            n1.down.up = n1;
                            n1.up.down = n1;

                            Node n2 = new Node();
                            n2.up = cols[j * 9 + i + 81];
                            n2.col = cols[j * 9 + i + 81];
                            n2.down = n2.up.down;
                            n2.down.up = n2;
                            n2.up.down = n2;

                            Node n3 = new Node();
                            n3.up = cols[j * 9 + i + 162];
                            n3.col = cols[j * 9 + i + 162];
                            n3.down = n3.up.down;
                            n3.down.up = n3;
                            n3.up.down = n3;

                            Node n4 = new Node();
                            n4.up = cols[j * 9 + i + 243];
                            n4.col = cols[j * 9 + i + 243];
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

                            var fill = new Fill(i, j, Fill.GetBox(i, j), k);
                            n1.fill = fill;
                            n2.fill = fill;
                            n3.fill = fill;
                            n4.fill = fill;

                            var r = new Row();
                            r.nodes = new List<Node>() { n1, n2, n3, n4 };
                            r.fill = fill;
                            rows.Add(r);
                        }
                    }
                }
                for(int j = 0; j < 9; j++)
                {
                    for(int i = 0; i < 9; i++)
                    {
                        if (s[i + j * 9] == '.') continue;
                        int num = int.Parse(s.Substring(i + j * 9, 1));
                        var f = new Fill(i, j, Fill.GetBox(i, j), num);
                        var r = rows.Single(q => q.fill == f);
                        cols.RemoveAll(q => r.nodes.Any(w => w.col == q));
                        solution.Add(r);
                    }
                }
            }
            public void Solve(int l = 0)
            {
                var c = cols.FirstOrDefault(q => !solution.Any(w => w.nodes.Any(e => e.col == q)));
                if (c == null)
                {
                    //done
                }
                var r = rows.FirstOrDefault(q => q.nodes.Any(w => w.col == c));
                if (r == null)
                {
                    //backtrack
                }
                else
                {
                    removed[l] = 
                    foreach(var box in c.nodes)
                    {
                        box.right.left = box.left;
                        box.left.right = box.right;
                    }
                    c.right.left = c.left;
                    c.left.right = c.right;
                }
                return null;
            }
        }
        private class Column : Node
        {
            public IEnumerable<Node> nodes
            {
                get
                {
                    var cur = down;
                    yield return cur;
                    var self = this;
                    while(cur != self)
                    {
                        yield return cur;
                        cur = cur.down;
                    }
                }
            }
        }
        private class Node
        {
            public Node up;
            public Node down;
            public Node right;
            public Node left;
            public Column col;
            public Fill fill;
            public Node()
            {
                up = this;
                down = this;
                right = this;
                left = this;
            }
        }
        private class Fill
        {
            public int x;
            public int y;
            public int z;
            public int num;
            public Fill(int x, int y, int z, int num)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.num = num;
            }
            public static int GetBox(int x, int y)
            {
                return boxes[x, y];
            }
            public override bool Equals(object other)
            {
                var f = other as Fill;
                if (f == null) return false;
                if (x != f.x || y != f.y || z != f.z || num != f.num) return false;
                return true;
            }
            public static bool operator ==(Fill f1, Fill f2)
            {
                if (ReferenceEquals(f1, f2)) return true;
                return f1.Equals(f2);
            }
            public static bool operator !=(Fill f1, Fill f2)
            {
                return !f1.Equals(f2);
            }
        }
        private class Row
        {
            public Fill fill;
            public List<Node> nodes;
            public Row()
            {
                    
            }
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
