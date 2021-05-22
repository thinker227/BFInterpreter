namespace BFInterpreter.Parsers {
	internal class IncrementPointerParser : ISymbolParser {

		public char GetSymbol() => '>';

		public void Parse(Interpreter interpreter) => interpreter.Program.IncrementPointer();

	}
}
