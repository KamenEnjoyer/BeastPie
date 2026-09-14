using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class NumbersConvertor : MonoBehaviour
{
    public static string ToRoman(int number)
    {
        if (number < 1 || number > 10) return "0";
        int[] values = {10, 9, 5, 4, 1};
        string[] symbols = {"X", "IX", "V", "IV", "I"};
        StringBuilder result = new StringBuilder();

        for (int i = 0; i < values.Length; i++)
        {
            while (number >= values[i])
            {
                number -= values[i];
                result.Append(symbols[i]);
            }
        }

        return result.ToString();
    }
}
