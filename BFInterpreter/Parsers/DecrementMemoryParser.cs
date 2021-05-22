namespace BFInterpreter.Parsers {
	public class DecrementMemoryParser : ISymbolParser {

		public char GetSymbol() => '-';

		public void Parse(Interpreter interpreter) => interpreter.Program.DecrementCurrentMemory();

	}
}
