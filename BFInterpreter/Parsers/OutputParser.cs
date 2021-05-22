namespace BFInterpreter.Parsers {
	public class OutputParser : ISymbolParser {

		public char Symbol => '.';

		public void Parse(Interpreter interpreter) => interpreter.Program.WriteOutput();

	}
}
