using System;
using System.Collections.Generic;
using System.Text;

namespace HeapSort
{
    class Node
    {
        public int data;
        public Node left;
        public Node right;
        public Node parent;
        public bool used;

        public Node(Node par)
        {
            parent = par;
        }

        public Node(int val, bool us)
        {
            data = val;
            used = us;
        }
    }
}
