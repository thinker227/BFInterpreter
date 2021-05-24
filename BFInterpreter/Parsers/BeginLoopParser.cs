namespace BFInterpreter.Parsers {
	public sealed class BeginLoopParser : ISymbolParser {

		public char Symbol => '[';
		internal LoopHandler Handler { get; }



		internal BeginLoopParser(LoopHandler handler) => Handler = handler;



		public void Parse(Interpreter interpreter) {
			if (interpreter.Program.GetCurrentMemory() == 0) {
				Handler.Interpreter.InstructionPointer = Handler.GetLoopExit();
			}
		}

	}
}
