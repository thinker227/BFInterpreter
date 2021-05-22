namespace BFInterpreter.Parsers {
	public sealed class DecrementMemoryParser : ISymbolParser {

		public char Symbol => '-';

		public void Parse(Interpreter interpreter) => interpreter.Program.DecrementCurrentMemory();

	}
}
