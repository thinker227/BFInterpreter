namespace BFInterpreter.Parsers {
	public class InputParser : ISymbolParser {

		public char Symbol => ',';

		public void Parse(Interpreter interpreter) => interpreter.Program.InputCurrentMemory();

	}
}
