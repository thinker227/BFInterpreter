using BFInterpreter;
using BFInterpreter.Parsers;

namespace PBrain.Parsers {
	public sealed class BeginFunctionParser : ISymbolParser {

		private readonly FunctionHandler handler;



		public char Symbol => '(';



		internal BeginFunctionParser(FunctionHandler handler) => this.handler = handler;



		public void Parse(Interpreter interpreter) {
			handler.DefineFunction();
			interpreter.InstructionPointer = handler.GetFunctionExit();
		}

	}
}
