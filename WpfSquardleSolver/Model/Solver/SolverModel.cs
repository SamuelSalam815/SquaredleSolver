﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WpfSquaredleSolver.Model;

class SolverModel
{
    private SolverContext context;
    public ISolverState CurrentState => context.CurrentState;
    public ObservableCollection<AnswerModel> AnswersFoundInPuzzle => context.AnswersFoundInPuzzle;


    public EventHandler<SolverStateChangedEventArgs>? StateChanged;

    public SolverModel(PuzzleModel puzzleModel)
    {

        context = new SolverContext(puzzleModel);
        context.StateChanged += (sender, e) => StateChanged?.Invoke(this, e);

        puzzleModel.PropertyChanged += OnPuzzleModelChanged;
    }

    private void OnPuzzleModelChanged(object? sender, PropertyChangedEventArgs args)
    {
        CurrentState.OnPuzzleModelChanged(context);
    }

    public void StartSolvingPuzzle()
    {
        CurrentState.StartSolution(context);
    }

    public void StopSolvingPuzzle()
    {
        CurrentState.StopSolution(context);
    }


}