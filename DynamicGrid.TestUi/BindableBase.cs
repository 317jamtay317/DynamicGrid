using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DynamicGrid.TestUi;

public class BindableBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetValue<T>(T value, [CallerMemberName] string propertyName = null)
    {
        try
        {
            
            _values[propertyName] = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }

    protected T GetValue<T>([CallerMemberName] string propertyName = null)
    {
        if (_values.ContainsKey(propertyName))
        {
            return (T)_values[propertyName];
        }

        return default(T);
    }

    private Dictionary<string, object> _values = new();
}