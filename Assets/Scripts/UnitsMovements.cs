using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsMovements
{
    private Stack<Command> _commands;

    public UnitsMovements()
    {
        _commands = new Stack<Command>();
    }

    public void addCommand(Command newCommand)
    {
        newCommand.execute();
       _commands.Push(newCommand); 
    }

    public void undoCommand()
    {
        if (_commands.Count > 0)
        {
            var lastCommand = _commands.Pop();
            lastCommand.undo();
        }
    }

    public int getCommandsCount()
    {
        return _commands.Count;
    }
}
