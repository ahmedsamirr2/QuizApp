using QuizApp.ViewModels;
using System.Timers;

namespace QuizApp.Views;

public partial class QuizPage : ContentPage
{
    private readonly QuizViewModel _vm;
    private System.Timers.Timer _quizTimer;
    private System.Timers.Timer _questionTimer;

    private int _quizTimeLeft;
    private int _questionTimeLeft = 20;
    private bool _useTimers = false;


    public QuizPage(QuizViewModel vm)
    {
        InitializeComponent();
        _vm = vm;

        // Enable timers only if total time was set
        _useTimers = _vm.TotalTimeSeconds.HasValue;

        if (_useTimers)
        {
            _quizTimeLeft = _vm.TotalTimeSeconds.Value;
            StartQuizTimer();
        }

        LoadQuestion();
    }


    private void StartQuizTimer()
    {
        _quizTimer = new System.Timers.Timer(1000);
        _quizTimer.Elapsed += (s, e) =>
        {
            _quizTimeLeft--;
            MainThread.BeginInvokeOnMainThread(() =>
                QuizTimerLabel.Text = $"Quiz Time Left: {_quizTimeLeft}s");

            if (_quizTimeLeft <= 0)
            {
                _quizTimer.Stop();
                MainThread.BeginInvokeOnMainThread(async () => await ShowResult());
            }
        };
        _quizTimer.Start();
    }

    private void StartQuestionTimer()
    {
        if (!_useTimers) return; // No timer in question-count mode

        _questionTimeLeft = 20;
        QuestionTimerLabel.Text = $"Time Left: {_questionTimeLeft}s";

        _questionTimer = new System.Timers.Timer(1000);
        _questionTimer.Elapsed += (s, e) =>
        {
            _questionTimeLeft--;

            MainThread.BeginInvokeOnMainThread(() =>
                QuestionTimerLabel.Text = $"Time Left: {_questionTimeLeft}s");

            if (_questionTimeLeft <= 0)
            {
                _questionTimer.Stop();
                MainThread.BeginInvokeOnMainThread(() => MoveToNextQuestion());
            }
        };
        _questionTimer.Start();
    }

    private void LoadQuestion()
    {
        if (_vm.IsFinished)
        {
            ShowResult();
            return;
        }

        _questionTimer?.Stop();
        if (_useTimers)
            StartQuestionTimer();

        var question = _vm.Current;
        CounterLabel.Text = $"Question {_vm.QuestionNumber} of {_vm.TotalQuestions}";
        QuestionLabel.Text = question.Text;

        OptionsGrid.Children.Clear();
        OptionsGrid.RowDefinitions.Clear();

        int totalOptions = question.Options.Count;
        int rows = (int)Math.Ceiling(totalOptions / 2.0);

        for (int i = 0; i < rows; i++)
        {
            OptionsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        }

        for (int i = 0; i < totalOptions; i++)
        {
            var optionButton = new Button
            {
                Text = question.Options[i],
                Style = (Style)Application.Current.Resources["HoverButtonStyle"]
            };

            int index = i;
            optionButton.Clicked += (s, e) => OnOptionSelected(index);

            int row = i / 2;
            int col = i % 2;
            OptionsGrid.Add(optionButton, col, row);
        }
    }

    private void MoveToNextQuestion()
    {
        bool finished = _vm.AnswerQuestion(-1);
        if (finished)
            ShowResult();
        else
            LoadQuestion();
    }

    private async void OnOptionSelected(int index)
    {
        _questionTimer?.Stop();

        bool finished = _vm.AnswerQuestion(index);

        if (finished)
        {
            await ShowResult();
        }
        else
        {
            LoadQuestion();
        }
    }

    private async Task ShowResult()
    {
        _quizTimer?.Stop();
        _questionTimer?.Stop();
        await Navigation.PushAsync(new ResultPage(_vm));
    }
}





