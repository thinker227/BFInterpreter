using System;

namespace BFInterpreter.Parsers {
	public sealed class EndLoopParser : ISymbolParser {

		public char Symbol => ']';
		internal LoopHandler Handler { get; }



		internal EndLoopParser(LoopHandler handler) => Handler = handler;



		public void Parse(Interpreter interpreter) {
			Handler.Interpreter.InstructionPointer = Handler.GetLoopEntry();
		}

	}
}
