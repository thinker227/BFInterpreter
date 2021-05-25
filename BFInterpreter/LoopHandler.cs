using System.Collections.Generic;
using BFInterpreter.Parsers;

namespace BFInterpreter {
	/// <summary>
	/// Represents a handler for BF loops.
	/// </summary>
	internal sealed class LoopHandler {

		private readonly Dictionary<int, int> beginEndPairs;



		public Interpreter Interpreter { get; }



		/// <summary>
		/// Initializes a new <see cref="LoopHandler"/> instance.
		/// </summary>
		public LoopHandler(Interpreter interpreter) {
			Interpreter = interpreter;
			
			beginEndPairs = Interpreter.GetBeginEndPairs(Interpreter.CommandString, '[', ']');
		}



		/// <summary>
		/// Gets the instruction pointer position corresponding to the exit point of the current loop entry.
		/// </summary>
		/// <returns>The exit point corresponding to the current instruction pointer position,
		/// assuming the current instruction pointer position is a loop entry point.</returns>
		public int GetLoopExit() {
			return beginEndPairs[Interpreter.InstructionPointer];
		}

		/// <summary>
		/// Gets the instruction pointer position corresponding to the entry point of the current loop exit.
		/// </summary>
		/// <returns>The entry point corresponding to the current instruction pointer position,
		/// assuming the current instruction pointer position is a loop exit point.</returns>
		public int GetLoopEntry() {
			return beginEndPairs[Interpreter.InstructionPointer] - 1;
		}

	}
}
