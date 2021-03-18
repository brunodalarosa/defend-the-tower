using System;
using UnityEngine;

namespace TowerDefense.Scripts.Extensions
{
    public static class ArrayExtensions
    {
        public static void Print2DArray(this int[,] array)
        {
            string printString = string.Empty;
            
            for (int x = 0; x < array.GetLength(0); x++)
            {
                for (int y = 0; y < array.GetLength(1); y++)
                {
                    printString += array[x, y].ToString();

                    if (y + 1 < array.GetLength(1)) printString += ",";
                }

                if (x + 1 < array.GetLength(0)) printString += Environment.NewLine;
            }
            
            Debug.Log(printString);
        }
    }
}