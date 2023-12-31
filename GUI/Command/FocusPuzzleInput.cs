using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GUI.Command;
internal class FocusPuzzleInput : ICommand
{
    private readonly TextBox puzzleInput;
    public event EventHandler? CanExecuteChanged;

    public FocusPuzzleInput(TextBox puzzleInput)
    {
        this.puzzleInput = puzzleInput;
    }

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        puzzleInput.Visibility = Visibility.Visible;
        puzzleInput.Focus();
    }
}
