using UnityEngine;

namespace Utilities
{
    static class Helper
    {
        public static void PrintMatrix<T>(T[,] matrix)
        {
            string s = "\n{";
            
            s += "Columns\t| ";
            for (int j = 0; j < matrix.GetLength(1); ++j)
            {
                s += j + ", ";
            }
            s = s.Remove(s.Length - 2);
            s += " }\n{";
            
            for (int i = 0; i < matrix.GetLength(0); ++i)
            {
                s += i + "\t| ";
                for (int j = 0; j < matrix.GetLength(1); ++j)
                {
                    s += matrix[i, j] + ", ";
                }
                s = s.Remove(s.Length - 2);
                s += " }\n{";
            }
            s = s.Remove(s.Length - 1);
            Debug.Log(s);
        }
    }
}