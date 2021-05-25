namespace BFInterpreter.Parsers {
	public sealed class BeginLoopParser : ISymbolParser {

		private readonly LoopHandler handler;



		public char Symbol => '[';



		internal BeginLoopParser(LoopHandler handler) => this.handler = handler;



		public void Parse(Interpreter interpreter) {
			if (interpreter.Program.GetCurrentMemory() == 0) {
				interpreter.InstructionPointer = handler.GetLoopExit();
			}
		}

	}
}
