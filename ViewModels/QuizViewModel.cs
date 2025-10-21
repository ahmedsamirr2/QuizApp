using QuizApp.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuizApp.ViewModels;

public class QuizViewModel : INotifyPropertyChanged
{
    private int _index;
    private int _score;

    public event PropertyChangedEventHandler PropertyChanged;

    public string UserName { get; }
    public int UserAge { get; }
    public ObservableCollection<Question> Questions { get; }
    public int? TotalTimeSeconds { get; }

    public Question Current => Questions[_index];
    public int QuestionNumber => _index + 1;
    public int TotalQuestions => Questions.Count;
    public int Score => _score;
    public bool IsFinished => _index >= Questions.Count;

    public QuizViewModel(string name, int age, int? totalQuestions = null, int? totalTimeSeconds = null)
    {
        UserName = name;
        UserAge = age;
        TotalTimeSeconds = totalTimeSeconds;

        int count = totalQuestions ?? 10; // default if not provided
        Questions = new ObservableCollection<Question>(QuestionFactory.GenerateMathQuestions(count));
    }

    public bool AnswerQuestion(int answerIndex)
    {
        if (IsFinished) return true;

        if (answerIndex >= 0 && Current.CorrectIndex == answerIndex)
            _score++;

        _index++;

        OnPropertyChanged(nameof(Current));
        OnPropertyChanged(nameof(QuestionNumber));
        OnPropertyChanged(nameof(IsFinished));
        return IsFinished;
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}










