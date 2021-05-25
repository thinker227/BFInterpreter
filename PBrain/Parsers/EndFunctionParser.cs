using BFInterpreter;
using BFInterpreter.Parsers;

namespace PBrain.Parsers {
	public sealed class EndFunctionParser : ISymbolParser {

		private readonly FunctionHandler handler;



		public char Symbol => ')';



		internal EndFunctionParser(FunctionHandler handler) => this.handler = handler;



		public void Parse(Interpreter interpreter) {
			interpreter.InstructionPointer = handler.GetReturnPosition();
		}

	}
}
