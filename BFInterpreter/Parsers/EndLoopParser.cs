using System;

namespace BFInterpreter.Parsers {
	public sealed class EndLoopParser : ISymbolParser {

		public char Symbol => ']';



		public void Parse(Interpreter interpreter) {
			if (!interpreter.Program.CurrentMemoryIsZero()) {
				interpreter.InstructionPointer = GetLoopStart(interpreter);
			}
		}

		private int GetLoopStart(Interpreter interpreter) {
			int loopDepth = 0;
			for (int i = interpreter.InstructionPointer - 1; i >= 0; i--) {
				char current = interpreter.CommandString[i];

				if (loopDepth == 0 && current == '[') return i;
				if (current == ']') loopDepth++;
				if (current == '[') loopDepth--;
			}

			throw new Exception($"Malformed loop ending at position {interpreter.InstructionPointer}.");
		}

	}
}
