using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BFInterpreter {
	internal sealed class LoopHandler {

		private readonly Dictionary<int, int> beginEndPairs;



		public Interpreter Interpreter { get; }



		public LoopHandler(Interpreter interpreter) {
			Interpreter = interpreter;
			beginEndPairs = Interpreter.GetBeginEndPairs(Interpreter.CommandString, '[', ']');
		}



		public void JumpToLoopEnd() {
			Interpreter.InstructionPointer = beginEndPairs[Interpreter.InstructionPointer];
		}

		public void JumpToLoopBegin() {
			// Jumps to the position before the loop opening
			Interpreter.InstructionPointer = beginEndPairs[Interpreter.InstructionPointer] - 1;
		}

	}
}
