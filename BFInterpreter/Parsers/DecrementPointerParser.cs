namespace BFInterpreter.Parsers {
	public class DecrementPointerParser : ISymbolParser {

		public char Symbol => '<';

		public void Parse(Interpreter interpreter) => interpreter.Program.DecrementPointer();

	}
}
