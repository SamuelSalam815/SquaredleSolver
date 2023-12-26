using GraphWalking.Graphs;
using System.ComponentModel;
using System.Windows.Input;
using WpfSquardleSolver.Command;
using WpfSquardleSolver.Model;

namespace WpfSquardleSolver.ViewModel;

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

    private System.Collections.Generic.List<CharacterNode> PuzzleAsCharacterNodes
    {
        get { return puzzleModel.PuzzleAsAdjacencyList.GetAllNodes(); }
    }

    public BindingList<AnswerModel> AnswersFoundInPuzzle =>
        solverModel.AnswersFoundInPuzzle;

    private readonly SolverModel solverModel;
    private readonly PuzzleModel puzzleModel;

    public MainWindowViewModel(PuzzleModel puzzleModel, SolverModel solverModel)
    {
        this.puzzleModel = puzzleModel;
        puzzleModel.PropertyChanged += OnPuzzleModelChanged;

        this.solverModel = solverModel;
        solverModel.SolverStarted += () => IsSolverRunning = true;
        solverModel.SolverStopped += () => IsSolverRunning = false;
        ToggleSolverOnOff = new ToggleSolverOnOff(solverModel, this);
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
