using System;
using System.Collections.Generic;
using System.Text;

namespace HeapSort
{
    class DiagonalOrder
    {
        public static int[,] Diagonal(int[] A, int x, int y)
        {
            int[,] arr = new int[x, y];
            int index = 0;

            var last = 0;
            //sort first part from upper left corner
            for (int k = 0; k < y; k++)
            {
                arr[x - 1, 0] = A[A.Length - 1];
                int i = x - 1;
                int j = k;

                while (!(i < 0 || i >= x || j >= y || j < 0))
                {
                    last = A[A.Length - 1 - index++];
                    arr[i, j] = last;
                    i--;
                    j--;
                }
            }

            index = 0;
            //sort second part from down right corner
            for (int k = 0; k < y; k++)
            {
                arr[0, y - 1] = A[0];
                int i = k;
                int j = y - 1;

                while (!(i < 0 || i >= x || j >= y || j < 0))
                {
                    if (!(index > last))
                    {
                        arr[i, j] = A[index++];
                        i--;
                        j--;
                    }
                }
            }
            return arr;
        }
    }
}
