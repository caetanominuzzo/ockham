using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Linq;

namespace UndoRedoFramework
{
    /// <summary>
    /// Represents a node in a multi-way tree.
    /// It node can have just one parent and multiple children.
    /// </summary>
    internal class TreeCommand
    {
        private List<TreeCommand> _branches;

        private TreeCommand _mostRecentChildCommand, _parentCommand;

        private Command _command;

        /// <summary>
        /// Initializes a new instance of the TreeCommmand Class.
        /// </summary>
        /// <param name="command">The command to be associated to the Tree.</param>
        public TreeCommand(Command command)
        {
            _command = command;
            _branches = new List<TreeCommand>();
        }

        /// <summary>
        /// Initializes a new instance of the TreeCommmand Class.
        /// <remarks>This construct is usefull to indicates the root state in the tree. It takes no Command.</remarks>
        /// </summary>
        public TreeCommand() : this(null)
        {
            
        }

        /// <summary>
        /// Gets the 
        /// </summary>
        public Command Command
        {
            get { 
                return _command; }
        }

        /// <summary>
        /// Gets the most recent child Command of this TreeCommand node.
        /// </summary>
        public TreeCommand MostRecentChildCommand
        {
            get { return _mostRecentChildCommand; }
        }

        /// <summary>
        /// Gets the TreeCommand node parent of this TreeCommand node.
        /// </summary>
        public TreeCommand ParentCommand
        {
            get { return _parentCommand; }
        }

        /// <summary>
        /// Adds a new Command 
        /// </summary>
        /// <param name="command">The new Command node to be added to this TreeCommand node.</param>
        /// <remarks>If the Command alredy exists as a branch of this node</remarks>
        public void AddChild(Command command)
        {
            //TODO: Here it's using Command.Caption to verify identity.
            //Better was to compare by Command items.
            var v = (from b in _branches where b.Command.Caption == command.Caption select b);
        
            TreeCommand tmp = v.Count() == 1 ? v.First() : null;

            if (tmp == null)
            {
                _mostRecentChildCommand = new TreeCommand(command);
                _mostRecentChildCommand._parentCommand = this;
                _branches.Add(_mostRecentChildCommand);
            }
            else
            {
                _mostRecentChildCommand = tmp;
            }
        }

        public IEnumerable<TreeCommand> Branches
        {
            get
            {
                foreach (TreeCommand cursor in _branches)
                    yield return cursor;
            }
        }
    }
}
