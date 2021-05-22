namespace BFInterpreter.Parsers {
	public sealed class IncrementPointerParser : ISymbolParser {

		public char Symbol => '>';

		public void Parse(Interpreter interpreter) => interpreter.Program.IncrementPointer();

	}
}
