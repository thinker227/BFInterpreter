using BFInterpreter;
using BFInterpreter.Parsers;

namespace PBrain.Parsers {
	public sealed class ExecuteFunctionParser : ISymbolParser {

		public char Symbol => ':';
		internal FunctionHandler Handler { get; }



		internal ExecuteFunctionParser(FunctionHandler handler) => Handler = handler;



		public void Parse(Interpreter interpreter) => Handler.EnterFunction();

	}
}
