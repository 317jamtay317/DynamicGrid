using System;
using System.Windows.Input;

namespace DynamicGrid.TestUi;

public class MainWindowViewModel : BindableBase
{
    public MainWindowViewModel()
    {
        AddColumnCommand = new DelegateCommand(() => Columns++);
        AddRowCommand = new DelegateCommand(() => Rows++);
        RemoveRowCommand = new DelegateCommand(() => Rows--);
        RemoveColumnCommand = new DelegateCommand(() => Columns--);

        Columns = 6;
        Rows = 5;
        IsConfigure = true;
    }
    public int Columns
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public int Rows
    {
        get => GetValue<int>();
        set => SetValue(value);
    }

    public bool IsConfigure
    {
        get => GetValue<bool>();
        set => SetValue(value);
    }
    
    public ICommand AddColumnCommand { get; }
    
    public ICommand AddRowCommand { get; }
    
    public ICommand RemoveColumnCommand { get; }
    
    public ICommand RemoveRowCommand { get; }
}

public class DelegateCommand : ICommand
{
    private readonly Action _execute;

    public DelegateCommand(Action execute)
    {
        _execute = execute;
    }
    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        _execute();
    }

    public event EventHandler? CanExecuteChanged;
}