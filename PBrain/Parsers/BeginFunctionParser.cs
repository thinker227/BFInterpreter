using BFInterpreter;
using BFInterpreter.Parsers;

namespace PBrain.Parsers {
	public sealed class BeginFunctionParser : ISymbolParser {

		public char Symbol => '(';
		internal FunctionHandler Handler { get; }



		internal BeginFunctionParser(FunctionHandler handler) => Handler = handler;



		public void Parse(Interpreter interpreter) {
			Handler.DefineFunction();
			Handler.SkipToFunctionEnd();
		}

	}
}
