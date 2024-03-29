﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m_crossword
{
    class Program
    {
        private static MDictionary newDictionary;

        /// <summary>
        /// Main method when application starts. Handles
        /// user entered arguments accordingly.
        /// </summary>
        /// <param name="args">Arguments passed to application</param>
        static void Main(string[] args)
        {
            Console.WriteLine("Crossword Helper by Mina Asad");

            if (!IsArgsValid(args)) return;

            newDictionary = new MDictionary();
            newDictionary.LoadFromFile(args[1]);

            int matchesFound = 0;
            switch (args[0])
            {
                case "A":
                    //Anagrams
                    matchesFound = WriteOutAnagrams(args[2], matchesFound);
                    break;

                case "P":
                    //Patterns
                    //if (!args[2].Contains("_"))
                    //{ 
                    //    Console.WriteLine("No placeholders/underscores found.");
                    //    break;
                    //}
                    matchesFound = WriteOutPatterns(args[2], matchesFound);
                    break;
            }
            Console.WriteLine("Matches: " + matchesFound);
        }

        /// <summary>
        /// This method validates arguments passed to my crosswords console application.
        /// The format should consist of the following command line arguments:
        /// [A|P] [dictionary] [letters]
        /// </summary>
        /// <param name="args">String array of arguments</param>
        /// <returns>True if arguments are valid or false is there is something wrong</returns>
        private static Boolean IsArgsValid(string[] args)
        {
            try
            {
                //Validate number of arguments
                if (args.Length != 3)
                {
                    Console.WriteLine("Invalid number of arguments.");
                    Console.WriteLine("Please use the format: m_crossword [A|P] [dictionary] [letters]");
                    return false;
                }

                //Validate against any numbers or digits
                if (args[2].Any(c => char.IsDigit(c)))
                {
                    Console.WriteLine("Invalid argument(s).");
                    Console.WriteLine("Numbers or digits are not allowed");
                    return false;
                }

                //Validate against any unwanted punctuation characters
                foreach (char user_word_char in args[2])
                {
                    if (user_word_char.Equals('_'))
                    {
                        //Do nothing: this punctuation letter is an exception.
                    }
                    else if (char.IsPunctuation(user_word_char))
                    {
                        Console.WriteLine("Invalid argument(s).");
                        Console.WriteLine("Punctuation characters are not allowed");
                        return false;
                    }
                }

                //Validate first argument to be "A" or "P"
                if (!(args[0].Equals("A") | args[0].Equals("P")))
                {
                    Console.WriteLine("Invalid first argument '" + args[0] + "'");
                    Console.WriteLine("Please use the format: [A|P]");
                    return false;
                }

                //Check if dictionary file is empty
                if (new FileInfo(args[1]).Length == 0)
                {
                    Console.WriteLine("Invalid second argument '" + args[1] + "'");
                    Console.WriteLine("Please make sure it is a non-empty readable dictionary file");
                    return false;
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Invalid second argument '" + args[1] + "'");
                Console.WriteLine("File not found.");
                return false;
            }
            catch (Exception m)
            {
                Console.WriteLine("Exception: '" + m + "'");
                return false;
            }
            return true;
        }

        /// <summary>
        /// This method should performed if first argument is "A" (anagram).
        /// It passes the user's word and checks it for anagrams against 
        /// the dictionary words. It also writes in the console output its'
        /// results as well as keeps track of how many matches were found.
        /// </summary>
        /// <param name="user_word">String letters that user entered</param>
        /// <param name="counter">Starting position of counter. Ideally 0</param>
        /// <returns>Number of anagram matches found</returns>
        private static int WriteOutAnagrams(string user_word, int counter)
        {
            Cryptic cChecker = new Cryptic();
            foreach (var dictionary_word in newDictionary.dictionary_words)
            {
                if (cChecker.IsAnagram(dictionary_word, user_word))
                {
                    counter++;
                    Console.WriteLine(counter + ": " + dictionary_word);
                }
            }
            return counter;
        }

        /// <summary>
        /// This method should performed if first argument is "P" (pattern).
        /// It passes the user's word and checks it for patterns against 
        /// the dictionary words. It also writes in the console output its'
        /// results as well as keeps track of how many matches were found.
        /// </summary>
        /// <param name="user_word">String letters that user entered</param>
        /// <param name="counter">Starting position of counter. Ideally 0</param>
        /// <returns>Number of pattern matches found</returns>
        private static int WriteOutPatterns(string user_word, int counter)
        {
            Cryptic cChecker = new Cryptic();
            List<string> fixedLetters = new List<string>();

            for (int i = 0; i < user_word.Length; i++)
            {
                if (user_word[i] != '_')
                {
                    fixedLetters.Add(i + ":" + user_word[i]);
                }
            }

            foreach (var dictionary_word in newDictionary.dictionary_words)
            {
                if (cChecker.IsPattern(dictionary_word, fixedLetters, user_word.Length))
                {
                    counter++;
                    Console.WriteLine(counter + ": " + dictionary_word);
                }
            }
            return counter;
        }

    }
}
