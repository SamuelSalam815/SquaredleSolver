﻿using GraphWalking.Graphs;
using SquaredleSolver;
using SquaredleSolver.SolverStates;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfSquaredleSolver.Command;

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

    public ISolverState SolverState => solverModel.CurrentState;

    private List<CharacterNode> PuzzleAsCharacterNodes
    {
        get { return puzzleModel.PuzzleAsAdjacencyList.GetAllNodes(); }
    }

    public ObservableCollection<CharacterGridViewModel> CharacterGridViewModels { get; }

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

    private TimeSpan _solverRunTime = TimeSpan.Zero;
    public TimeSpan SolverRunTime
    {
        get { return _solverRunTime; }
        set
        {
            _solverRunTime = value;
            OnPropertyChanged(nameof(SolverRunTime));
        }
    }

    private CancellationTokenSource solverRunTimeCancellationTokenSource;
    private readonly SolverModel solverModel;
    private readonly PuzzleModel puzzleModel;

    public MainWindowViewModel(PuzzleModel puzzleModel, SolverModel solverModel)
    {
        CharacterGridViewModels = new ObservableCollection<CharacterGridViewModel>();
        this.puzzleModel = puzzleModel;
        puzzleModel.PropertyChanged += OnPuzzleModelChanged;

        this.solverModel = solverModel;
        ToggleSolverOnOff = new ToggleSolverOnOff(solverModel);

        solverRunTimeCancellationTokenSource = new CancellationTokenSource();
        solverModel.StateChanged += OnSolverStateChanged;
        solverModel.AnswersFoundInPuzzle.CollectionChanged +=
            (sender, e) => Application.Current.Dispatcher.Invoke(() => OnAnswersFoundInPuzzleChanged(sender, e));
    }

    private void OnSolverStateChanged(object? sender, SolverStateChangedEventArgs e)
    {
        OnPropertyChanged(nameof(SolverState));

        if (e.CurrentState is SolverRunning)
        {
            solverRunTimeCancellationTokenSource = new CancellationTokenSource();
            UpdateSolverRunTime(
                TimeSpan.FromMilliseconds(50),
                solverRunTimeCancellationTokenSource.Token);
        }

        if (e.PreviousState is SolverRunning)
        {
            solverRunTimeCancellationTokenSource.Cancel();
            SolverRunTime = solverModel.StopTime - solverModel.StartTime;
        }
    }

    private async void UpdateSolverRunTime(TimeSpan updateInterval, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            SolverRunTime = DateTime.Now - solverModel.StartTime;
            await Task.Delay(updateInterval);
        }
    }

    private void OnAnswersFoundInPuzzleChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        IEnumerable<AnswerModel> answersToAdd;
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                answersToAdd = e.NewItems.Cast<AnswerModel>();
                break;
            default:
                CharacterGridViewModels.Clear();
                answersToAdd = solverModel.AnswersFoundInPuzzle;
                break;
        }

        foreach (AnswerModel answer in answersToAdd)
        {
            CharacterGridViewModels.Add(new CharacterGridViewModel(puzzleModel, answer));
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
