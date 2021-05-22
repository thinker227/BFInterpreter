namespace BFInterpreter.Parsers {
	public sealed class IncrementMemoryParser : ISymbolParser {

		public char Symbol => '+';

		public void Parse(Interpreter interpreter) => interpreter.Program.IncrementCurrentMemory();

	}
}
