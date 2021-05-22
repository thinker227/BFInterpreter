using System;
using System.Collections.Generic;
using BFInterpreter.Parsers;

namespace BFInterpreter {
	public class Interpreter {

		private readonly Dictionary<char, ISymbolParser> symbolParsers;



		public BFProgram Program { get; }
		public string CommandString { get; }



		public Interpreter(string commandString, int memorySize) {
			symbolParsers = new();
			Program = new(memorySize);
			CommandString = commandString;

			RegisterDefaultSymbolParsers();
		}

		public Interpreter(string commandString) : this(commandString, BFProgram.DefaultMemorySize) { }



		public void RegisterSymbolParser<T>() where T : class, ISymbolParser, new() {
			ISymbolParser instance = Activator.CreateInstance<T>();
			char symbol = instance.GetSymbol();
			bool success = symbolParsers.TryAdd(symbol, instance);

			if (!success) throw new Exception(
				$"A parser of type '{typeof(T).FullName}' is already registered."
			);
		}

		private void RegisterDefaultSymbolParsers() {
			RegisterSymbolParser<IncrementPointerParser>();
			RegisterSymbolParser<DecrementPointerParser>();

			RegisterSymbolParser<IncrementMemoryParser>();
			RegisterSymbolParser<DecrementMemoryParser>();
		}

		public void Run() {
			
		}

	}
}
