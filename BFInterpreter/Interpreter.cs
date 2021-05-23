using System;
using System.Collections.Generic;
using BFInterpreter.Parsers;

namespace BFInterpreter {
	/// <summary>
	/// Represents an interpreter for a <see cref="BFProgram"/>.
	/// </summary>
	public sealed class Interpreter {

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
		/// The current location in <see cref="CommandString"/> execution is occuring.
		/// </summary>
		public int InstructionPointer { get; set; }
		/// <summary>
		/// Event invoked when the program exits.
		/// </summary>
		public event ProgramExitEventHandler OnProgramExit;



		/// <summary>
		/// Initializes a new <see cref="Interpreter"/> instance.
		/// </summary>
		/// <param name="commandString">The string of command to be interpreted.</param>
		/// <param name="memorySize">The size of the program's memory.</param>
		/// <param name="input">The <see cref="IInput"/> to use for getting input to the program.</param>
		/// <param name="output">The <see cref="IOutput"/> to use writing output from the program.</param>
		public Interpreter(string commandString, int memorySize, IInput input, IOutput output) {
			symbolParsers = new();
			InstructionPointer = 0;
			Program = new(memorySize, input, output);
			CommandString = commandString;

			RegisterDefaultSymbolParsers();
		}

		/// <summary>
		/// Initializes a new <see cref="Interpreter"/> instance.
		/// Memory size will be initialized to the default memory size.
		/// </summary>
		/// <param name="commandString">The string of command to be interpreted.</param>
		/// <param name="input">The <see cref="IInput"/> to use for getting input to the program.</param>
		/// <param name="output">The <see cref="IOutput"/> to use writing output from the program.</param>
		public Interpreter(string commandString, IInput input, IOutput output) :
			this(commandString, BFProgram.DefaultMemorySize, input, output) { }



		/// <summary>
		/// Registers an <see cref="ISymbolParser"/> to use for parsing a symbol.
		/// </summary>
		/// <typeparam name="T">The type of the <see cref="ISymbolParser"/> to register.</typeparam>
		public void RegisterSymbolParser<T>() where T : class, ISymbolParser, new() {
			ISymbolParser instance = Activator.CreateInstance<T>();
			char symbol = instance.Symbol;
			bool success = symbolParsers.TryAdd(symbol, instance);

			if (!success) throw new Exception(
				$"A parser with the symbol '{symbol}' is already registered."
			);
		}
		private void RegisterDefaultSymbolParsers() {
			RegisterSymbolParser<IncrementPointerParser>();
			RegisterSymbolParser<DecrementPointerParser>();

			RegisterSymbolParser<IncrementMemoryParser>();
			RegisterSymbolParser<DecrementMemoryParser>();

			RegisterSymbolParser<InputParser>();
			RegisterSymbolParser<OutputParser>();

			RegisterSymbolParser<BeginLoopParser>();
			RegisterSymbolParser<EndLoopParser>();
		}

		/// <summary>
		/// Runs the interpreter.
		/// </summary>
		public void Run() {
			while (true) {
				char current = CommandString[InstructionPointer];
				ParseSymbol(current);

				InstructionPointer++;
				if (InstructionPointer >= CommandString.Length) break;
			}

			OnProgramExit?.Invoke(0, "Success");
		}

		private void ParseSymbol(char symbol) {
			if (symbolParsers.TryGetValue(symbol, out ISymbolParser parser)) parser.Parse(this);
		}

	}
}
