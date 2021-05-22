using System;
using System.Collections.Generic;
using BFInterpreter.Parsers;

namespace BFInterpreter {
	public class Interpreter {
	/// <summary>
	/// Represents an interpreter for a <see cref="BFProgram"/>.
	/// </summary>

		private readonly Dictionary<char, ISymbolParser> symbolParsers;



		/// <summary>
		/// The <see cref="BFProgram"/> managed by this <see cref="Interpreter"/>.
		/// </summary>
		public BFProgram Program { get; }
		/// <summary>
		/// The current string of commands interpreted by this <see cref="Interpreter"/>.
		/// </summary>
		public string CommandString { get; }



		/// <summary>
		/// Initializes a new <see cref="Interpreter"/> instance.
		/// </summary>
		/// <param name="commandString">The string of command to be interpreted.</param>
		/// <param name="memorySize">The size of the program's memory.</param>
		public Interpreter(string commandString, int memorySize) {
			symbolParsers = new();
			Program = new(memorySize);
			CommandString = commandString;

			RegisterDefaultSymbolParsers();
		}

		/// <summary>
		/// Initializes a new <see cref="Interpreter"/> instance.
		/// Memory size will be initialized to the default memory size.
		/// </summary>
		/// <param name="commandString">The string of command to be interpreted.</param>
		public Interpreter(string commandString) : this(commandString, BFProgram.DefaultMemorySize) { }



		/// <summary>
		/// Registers an <see cref="ISymbolParser"/> to use for parsing a symbol.
		/// </summary>
		/// <typeparam name="T">The type of the <see cref="ISymbolParser"/> to register.</typeparam>
		public void RegisterSymbolParser<T>() where T : class, ISymbolParser, new() {
			ISymbolParser instance = Activator.CreateInstance<T>();
			char symbol = instance.Symbol;
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

		/// <summary>
		/// Runs the interpreter.
		/// </summary>
		public void Run() {
			
		}

	}
}
