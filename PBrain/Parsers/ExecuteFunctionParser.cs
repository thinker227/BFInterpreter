using BFInterpreter;
using BFInterpreter.Parsers;

namespace PBrain.Parsers {
	public sealed class ExecuteFunctionParser : ISymbolParser {

		private readonly FunctionHandler handler;



		public char Symbol => ':';



		internal ExecuteFunctionParser(FunctionHandler handler) => this.handler = handler;



		public void Parse(Interpreter interpreter) => handler.GetFunctionEntry();

	}
}
