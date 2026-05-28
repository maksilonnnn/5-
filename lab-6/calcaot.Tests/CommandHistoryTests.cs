using calcaot.ViewModels.Commands;
using calcaot.Interfaces;

namespace calcaot.Tests;

internal class FakeCommand : IUndoableCommand
{
    public int ExecuteCount { get; private set; }
    public int UndoCount { get; private set; }

    public void Execute() => ExecuteCount++;
    public void Undo() => UndoCount++;
}

public class CommandHistoryTests
{
    [Fact]
    public void Initially_CannotUndoOrRedo()
    {
        var history = new CommandHistory();

        Assert.False(history.CanUndo);
        Assert.False(history.CanRedo);
    }

    [Fact]
    public void AfterExecute_CanUndo()
    {
        var history = new CommandHistory();
        history.Execute(new FakeCommand());

        Assert.True(history.CanUndo);
    }

    [Fact]
    public void AfterExecute_CannotRedo()
    {
        var history = new CommandHistory();
        history.Execute(new FakeCommand());

        Assert.False(history.CanRedo);
    }

    [Fact]
    public void Execute_CallsCommandExecute()
    {
        var history = new CommandHistory();
        var cmd = new FakeCommand();

        history.Execute(cmd);

        Assert.Equal(1, cmd.ExecuteCount);
    }

    [Fact]
    public void AfterUndo_CanRedo()
    {
        var history = new CommandHistory();
        history.Execute(new FakeCommand());
        history.Undo();

        Assert.True(history.CanRedo);
    }

    [Fact]
    public void AfterSingleUndo_CannotUndoAgain()
    {
        var history = new CommandHistory();
        history.Execute(new FakeCommand());
        history.Undo();

        Assert.False(history.CanUndo);
    }

    [Fact]
    public void Undo_CallsCommandUndo()
    {
        var history = new CommandHistory();
        var cmd = new FakeCommand();
        history.Execute(cmd);

        history.Undo();

        Assert.Equal(1, cmd.UndoCount);
    }

    [Fact]
    public void Execute_ClearsRedoStack()
    {
        var history = new CommandHistory();
        history.Execute(new FakeCommand());
        history.Undo();

        Assert.True(history.CanRedo);

        history.Execute(new FakeCommand());

        Assert.False(history.CanRedo);
    }

    [Fact]
    public void Redo_ReExecutesCommand()
    {
        var history = new CommandHistory();
        var cmd = new FakeCommand();
        history.Execute(cmd);
        history.Undo();
        history.Redo();

        Assert.Equal(2, cmd.ExecuteCount);
    }

    [Fact]
    public void AfterRedo_CannotRedoAgain()
    {
        var history = new CommandHistory();
        history.Execute(new FakeCommand());
        history.Undo();
        history.Redo();

        Assert.False(history.CanRedo);
    }

    [Fact]
    public void Execute_RaisesCommandExecutedEvent()
    {
        var history = new CommandHistory();
        bool eventFired = false;
        history.CommandExecuted += (_, _) => eventFired = true;

        history.Execute(new FakeCommand());

        Assert.True(eventFired);
    }

    [Fact]
    public void Execute_RaisesPropertyChangedForCanUndo()
    {
        var history = new CommandHistory();
        var changedProperties = new List<string?>();
        history.PropertyChanged += (_, e) => changedProperties.Add(e.PropertyName);

        history.Execute(new FakeCommand());

        Assert.Contains("CanUndo", changedProperties);
    }
}
