using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class WordCheck
    {
        public static bool CheckingWord(string word, List<string> ForbiddenNames)
        {
            word = word.ToLower();

            // if string is empty then it means no forbidden name used
            if( word == "")
            {
                return false;
            }
            
            
            if(ForbiddenNames.Contains(word))
            {
                return true;
            }

            //removing last char of the string
            word = word.Remove(word.Length - 1);
            return CheckingWord(word, ForbiddenNames);//recursive call 
        }
    }
}