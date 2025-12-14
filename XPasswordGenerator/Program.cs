﻿// X Password Generator (Kagansoft Account Security Project)
// Copyright (C) 2025 Kagansoft Corporation

// This program Is free software: you can redistribute it And/Or modify
// it under the terms Of the GNU General Public License As published by
// the Free Software Foundation, either version 3 Of the License, Or
// (at your Option) any later version.

// This program Is distributed In the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty Of
// MERCHANTABILITY Or FITNESS For A PARTICULAR PURPOSE. See the
// GNU General Public License For more details.

// You should have received a copy Of the GNU General Public License
// along With this program. If Not, see <https://www.gnu.org/licenses/>.

using System;
using System.Text;
using System.Linq;
using System.Threading;
using System.Security.Cryptography;

public class Program
{
    private const int MIN_LENGTH = 30;

    private static void SetConsoleTitle()
    {
        Console.Title = "X Password Generator";
    }

    private static string GeneratePassword(int length)
    {
        const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string DigitChars = "0123456789";
        const string SymbolChars = "!@#$%^&*()_-+=[]{}|;:,.<>/?";

        string allChars = LowercaseChars + UppercaseChars + DigitChars + SymbolChars;

        StringBuilder passwordBuilder = new StringBuilder();

        using (var rng = RandomNumberGenerator.Create())
        {
            passwordBuilder.Append(GetRandomChar(LowercaseChars, rng));
            passwordBuilder.Append(GetRandomChar(UppercaseChars, rng));
            passwordBuilder.Append(GetRandomChar(DigitChars, rng));
            passwordBuilder.Append(GetRandomChar(SymbolChars, rng));

            int remainingLength = length - passwordBuilder.Length;

            for (int i = 0; i < remainingLength; i++)
            {
                passwordBuilder.Append(GetRandomChar(allChars, rng));
            }

            string finalPassword = passwordBuilder.ToString();

            char[] chars = finalPassword.ToCharArray();
            Shuffle(chars, rng);

            return new string(chars);
        }
    }

    private static char GetRandomChar(string charSet, RandomNumberGenerator rng)
    {
        byte[] buffer = new byte[1];
        rng.GetBytes(buffer);

        int index = buffer[0] % charSet.Length;
        return charSet[index];
    }

    private static void Shuffle(char[] array, RandomNumberGenerator rng)
    {
        byte[] buffer = new byte[4];
        int n = array.Length;

        while (n > 1)
        {
            rng.GetBytes(buffer);
            int k = BitConverter.ToInt32(buffer, 0);
            k = Math.Abs(k) % n;

            n--;
            char value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
    }

    public static void Main(string[] args)
    {
        SetConsoleTitle();

        int length = 0;
        int count = 0;

        while (count <= 0)
        {
            Console.Write("Enter number of passwords to generate: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out count))
            {
                if (count <= 0)
                {
                    Console.Clear();
                    Console.WriteLine("Number of passwords must be 1 or more! Please try again.");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Invalid input. Please enter a valid number.");
                count = 0;
            }
        }

        Console.Clear();

        while (length < MIN_LENGTH)
        {
            Console.Write($"Enter password length (Minimum {MIN_LENGTH} characters): ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out length))
            {
                if (length < MIN_LENGTH)
                {
                    Console.Clear();
                    Console.WriteLine($"Length must be a minimum of {MIN_LENGTH} characters! Please try again.");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Invalid input. Please enter a valid number.");
                length = 0;
            }
        }

        Console.Clear();

        for (int i = 1; i <= count; i++)
        {
            string password = GeneratePassword(length);
            Console.WriteLine($"Password {i}: {password}");
        }

        Console.WriteLine(" ");
        Console.WriteLine($"Generated {count} Passwords of {length} Characters");
        Console.WriteLine("You can choose and copy one of the passwords.");
        Console.WriteLine(" ");

        Thread.Sleep(1000);
        Console.Write("Press any key to exit...");
        Console.ReadKey();
    }
}