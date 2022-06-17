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
       _commands.Push(newCommand); 
        newCommand.execute();
    }

    public void undoCommand()
    {
        var lastCommand = _commands.Pop();
        lastCommand.undo();
    }
}
