using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fe
{
    public class CommandBucket
    {      
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

        public Command AddCommand(ulong key)
        {
            //if (CommandCount >= Size)
            //{
            //    //TODO: Debug logging.
            //    return null; // Oops can't add a command when we have more than we support.
            //}

            // Increase total count of commands we have for next frame.            
            int currentCommand = Interlocked.Increment(ref _commandCount) - 1;

            // Store the key
            this._nextKeys[currentCommand] = key;
            
            // Store the command
            var command = this._nextCommands[currentCommand];
            command.ViewId = ViewId;
            
            return command;
        }

        public void Sort()
        {
            Utils.RadixSort64<Command>(this._nextKeys, this._nextCommands, this.CommandCount);
        }

        public int Submit(ref Command[] commands, int start)
        {
            // Add next commands into main command list.
            Array.Copy(_nextCommands, commands, this._commandCount);

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

        public uint Size{ get; set; }

        public int CommandCount { get { return _commandCount; } }

        public byte ViewId { get; set; }

        private int _commandCount;
        private ulong[] _nextKeys;
        private ulong[] _currentKeys;
        private Command[] _nextCommands;
        private Command[] _currentCommands;
    }
}
