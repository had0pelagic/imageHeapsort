using System;
using System.Collections.Generic;
using System.Text;

namespace HeapSort
{
    class Heap
    {
        public Node point;
        public Node root;
        public int count;

        public Heap(Node[] nodes)
        {
            count = 0;
            for (int i = 0; i < nodes.Length; i++)
                Add(nodes[i]);
        }

        public void HeapExternal(Node[] nodes)
        {
            count = 0;
            for (int i = 0; i < nodes.Length; i++)
                Add(nodes[i]);
        }

        public Heap()
        {
            count = 0;
        }

        public void Add(Node node)
        {
            if (root == null)
            {
                root = node;
                count++;
            }
            else
            {
                point = root;
                string toBinary = Convert.ToString(count + 1, 2);
                for (int i = 1; i < toBinary.Length; i++)
                {
                    if (toBinary[i] == '1')
                    {
                        if (point.right == null)
                            point.right = new Node(point);
                        point = point.right;
                    }
                    else
                    {
                        if (point.left == null)
                            point.left = new Node(point);
                        point = point.left;
                    }
                }
                point.data = node.data;

                while (true)
                {
                    if (point == root)
                        break;
                    if (point.data < point.parent.data)
                    {
                        int temp = point.data;
                        point.data = point.parent.data;
                        point.parent.data = temp;
                        point = point.parent;
                    }
                    else
                        break;
                }
                count++;
            }
        }

        public int[] toArray()
        {
            int[] array = new int[count];
            Node point = root;
            int arrayCount = 1;
            array[0] = root.data;

            for (int x = 1; x < count; x++)
            {
                point = root;
                string toBinary = Convert.ToString(arrayCount + 1, 2);
                for (int i = 1; i < toBinary.Length; i++)
                {
                    if (toBinary[i] == '1')
                    {
                        if (point.right == null)
                            point.right = new Node(point);
                        point = point.right;
                    }
                    else
                    {
                        if (point.left == null)
                            point.left = new Node(point);
                        point = point.left;
                    }
                }
                array[x] = point.data;
                arrayCount++;
            }
            return array;
        }

        private void Heapify()
        {
            Node toCompare;
            point = root;

            while (true)
            {
                if (point.left == null || point.left.used == true)
                    break;
                if (point.right == null || point.right.used == true)
                    toCompare = point.left;
                else
                {
                    if (point.left.data <= point.right.data)
                        toCompare = point.left;
                    else
                        toCompare = point.right;
                }
                if (point.data > toCompare.data)
                {
                    int temp = point.data;
                    point.data = toCompare.data;
                    toCompare.data = temp;
                    point = toCompare;
                    point.used = false;
                }
                else
                    break;
            }
        }

        public void Repointer(ref int newCount)
        {
            Node newpoint = root;
            point = root;
            string toBinary = Convert.ToString(newCount - 1, 2);
            point = root;

            for (int i = 1; i < toBinary.Length; i++)
            {
                if (toBinary[i] == '0')
                    newpoint = newpoint.left;
                else
                    newpoint = newpoint.right;
            }
            point = newpoint;
            newCount--;
        }

        public void HeapSort()
        {
            int newCount = count;
            for (int i = 0; i < count - 1; i++)
            {
                int temp = point.data;
                point.data = root.data;
                root.data = temp;
                point.used = true;
                Heapify();
                Repointer(ref newCount);
            }
        }

        public int[] HeapSortExternal(Node[] nodes)
        {
            Heap heap = new Heap();
            heap.HeapExternal(nodes);
            heap.HeapSort();
            return heap.toArray();
        }
    }
}
