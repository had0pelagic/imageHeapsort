using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HeapSort
{
    class External
    {
        private BinaryReader reader;
        private BinaryWriter writer;
        private FileStream stream;
        private int intSize;
        public int size;

        public External(string fileName)
        {
            stream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
            intSize = sizeof(int);
            size = 0;
        }

        public void Add(int element)
        {
            writer.Write(element);
            size++;
        }

        public Node Read(int index)
        {
            reader.BaseStream.Seek(index * intSize, SeekOrigin.Begin);
            Node node = new Node(reader.ReadInt32(), false);
            return node;
        }

        public Node[] toNodeArray(External e)
        {
            Node[] nodeArray = new Node[e.size];

            for (int i = 0; i < e.size; i++)
                nodeArray[i] = Read(i);

            return nodeArray;
        }
    }
}
