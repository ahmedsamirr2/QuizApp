using QuizApp.Views;
using QuizApp.ViewModels;

namespace QuizApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    // 🔹 When user enters question count, disable time input
    private void QuestionCountEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(QuestionCountEntry.Text))
        {
            TimeEntry.Text = string.Empty;
            TimeEntry.IsEnabled = false;
        }
        else
        {
            TimeEntry.IsEnabled = true;
        }
    }

    // 🔹 When user enters time, disable question count input
    private void TimeEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(TimeEntry.Text))
        {
            QuestionCountEntry.Text = string.Empty;
            QuestionCountEntry.IsEnabled = false;
        }
        else
        {
            QuestionCountEntry.IsEnabled = true;
        }
    }

    private async void StartQuiz_Clicked(object sender, EventArgs e)
    {
        string name = NameEntry.Text;
        string ageText = AgeEntry.Text;

        if (string.IsNullOrWhiteSpace(name) || !int.TryParse(ageText, out int age))
        {
            await DisplayAlert("Error", "Please enter a valid name and age.", "OK");
            return;
        }

        int? count = int.TryParse(QuestionCountEntry.Text, out int qCount) ? qCount : null;
        int? totalTime = int.TryParse(TimeEntry.Text, out int minutes) ? minutes * 60 : null;

        if (count == null && totalTime == null)
        {
            await DisplayAlert("Error", "Please enter either the number of questions or the quiz time.", "OK");
            return;
        }

        var vm = new QuizViewModel(name, age, count, totalTime);
        await Navigation.PushAsync(new QuizPage(vm));
    }
}







