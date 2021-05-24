using System;
using System.Collections.Generic;
using BFInterpreter.Parsers;

namespace BFInterpreter {
	/// <summary>
	/// Represents an interpreter for a <see cref="BFProgram"/>.
	/// </summary>
	public sealed class Interpreter {

		private readonly Dictionary<char, Type> parserTypes;
		private readonly Dictionary<Type, ISymbolParser> parserInstances;



		/// <summary>
		/// The <see cref="BFProgram"/> managed by this <see cref="Interpreter"/>.
		/// </summary>
		public BFProgram Program { get; }
		/// <summary>
		/// The current string of commands interpreted by this <see cref="Interpreter"/>.
		/// </summary>
		public string CommandString { get; }
		/// <summary>
		/// The configuration for the interpreter and program.
		/// </summary>
		public IInterpreterConfig Config { get; }
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
		/// <param name="config">The configuration for the interpreter and program.</param>
		public Interpreter(string commandString, IInterpreterConfig config) {
			parserTypes = new();
			parserInstances = new();
			InstructionPointer = 0;
			Program = new(config);
			CommandString = commandString;

			RegisterDefaultSymbolParsers();
		}



		/// <summary>
		/// Creates an instance of the specified <see cref="ISymbolParser"/> type and registers it as a symbol parser.
		/// </summary>
		/// <typeparam name="T">The type of the <see cref="ISymbolParser"/> to register.</typeparam>
		public void RegisterSymbolParser<T>() where T : class, ISymbolParser, new() {
			ISymbolParser instance = Activator.CreateInstance<T>();
			RegisterSymbolParser(instance);
		}
		/// <summary>
		/// Registers an <see cref="ISymbolParser"/> as a symbol parser.
		/// </summary>
		/// <param name="instance"></param>
		public void RegisterSymbolParser(ISymbolParser instance) {
			char symbol = instance.Symbol;
			Type type = instance.GetType();

			if (!parserTypes.TryAdd(symbol, type)) throw new Exception(
				$"A parser with the symbol '{symbol}' is already registered."
			);
			if (!parserInstances.TryAdd(type, instance)) throw new Exception(
				$"A parser of type '{type.FullName}' is already registered."
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
		/// <exception cref="BFException">Thrown when an internal exception is thrown,
		/// including the current interpreter.</exception>
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
			if (
				parserTypes.TryGetValue(symbol, out Type type) &&
				parserInstances.TryGetValue(type, out ISymbolParser parser)
			) {
				try {
					parser.Parse(this);
				} catch (Exception e) {
					throw new BFException(this, e);
				}
			}
		}

	}
}
