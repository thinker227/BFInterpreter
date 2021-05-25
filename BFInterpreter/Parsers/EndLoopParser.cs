namespace BFInterpreter.Parsers {
	public sealed class EndLoopParser : ISymbolParser {

		private readonly LoopHandler handler;



		public char Symbol => ']';



		internal EndLoopParser(LoopHandler handler) => this.handler = handler;



		public void Parse(Interpreter interpreter) {
			interpreter.InstructionPointer = handler.GetLoopEntry();
		}

	}
}
