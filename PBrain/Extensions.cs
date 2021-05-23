using BFInterpreter;
using PBrain.Parsers;

namespace PBrain {
	public static class Extensions {

		/// <summary>
		/// Registers the PBrain parsers in the interpreter.
		/// </summary>
		/// <param name="interpreter"></param>
		public static void RegisterPBrainParsers(this Interpreter interpreter) {
			FunctionHandler handler = new(interpreter);

			interpreter.RegisterSymbolParser(new BeginFunctionParser(handler));
			interpreter.RegisterSymbolParser(new EndFunctionParser(handler));
			interpreter.RegisterSymbolParser(new ExecuteFunctionParser(handler));
		}

	}
}
