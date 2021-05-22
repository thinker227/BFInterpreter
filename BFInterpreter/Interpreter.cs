using System;

namespace BFInterpreter {
	public class Interpreter {

		private readonly string commandString;



		public BFProgram Program { get; }



		public Interpreter(string commandString, int memorySize) {
			Program = new(memorySize);
			this.commandString = commandString;
		}

		public Interpreter(string commandString) : this(commandString, BFProgram.DefaultMemorySize) { }





	}
}
