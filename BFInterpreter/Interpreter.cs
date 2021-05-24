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
		/// Utility method for getting the begin and end pairs of a sequence in a command string.
		/// </summary>
		/// <param name="commandString">The command string to get the begin and end pairs of.</param>
		/// <param name="begin">The character representing the beginning of the pair.</param>
		/// <param name="end">The character representing the end of the pair.</param>
		/// <returns>A dictionary containing the beginning and ending pairs for each instance of
		/// <paramref name="begin"/> and <paramref name="end"/> in <paramref name="commandString"/></returns>
		public static Dictionary<int, int> GetBeginEndPairs(string commandString, char begin, char end) {
			Dictionary<int, int> pairs = new();
			Stack<int> beginnings = new();

			for (int i = 0; i < commandString.Length; i++) {
				char current = commandString[i];

				if (current == end) beginnings.Push(i);
				if (current == begin) {
					if (beginnings.Count == 0) throw new Exception(
						$"Inconsistent pair ending at position {i}."
					);

					int currentBegin = beginnings.Pop();
					pairs.Add(currentBegin, i);
					pairs.Add(i, currentBegin);
				}
			}

			return pairs;
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
