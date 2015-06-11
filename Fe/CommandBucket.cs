using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fe
{
    public class CommandBucket : ICommandBucket
    {
        public CommandBucket(uint size, byte viewId = 0)
        {
            this.Size = size;            
            this.ViewId = viewId;

            this._commandCount = 0;
            //this._keys = new TKey[size];
            this._keys = new ulong[size];
            this._commands = new Command[size];
            
            for (uint i = 0; i < size; i++)
            {
                this._commands[i] = new Command();
            }
            //this._keys = Enumerable.Repeat<T>(T.MaxValue, count).ToArray();
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
            this._keys[currentCommand] = key;
            
            // Store the command
            var command = this._commands[currentCommand];
            command.viewId = ViewId;
            
            return command;
        }

        public void Sort()
        {
            Utils.RadixSort64<Command>(this._keys, this._commands, this.CommandCount);
        }

        public int Submit(ref Command[] commands, int start)
        {
            for (int i = 0; i < this._commandCount; i++)
            {
                int newIndex = start + i;
                commands[newIndex].IndexBuffer = _commands[i].IndexBuffer;
                commands[newIndex].VertexBuffer = _commands[i].VertexBuffer;
                commands[newIndex].ShaderProgram = _commands[i].ShaderProgram;
                commands[newIndex].SharedUniforms = _commands[i].SharedUniforms;
                commands[newIndex].Transform = _commands[i].Transform;
                commands[newIndex].viewId = _commands[i].viewId;
            }
            
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

        public Command[] Commands {
            get {
                return this._commands;
            }
        }

        private int _commandCount;
        private ulong[] _keys;
        private Command[] _commands;
    }
}
