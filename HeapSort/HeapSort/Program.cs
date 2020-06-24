using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace HeapSort
{
    class Program
    {
        static void Main(string[] args)
        {
            //Main image directory 
            var start = AppDomain.CurrentDomain.BaseDirectory;
            var pf = start + @"img1.jpg";
            //BMP sorted file 
            var pbmp = start + @"img1";
            //External save file
            string xdir = start + @"external.txt"; ;
            Bitmap image = new Bitmap(pf);
            var name = Path.GetFileNameWithoutExtension(pf);
            image.Save(name + ".bmp", ImageFormat.Bmp);

            using (FileStream file = new FileStream(pbmp + ".bmp", FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                Console.WriteLine("Enter 1 for External\nEnter 2 for Internal\nEnter 3 for InternalTest\nEnter 4 for ExternalTest");
                var input = Convert.ToInt32(Console.ReadLine());
                switch (input)
                {
                    case 1:
                        External(bytes, name, xdir);
                        break;
                    case 2:
                        Internal(bytes, name);
                        break;
                    case 3:
                        Console.WriteLine("Enter data size");
                        var valI = Convert.ToInt32(Console.ReadLine());
                        testInternal(bytes, name, valI);
                        break;
                    case 4:
                        Console.WriteLine("Enter data size");
                        var valE = Convert.ToInt32(Console.ReadLine());
                        testExternal(bytes, name, valE, xdir);
                        break;
                }
                Console.ReadLine();

                file.Close();
            }
        }

        public static void Internal(byte[] bytes, string name)
        {
            int width = BitConverter.ToInt32(bytes, 0x0012);
            int height = BitConverter.ToInt32(bytes, 0x0016);
            int[] bs = new int[width * height];
            Heap heap = new Heap();
            int j = 54;

            for (int i = 0; i < bs.Length; i++)
            {
                bs[i] = (((bytes[j + 2] << 8) + bytes[j + 1]) << 8) + bytes[j];
                Node node = new Node(bs[i], false);
                heap.Add(node);
                j += 3;
            }

            heap.HeapSort();

            j = 54;
            foreach (var n in DiagonalOrder.Diagonal(heap.toArray(), height, width))
            {
                byte[] p = BitConverter.GetBytes(n);
                bytes[j] = p[0];
                bytes[j + 1] = p[1];
                bytes[j + 2] = p[2];
                j += 3;
            }

            using (FileStream file2 = new FileStream(name + "_sorted.bmp", FileMode.Create, FileAccess.Write))
            {
                file2.Seek(0, SeekOrigin.Begin);
                file2.Write(bytes, 0, bytes.Length);
                file2.Close();
            }
        }

        public static void External(byte[] bytes, string name, string xdir)
        {
            int width = BitConverter.ToInt32(bytes, 0x0012);
            int height = BitConverter.ToInt32(bytes, 0x0016);
            int[] bs = new int[width * height];
            Heap heap = new Heap();
            External external = new External(xdir);
            int j = 54;

            for (int i = 0; i < bs.Length; i++)
            {
                bs[i] = (((bytes[j + 2] << 8) + bytes[j + 1]) << 8) + bytes[j];
                Node node = new Node(bs[i], false);
                heap.Add(node);
                j += 3;
            }

            int[] arr = heap.toArray();

            for (int i = 0; i < arr.Length; i++)
                external.Add(arr[i]);

            j = 54;
            foreach (var n in DiagonalOrder.Diagonal(heap.HeapSortExternal(external.toNodeArray(external)), height, width))
            {
                byte[] p = BitConverter.GetBytes(n);
                bytes[j] = p[0];
                bytes[j + 1] = p[1];
                bytes[j + 2] = p[2];
                j += 3;
            }
            using (FileStream file2 = new FileStream(name + "_sorted.bmp", FileMode.Create, FileAccess.Write))
            {
                file2.Seek(0, SeekOrigin.Begin);
                file2.Write(bytes, 0, bytes.Length);
                file2.Close();
            }
        }

        public static void testInternal(byte[] bytes, string name, int size)
        {
            Stopwatch stopwatch = new Stopwatch();
            Heap heap = new Heap();
            Random rnd = new Random();
            for (int i = 0; i < size; i++)
            {
                Node node = new Node(rnd.Next(0, i), false);
                heap.Add(node);
            }

            stopwatch.Start();
            heap.HeapSort();
            stopwatch.Stop();

            TimeSpan ts = stopwatch.Elapsed;
            string elapsedTimeI = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTimeI);
        }

        public static void testExternal(byte[] bytes, string name, int size, string xdir)
        {
            Stopwatch stopwatch = new Stopwatch();
            Heap heap = new Heap();
            External external = new External(xdir);
            Random rnd = new Random();

            for (int i = 0; i < size; i++)
            {
                Node node = new Node(rnd.Next(0, i), false);
                heap.Add(node);
            }

            int[] arr = heap.toArray();

            for (int i = 0; i < arr.Length; i++)
                external.Add(arr[i]);

            stopwatch.Start();
            heap.HeapSortExternal(external.toNodeArray(external));
            stopwatch.Stop();

            TimeSpan ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
        }

        public static void Print(int[] bs, string note)
        {
            Console.WriteLine(note);
            for (int i = 0; i < bs.Length; i++)
                Console.Write(bs[i] + " ");
            Console.WriteLine();
        }
    }
}
