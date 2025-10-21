using QuizApp.ViewModels;


namespace QuizApp.Views;

public partial class ResultPage : ContentPage
{
    private readonly QuizViewModel _vm;

    public ResultPage(QuizViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        ShowResult();
    }

    private async void ShowResult()
    {
        
        string message = $"Congratulations, {_vm.UserName}.\n" +
                         $"Age: {_vm.UserAge}\n" +
                         $"Score: {_vm.Score}/{_vm.TotalQuestions}\n";

        ResultLabel.Text = message;
        //await DisplayAlert("Quiz Finished", message, "OK");
    }

    private async void OnFinishedClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();

    }


}