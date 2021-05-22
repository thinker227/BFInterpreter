namespace BFInterpreter.Parsers {
	public sealed class DecrementPointerParser : ISymbolParser {

		public char Symbol => '<';

		public void Parse(Interpreter interpreter) => interpreter.Program.DecrementPointer();

	}
}
