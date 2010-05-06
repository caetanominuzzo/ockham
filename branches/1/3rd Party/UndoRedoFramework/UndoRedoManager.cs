// Siarhei Arkhipenka (c) 2006-2007. email: sbs-arhipenko@yandex.ru
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Linq;

namespace UndoRedoFramework
{
    public static class UndoRedoManager
    {
        private static TreeCommand currentPosition = null;

        private static Command currentCommand = null;

        internal static Command CurrentCommand
        {
            get { return currentCommand; }
        }

        /// <summary>
        /// Returns true if history has command that can be undone
        /// </summary>
        public static bool CanUndo
        {
            get { return currentPosition.Command != null; }
        }

        /// <summary>
        /// Returns true if history has command that can be redone
        /// </summary>
        public static bool CanRedo
        {
            get { return currentPosition != null && currentPosition.MostRecentChildCommand != null; }
        }

        /// <summary>
        /// Undo last command from history list
        /// </summary>
        public static void Undo()
        {
            AssertNoCommand();
            if (CanUndo)
            {
                Command command = currentPosition.Command;

                foreach (IUndoRedoMember member in command.Keys)
                {
                    member.OnUndo(command[member]);
                }
                currentPosition = currentPosition.ParentCommand;

                OnCommandDone(CommandDoneType.Undo);
            }
        }

        /// <summary>
        /// Redo last command undone.
        /// </summary>
        public static void Redo()
        {
            Redo(currentPosition.MostRecentChildCommand);
        }

        /// <summary>
        /// Redo to a specific command branching from the current state.
        /// </summary>
        /// <param name="Node">Target branch command caption.</param>
        public static void Redo(string commandCaption)
        {
            Redo(GetBranch(commandCaption));
        }

        private static void Redo(TreeCommand Node)
        {
            AssertNoCommand();
            if (CanRedo)
            {
                currentPosition.AddChild(Node.Command);

                currentPosition = Node;

                Command command = currentPosition.Command;

                foreach (IUndoRedoMember member in command.Keys)
                {
                    member.OnRedo(command[member]);
                }
                OnCommandDone(CommandDoneType.Redo);
            }
        }

        /// <summary>
        /// Start command. Any data changes must be done within a command.
        /// </summary>
        /// <param name="commandCaption"></param>
        /// <returns></returns>
        public static IDisposable Start(string commandCaption)
        {
            AssertNoCommand();
            currentCommand = new Command(commandCaption);
            return currentCommand;
        }

        /// <summary>
        /// Commits current command and saves changes into history
        /// </summary>
        public static void Commit()
        {
            AssertCommand();
            foreach (IUndoRedoMember member in currentCommand.Keys)
                member.OnCommit(currentCommand[member]);

            //Creates initial position. A TreeNode without command.
            if (currentPosition == null)
                currentPosition = new TreeCommand();

            //Creates a new child node in the current position.
            currentPosition.AddChild(currentCommand);

            //Increases current position.
            currentPosition = currentPosition.MostRecentChildCommand;

            currentCommand = null;
            OnCommandDone(CommandDoneType.Commit);
        }

        /// <summary>
        /// Rollback current command. It does not saves any changes done in current command.
        /// </summary>
        public static void Cancel()
        {
            AssertCommand();
            foreach (IUndoRedoMember member in currentCommand.Keys)
                member.OnUndo(currentCommand[member]);
            currentCommand = null;
        }

        /// <summary>
        /// Clears all history. It does not affect current data but history only. 
        /// It is usefull after any data initialization if you want forbid user to undo this initialization.
        /// </summary>
        public static void FlushHistory()
        {
            currentCommand = null;
            currentPosition = null;
        }

        /// <summary>Checks there is no command started</summary>
        internal static void AssertNoCommand()
        {
            if (currentCommand != null)
                throw new InvalidOperationException("Previous command is not completed. Use UndoRedoManager.Commit() to complete current command.");
        }

        /// <summary>Checks that command had been started</summary>
        internal static void AssertCommand()
        {
            if (currentCommand == null)
                throw new InvalidOperationException("Command is not started. Use method UndoRedoManager.Start().");
        }

        private static bool IsCommandStarted
        {
            get { return currentCommand != null; }
        }

        /// <summary>Gets an enumeration of commands captions that can be undone.</summary>
        /// <remarks>The first command in the enumeration will be undone first</remarks>
        public static IEnumerable<string> UndoCommands
        {
            get
            {
                TreeCommand cursor = currentPosition;

                while (true)
                {
                    if (cursor.Command == null)
                        break;

                    yield return cursor.Command.Caption;

                    if (cursor.ParentCommand == cursor)
                        break;

                    cursor = cursor.ParentCommand;
                }

                yield break;
            }
        }

        /// <summary>Gets an enumeration of commands captions that can be redone.</summary>
        /// <remarks>The first command in the enumeration will be redone first</remarks>
        public static IEnumerable<string> RedoCommands()
        {
            return RedoCommands(currentPosition.MostRecentChildCommand);
        }
        
        /// <summary>Gets an enumeration of commands captions that can be redone.</summary>
        /// <remarks>The first command in the enumeration will be redone first</remarks>
        public static IEnumerable<string> RedoCommands(string commandCaption)
        {
            return RedoCommands(GetBranch(commandCaption));
        }

        private static IEnumerable<string> RedoCommands(TreeCommand node)
        {
            TreeCommand cursor = node.MostRecentChildCommand;

            while (true)
            {
                if (cursor == null)
                    break;

                yield return cursor.Command.Caption;

                cursor = cursor.MostRecentChildCommand;


            }

            yield break;
        }

        public static IEnumerable<string> BranchCommands
        {
            get
            {
                if (currentPosition != null)
                {
                    if (currentPosition.MostRecentChildCommand != null)
                    {
                        //Returns MostRecentChild first
                        yield return currentPosition.MostRecentChildCommand.Command.Caption;

                        foreach (TreeCommand cursor in currentPosition.Branches)
                        {
                            if(cursor != currentPosition.MostRecentChildCommand)
                                yield return cursor.Command.Caption;
                        }
                    }
                }
                yield break;
            }
        }

        public static event EventHandler<CommandDoneEventArgs> CommandDone;

        static void OnCommandDone(CommandDoneType type)
        {
            if (CommandDone != null)
                CommandDone(null, new CommandDoneEventArgs(type));
        }

        private static int maxHistorySize = 0;

        /// <summary>
        /// Gets/sets max commands stored in history. 
        /// Zero value (default) sets unlimited history size.
        /// </summary>
        public static int MaxHistorySize
        {
            get { return maxHistorySize; }
            set
            {
                if (IsCommandStarted)
                    throw new InvalidOperationException("Max size may not be set while command is run.");
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Value may not be less than 0");
                maxHistorySize = value;
                TruncateHistory();
            }
        }

        private static void TruncateHistory()
        {

        }

        private static TreeCommand GetBranch(string commandCaption)
        {
            var v = (from b in currentPosition.Branches where b.Command.Caption == commandCaption select b);

            TreeCommand tmp = v.Count() == 1 ? v.First() : null;

            if (tmp != null)
                return tmp;
            else
                throw new InvalidOperationException("That branch doesn't exists.");
        }

    }

    public enum CommandDoneType
    {
        Commit, Undo, Redo
    }

    public class CommandDoneEventArgs : EventArgs
    {
        public readonly CommandDoneType CommandDoneType;
        public CommandDoneEventArgs(CommandDoneType type)
        {
            CommandDoneType = type;
        }
    }

}
