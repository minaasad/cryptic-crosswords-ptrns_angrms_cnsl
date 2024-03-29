﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace m_crossword
{
    class MDictionary
    {
        public List<String> dictionary_words { get; set; }

        /// <summary>
        /// MDictionary Constructor
        /// </summary>
        public MDictionary()
        {
        }

        /// <summary>
        /// Loads lines (assumed to be words) from 
        /// a text file into a list of type String
        /// </summary>
        /// <param name="filePath">Path of text file</param>
        public void LoadFromFile(String filePath)
        {
            dictionary_words = new List<String>();
            string word;
            
            System.IO.StreamReader file = new System.IO.StreamReader(filePath);
            while ((word = file.ReadLine()) != null)
            {
                dictionary_words.Add(word);
            }
            file.Close();
        }
    }
}
