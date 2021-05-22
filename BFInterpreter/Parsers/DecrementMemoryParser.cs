namespace BFInterpreter.Parsers {
	public class DecrementMemoryParser : ISymbolParser {

		public char Symbol => '-';

		public void Parse(Interpreter interpreter) => interpreter.Program.DecrementCurrentMemory();

	}
}
