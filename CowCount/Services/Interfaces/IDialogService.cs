namespace CowCount.Services.Interfaces
{
    public interface IDialogService
    {
        object? ShowDialog(object viewModel);
        object? Show(object viewModel);
    }
}