using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsMovements
{
    private List<Command> _commands;

    public UnitsMovements()
    {
        _commands = new List<Command>();
    }

    public void addCommand(Command newCommand)
    {
        newCommand.execute();
       _commands.Add(newCommand); 
    }

}
