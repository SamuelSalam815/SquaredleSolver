using GraphWalking.Graphs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using WpfSquaredleSolver.Command;
using WpfSquaredleSolver.Model;

namespace WpfSquaredleSolver.ViewModel;

/// <summary>
///     Exposes the required properties for the user interface of the main window.
/// </summary>
internal class MainWindowViewModel : INotifyPropertyChanged
{

    public event PropertyChangedEventHandler? PropertyChanged;
    public ICommand ToggleSolverOnOff { get; }

    public string PuzzleAsText
    {
        get { return puzzleModel.PuzzleAsText; }
        set
        {
            puzzleModel.PuzzleAsText = value;
            OnPropertyChanged(nameof(PuzzleAsText));
        }
    }

    public uint NumberOfRowsInPuzzle => puzzleModel.NumberOfRows;
    public uint NumberOfColumnsInPuzzle => puzzleModel.NumberOfColumns;

    private bool isSolverRunningBackingField = false;
    public bool IsSolverRunning
    {
        get { return isSolverRunningBackingField; }
        set
        {
            isSolverRunningBackingField = value;
            OnPropertyChanged(nameof(IsSolverRunning));
        }
    }

    private List<CharacterNode> PuzzleAsCharacterNodes
    {
        get { return puzzleModel.PuzzleAsAdjacencyList.GetAllNodes(); }
    }

    public BindingList<CharacterGridViewModel> CharacterGridViewModels { get; }

    private double backingWrapPanelWidth;
    public double WrapPanelWidth
    {
        get { return backingWrapPanelWidth; }
        set
        {
            backingWrapPanelWidth = value;
            OnPropertyChanged(nameof(WrapPanelWidth));
        }
    }

    private readonly SolverModel solverModel;
    private readonly PuzzleModel puzzleModel;

    public MainWindowViewModel(PuzzleModel puzzleModel, SolverModel solverModel)
    {
        CharacterGridViewModels = new BindingList<CharacterGridViewModel>();
        this.puzzleModel = puzzleModel;
        puzzleModel.PropertyChanged += OnPuzzleModelChanged;

        this.solverModel = solverModel;
        ToggleSolverOnOff = new ToggleSolverOnOff(solverModel, this);

        solverModel.StateChanged += OnSolverStateChanged;
        solverModel.AnswersFoundInPuzzle.ListChanged += OnAnswersFoundInPuzzleChanged;
    }

    private void OnSolverStateChanged(object? sender, SolverStateChangedEventArgs e)
    {
        IsSolverRunning = e.CurrentState is SolverRunning;
    }

    private void OnAnswersFoundInPuzzleChanged(object? sender, ListChangedEventArgs e)
    {
        switch (e.ListChangedType)
        {
            case ListChangedType.ItemAdded:
                AnswerModel nextAnswer = solverModel.AnswersFoundInPuzzle[e.NewIndex];
                CharacterGridViewModels.Add(new CharacterGridViewModel(puzzleModel, nextAnswer));
                break;
            default:
                CharacterGridViewModels.Clear();
                foreach (AnswerModel answer in solverModel.AnswersFoundInPuzzle)
                {
                    CharacterGridViewModels.Add(new CharacterGridViewModel(puzzleModel, answer));
                }
                break;
        }
    }

    private void OnPropertyChanged(string nameOfProperty)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(nameOfProperty));
    }

    private void OnPuzzleModelChanged(object? sender, PropertyChangedEventArgs args)
    {
        switch (args.PropertyName)
        {
            case nameof(puzzleModel.PuzzleAsAdjacencyList):
                OnPropertyChanged(nameof(PuzzleAsCharacterNodes));
                break;
            case nameof(puzzleModel.NumberOfRows):
                OnPropertyChanged(nameof(NumberOfRowsInPuzzle));
                break;
            case nameof(puzzleModel.NumberOfColumns):
                OnPropertyChanged(nameof(NumberOfColumnsInPuzzle));
                break;
        }
    }

}
