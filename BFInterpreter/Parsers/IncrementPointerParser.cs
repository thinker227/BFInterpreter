namespace BFInterpreter.Parsers {
	public class IncrementPointerParser : ISymbolParser {

		public char Symbol => '>';

		public void Parse(Interpreter interpreter) => interpreter.Program.IncrementPointer();

	}
}
