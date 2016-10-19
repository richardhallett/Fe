using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fe
{
    /// <summary>
    /// Represents a sortable collection of <see cref="Command"/>s
    /// </summary>
    public class CommandBucket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBucket"/> class.
        /// </summary>
        /// <param name="size">How many commands it can store</param>
        /// <param name="viewId">Which view this command bucket will be rendered on.</param>
        public CommandBucket(uint size, byte viewId = 0)
        {
            this.Size = size;            
            this.ViewId = viewId;

            this._commandCount = 0;

            this._nextKeys = new ulong[size];
            this._currentKeys = new ulong[size];
            this._nextCommands = new Command[size];
            this._currentCommands = new Command[size];
            
            for (uint i = 0; i < size; i++)
            {
                this._nextCommands[i] = new Command();
                this._currentCommands[i] = new Command();
            }
        }

        /// <summary>
        /// Add a command to the bucket with specified sort key
        /// </summary>
        /// <param name="sortKey">Key used for sorting</param>
        /// <returns></returns>
        public Command AddCommand(ulong sortKey)
        {
            if (CommandCount >= Size)
            {
                //TODO: Debug logging.
                return null; // Oops can't add a command when we have more than we support.
            }

            // Increase total count of commands we have for next frame.            
            int currentCommand = Interlocked.Increment(ref _commandCount) - 1;

            // Store the key
            this._nextKeys[currentCommand] = sortKey;
            
            // Store the command
            var command = this._nextCommands[currentCommand];

            // Reset the data in a command for reuse
            command.Reset();
            
            // View is the one the bucket belongs to
            command.ViewId = ViewId;
            
            return command;
        }

        /// <summary>
        /// Sorts the commands in the bucket based upon the keys assigned.
        /// </summary>
        internal void Sort()
        {
            Utils.RadixSort64<Command>(this._nextKeys, this._nextCommands, this.CommandCount);
        }

        /// <summary>
        /// Submits all commands in the bucket to passed in commands array.
        /// </summary>
        /// <param name="commands">Command array that we're submitting to.</param>
        /// <param name="start">Offset into the command list we want to start adding from.</param>
        /// <returns></returns>
        internal int Submit(ref Command[] commands, int start)
        {
            // Add next commands into main command list.
            Array.Copy(_nextCommands, 0, commands, start, this._commandCount);

            // Swap command buffers
            var tempCommands = _currentCommands;
            var tempKeys = _currentKeys;
            _currentCommands = _nextCommands;
            _currentKeys = _nextKeys;
            _nextCommands = tempCommands;
            _nextKeys = tempKeys;            

            try
            {
                return this._commandCount;
            }
            finally
            {
                this._commandCount = 0;
            }
        }

        /// <summary>
        /// Size of the command bucket
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public uint Size{ get; private set; }

        /// <summary>
        /// Number of commands currently on the bucket.
        /// </summary>
        /// <value>
        /// The command count.
        /// </value>
        public int CommandCount { get { return _commandCount; } }

        /// <summary>
        /// View the bucket will render on
        /// </summary>
        /// <value>
        /// The view identifier.
        /// </value>
        public byte ViewId { get; private set; }

        private int _commandCount;
        private ulong[] _nextKeys;
        private ulong[] _currentKeys;
        private Command[] _nextCommands;
        private Command[] _currentCommands;
    }
}
