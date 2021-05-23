using BFInterpreter;
using BFInterpreter.Parsers;

namespace PBrain.Parsers {
	public class ExecuteFunctionParser : ISymbolParser {

		public char Symbol => ':';

		public void Parse(Interpreter interpreter) => throw new System.NotImplementedException();

	}
}
