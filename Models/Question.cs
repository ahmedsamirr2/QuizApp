using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.Models;

public class Question
{
    public string Text { get; set; }
    public List<string> Options { get; set; }
    public int CorrectIndex { get; set; }
    public Question() { }
    public Question(string text, List<string> options, int correct)
    {
        Text = text;
        Options = options;
        CorrectIndex = correct;
    }
}


