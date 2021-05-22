namespace BFInterpreter.Parsers {
	public class IncrementMemoryParser : ISymbolParser {

		public char GetSymbol() => '+';

		public void Parse(Interpreter interpreter) => interpreter.Program.IncrementCurrentMemory();

	}
}
