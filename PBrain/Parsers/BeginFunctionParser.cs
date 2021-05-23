using BFInterpreter;
using BFInterpreter.Parsers;

namespace PBrain.Parsers {
	public sealed class BeginFunctionParser : ISymbolParser {

		public char Symbol => '(';
		internal FunctionHandler Handler { get; }



		internal BeginFunctionParser(FunctionHandler handler) => Handler = handler;



		public void Parse(Interpreter interpreter) {
			Handler.DefineFunction();
			interpreter.InstructionPointer = GetFunctionExit(interpreter);
		}

		private static int GetFunctionExit(Interpreter interpreter) {
			int depth = 0;

			for (int i = interpreter.InstructionPointer + 1; i < interpreter.CommandString.Length; i++) {
				char current = interpreter.CommandString[i];

				if (depth == 0 && current == ')') return i;
				if (current == '(') depth++;
				if (current == ')') depth--;
			}

			throw new BFException(
				interpreter, $"Malformed function at position {interpreter.InstructionPointer}."
			);
		}

	}
}
