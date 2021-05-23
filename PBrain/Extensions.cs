using BFInterpreter;
using PBrain.Parsers;

namespace PBrain {
	public static class Extensions {

		/// <summary>
		/// Registers the PBrain parsers in the interpreter.
		/// </summary>
		/// <param name="interpreter"></param>
		public static void RegisterPBrainParsers(this Interpreter interpreter) {
			interpreter.RegisterSymbolParser<BeginFunctionParser>();
			interpreter.RegisterSymbolParser<EndFunctionParser>();
			interpreter.RegisterSymbolParser<ExecuteFunctionParser>();
		}

	}
}
