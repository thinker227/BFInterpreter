namespace BFInterpreter.Parsers {
	internal class DecrementPointerParser : ISymbolParser {

		public char GetSymbol() => '<';

		public void Parse(Interpreter interpreter) => interpreter.Program.DecrementPointer();

	}
}
