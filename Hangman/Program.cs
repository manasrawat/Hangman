using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using static System.Console;

namespace Hangman {
    class Program {
        static int lives, lettersFound;
        static int[] xPos = { 6, 4, 6, 4, 5, 5, 5 }, yPos = { 5, 5, 3, 3, 4, 3, 2 };
        static List<char> taken = new List<char>();
        static Random random = new Random();
        static void Main(string[] args) {
            Title = "Hangman";
            bool cont = true;
            do {
                lives = 6; lettersFound = 0;
                Clear();
                taken.Clear();
                string hangman = " ----¬";
                string[] parts = { "\\", "/", "-", "-", "|", "|", "O" };
                for (int i = 0; i < 5; i++) hangman += "\n |" + ((i == 0) ? "   |": null);
                hangman += "\n |\n---";
                ForegroundColor = ConsoleColor.DarkGreen; WriteLine(hangman);
                IEnumerable<string> dictionary = File.ReadLines("Dictionary.txt");
                int line = random.Next(0, dictionary.Count());
                string word = dictionary.ElementAt(line), dashes = "";
                foreach (char c in word) dashes += "-";
                ForegroundColor = ConsoleColor.DarkMagenta; WriteLine(dashes);
                while (lives >= 0 && lettersFound < word.Length) {
                    string letterstr = ReadLine();
                    bool addChar = true, deduct = true;
                    if (dictionary.Any(aword => aword == letterstr) && letterstr.Length == word.Length) {
                        if (letterstr == word) lettersFound = word.Length;
                    } else if (Regex.IsMatch(letterstr, @"^[A-Za-z]$")) {
                        char letter = Convert.ToChar(letterstr);
                        foreach (char c in taken) if (letter == c) addChar = false;
                        int currentY = CursorTop;
                        for (int i = 0; i < word.Length; i++) {
                            if (letter == word[i]) {
                                deduct = false;
                                if (addChar) {
                                    taken.Add(letter);
                                    SetCursorPosition(i, 8); WriteLine(word[i]);
                                    lettersFound += 1;
                                }
                            }
                        }
                    } else {
                        deduct = false;
                        CursorTop = 9; WriteLine(new string(' ', WindowWidth));
                        CursorTop = 9; ForegroundColor = ConsoleColor.DarkRed; WriteLine("Invalid input");
                        Thread.Sleep(1225);
                        CursorTop = 9; WriteLine("             ");
                        CursorTop = 9; ForegroundColor = ConsoleColor.DarkMagenta;
                    }
                    if (deduct) {
                        ForegroundColor = ConsoleColor.Green; SetCursorPosition(xPos[lives], yPos[lives]); WriteLine(parts[lives]);
                        lives -= 1;
                    }
                    ForegroundColor = ConsoleColor.DarkMagenta; CursorTop = 9; WriteLine(new string(' ', WindowWidth)); CursorTop = 9;
                }
                ForegroundColor = ConsoleColor.DarkCyan;
                WriteLine((lettersFound == word.Length) ? "Congratulations! You survived." : "You died. The word was " + word);
                ForegroundColor = ConsoleColor.DarkYellow; WriteLine("Wanna play again? Type 'no' to exit.");
                string answer = ReadLine();
                if (answer == "no") cont = false;
            } while (cont);
        }
    }
}
