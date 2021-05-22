namespace BFInterpreter.Parsers {
	public class IncrementMemoryParser : ISymbolParser {

		public char Symbol => '+';

		public void Parse(Interpreter interpreter) => interpreter.Program.IncrementCurrentMemory();

	}
}
