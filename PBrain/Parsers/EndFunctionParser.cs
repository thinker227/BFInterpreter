using BFInterpreter;
using BFInterpreter.Parsers;

namespace PBrain.Parsers {
	public sealed class EndFunctionParser : ISymbolParser {

		public char Symbol => ')';
		internal FunctionHandler Handler { get; }



		internal EndFunctionParser(FunctionHandler handler) => Handler = handler;



		public void Parse(Interpreter interpreter) => Handler.ExitFunction();

	}
}
