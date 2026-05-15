using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using calcaot.Interfaces;

namespace calcaot.ViewModels.Commands
{
    /// <summary>
    /// История команд — паттерн Command (Команда), роль Invoker (Вызывателя).
    ///
    /// CommandHistory управляет выполнением и отменой команд.
    /// Хранит два стека:
    ///   • _undoStack — выполненные команды, которые можно отменить;
    ///   • _redoStack — отменённые команды, которые можно повторить.
    ///
    /// Как это работает:
    ///   При вызове Execute(command) — команда выполняется и кладётся в _undoStack.
    ///   При вызове Undo() — последняя команда извлекается из _undoStack,
    ///   вызывается её Undo(), и она перемещается в _redoStack.
    ///   При вызове Redo() — команда извлекается из _redoStack и выполняется снова.
    ///
    /// Свойства CanUndo и CanRedo реализуют INotifyPropertyChanged,
    /// чтобы кнопки «Отменить» и «Повторить» в UI автоматически
    /// становились активными или неактивными.
    /// </summary>
    public class CommandHistory : INotifyPropertyChanged
    {
        private readonly Stack<IUndoableCommand> _undoStack = new();
        private readonly Stack<IUndoableCommand> _redoStack = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        public void Execute(IUndoableCommand command)
        {
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear();

            NotifyChanged();
        }

        public void Undo()
        {
            if (!_undoStack.TryPop(out var command))
                return;

            command.Undo();
            _redoStack.Push(command);

            NotifyChanged();
        }

        public void Redo()
        {
            if (!_redoStack.TryPop(out var command))
                return;

            command.Execute();
            _undoStack.Push(command);

            NotifyChanged();
        }

        private void NotifyChanged()
        {
            OnPropertyChanged(nameof(CanUndo));
            OnPropertyChanged(nameof(CanRedo));
            System.Windows.Input.CommandManager.InvalidateRequerySuggested();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
