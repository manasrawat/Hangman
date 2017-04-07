using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using static System.Console;

namespace Hangman {
    class Program {
        static int[] xPos = { 5, 4, 6, 5, 4, 6 }, yPos = { 2, 3, 3, 4, 5, 5 };
        static List<char> taken = new List<char>();
        static Random random = new Random();
        static void Main(string[] args) {
            bool cont = true;
            do {
                int lives = 5, lettersFound = 0;
                Clear();
                taken.Clear();
                string hangman = " ----¬";
                string[] parts = { " |", " O", "- -", " |", "/ \\" };
                foreach (string part in parts) hangman += "\n |  " + part;
                hangman += "\n |\n---";
                ForegroundColor = ConsoleColor.Green; WriteLine(hangman);
                IEnumerable<string> dictionary = File.ReadLines("Dictionary.txt");
                int line = random.Next(0, dictionary.Count());
                string word = dictionary.ElementAt(line), dashes = "";
                foreach (char c in word) dashes += "-";
                ForegroundColor = ConsoleColor.DarkMagenta; WriteLine(dashes + "\n" + word);
                while (lives >= 0 && lettersFound < word.Length) {
                    string letterstr = ReadLine();
                    while (!Regex.IsMatch(letterstr, @"^[A-Za-z]$")) {
                        CursorTop = 10; ForegroundColor = ConsoleColor.DarkRed; WriteLine("Invalid input");
                        Thread.Sleep(1250);
                        CursorTop = 10; ForegroundColor = ConsoleColor.DarkMagenta; WriteLine("             ");
                        CursorTop = 10; letterstr = ReadLine();
                    }
                    char letter = Convert.ToChar(letterstr);
                    bool addChar = true, charPresent = false;
                    foreach (char c in taken) if (letter == c) addChar = false;
                    int currentY = CursorTop;
                    for (int i = 0; i < word.Length; i++) {
                        if (letter == word[i]) {
                            charPresent = true;
                            if (addChar) {
                                taken.Add(letter);
                                SetCursorPosition(i, 8); WriteLine(word[i]);
                                lettersFound += 1;
                            }
                        }
                    }
                    if (!charPresent) {
                        SetCursorPosition(xPos[lives], yPos[lives]); WriteLine(" ");
                        lives -= 1;
                    }
                    CursorTop = 10; WriteLine(" "); CursorTop = 10;
                }
                ForegroundColor = ConsoleColor.DarkCyan;
                if (lettersFound == word.Length) {
                    WriteLine("Congratulations! You survived.");
                }
                else {
                    WriteLine("Unfortunately... you died.");
                }
                ForegroundColor = ConsoleColor.DarkYellow; WriteLine("Wanna play again? Type 'no' to exit.");
                string answer = ReadLine();
                if (answer == "no") cont = false;
            } while (cont);
        }
    }
}
