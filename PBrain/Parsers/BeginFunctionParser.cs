using BFInterpreter;
using BFInterpreter.Parsers;

namespace PBrain.Parsers {
	public class BeginFunctionParser : ISymbolParser {

		public char Symbol => '(';

		public void Parse(Interpreter interpreter) => throw new System.NotImplementedException();

	}
}
