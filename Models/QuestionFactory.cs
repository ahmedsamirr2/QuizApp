using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections.Generic;


namespace QuizApp.Models;

public static class QuestionFactory
{
    
    private static readonly Random random = new();
    private static readonly string[] operations = { "+", "-", "*", "/" };
    private static readonly DataTable evaluator = new();
    
    public static List<Question> GenerateMathQuestions(int count)
    {
        var questions = new List<Question>();

        for (int i = 0; i < count; i++) 
        {
            int x = random.Next(1,21);
            int y = random.Next(1,21);
            string op = operations[random.Next(operations.Length)];
            

            if(op == "/" && y ==0)
                y = 1;

            string expression = $"{x} {op} {y}";

            double correctAnswer = Math.Round(Convert.ToDouble(evaluator.Compute(expression, null)), 2);
            
            
            var options = new List<string>();
            int correctIndex = random.Next(4);

            for (int j = 0; j < 4; j++) 
            {

                if (j == correctIndex) 
                {
                    options.Add(correctAnswer.ToString());
                    
                }

                else
                {
                    double wrongAnswer = correctAnswer + random.Next(-5, 6);
                    options.Add(wrongAnswer.ToString());
                }


            }

                questions.Add(new Question
                {
                    Text = $"What is {x} {op} {y}?",
                    Options = options,
                    CorrectIndex =correctIndex
                    
                });
        }

                return questions;
    }
    
}

