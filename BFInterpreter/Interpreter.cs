using System;
using System.Collections.Generic;
using System.Threading;
using BFInterpreter.Parsers;

namespace BFInterpreter {
	/// <summary>
	/// Represents an interpreter for a <see cref="BFProgram"/>.
	/// </summary>
	public sealed class Interpreter {

		private int instructionPointer;
		private string commandString;
		private bool isRunning;
		private readonly Dictionary<char, Type> parserTypes;
		private readonly Dictionary<Type, ISymbolParser> parserInstances;



		/// <summary>
		/// The <see cref="BFProgram"/> managed by this <see cref="Interpreter"/>.
		/// </summary>
		public BFProgram Program { get; }
		/// <summary>
		/// The configuration for the interpreter and program.
		/// </summary>
		public IInterpreterConfig Config { get; }
		/// <summary>
		/// The current location in <see cref="CommandString"/> execution is occuring.
		/// </summary>
		public int InstructionPointer {
			get => instructionPointer;
			set {
				if (!IsRunning) throw new InterpreterException(
					this, $"{nameof(InstructionPointer)} can only be set during execution."
				);
				instructionPointer = value;
			}
		}
		/// <summary>
		/// The current string of commands interpreted by this <see cref="Interpreter"/>.
		/// </summary>
		public string CommandString => commandString;
		/// <summary>
		/// Whether the interpreter is currently running or not.
		/// </summary>
		public bool IsRunning => isRunning;
		/// <summary>
		/// Event invoked when <see cref="CommandString"/> is changed.
		/// </summary>
		public event CommandStringChangedEventHandler OnCommandStringChanged;
		/// <summary>
		/// Event invoked at the end of a step of the program.
		/// </summary>
		public event InterpreterEventHandler OnProgramStep;
		/// <summary>
		/// Event invoked when the program exits.
		/// </summary>
		public event InterpreterEventHandler OnProgramExit;



		/// <summary>
		/// Initializes a new <see cref="Interpreter"/> instance.
		/// </summary>
		/// <param name="commandString">The string of command to be interpreted.</param>
		/// <param name="config">The configuration for the interpreter and program.</param>
		public Interpreter(IInterpreterConfig config) {
			instructionPointer = 0;
			commandString = string.Empty;
			isRunning = false;
			parserTypes = new();
			parserInstances = new();

			Program = new(config);
			Config = config;

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
		public Dictionary<int, int> GetBeginEndPairs(char begin, char end) {
			Dictionary<int, int> pairs = new();
			Stack<int> beginnings = new();

			for (int i = 0; i < CommandString.Length; i++) {
				char current = CommandString[i];

				if (current == begin) beginnings.Push(i);
				if (current == end) {
					if (beginnings.Count == 0) throw new InterpreterException(
						this, $"Inconsistent pair ending at position {i}."
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

			if (!parserTypes.TryAdd(symbol, type)) throw new InterpreterException(
				this, $"A parser with the symbol '{symbol}' is already registered."
			);
			if (!parserInstances.TryAdd(type, instance)) throw new InterpreterException(
				this, $"A parser of type '{type.FullName}' is already registered."
			);
		}
		private void RegisterDefaultSymbolParsers() {
			RegisterSymbolParser<IncrementPointerParser>();
			RegisterSymbolParser<DecrementPointerParser>();

			RegisterSymbolParser<IncrementMemoryParser>();
			RegisterSymbolParser<DecrementMemoryParser>();

			RegisterSymbolParser<InputParser>();
			RegisterSymbolParser<OutputParser>();

			LoopHandler loopHandler = new(this);
			RegisterSymbolParser(new BeginLoopParser(loopHandler));
			RegisterSymbolParser(new EndLoopParser(loopHandler));
		}
		/// <summary>
		/// Gets the registered <see cref="ISymbolParser"/> of a specified type.
		/// </summary>
		/// <typeparam name="T">The type of the <see cref="ISymbolParser"/> to get.</typeparam>
		/// <returns>The <see cref="ISymbolParser"/> registered in
		/// the interpreter of type <typeparamref name="T"/>.</returns>
		public ISymbolParser GetParser<T>() where T : ISymbolParser {
			Type type = typeof(T);

			bool success = parserInstances.TryGetValue(type, out ISymbolParser parser);

			if (success) return parser;
			throw new InterpreterException(
				this, $"No parser of type '{type.FullName}' is registered."
			);
		}
		/// <summary>
		/// Unregisters a previously registered symbol parser.
		/// </summary>
		/// <typeparam name="T">The <see cref="ISymbolParser"/> to unregister.</typeparam>
		public void UnregisterSymbolParser<T>() where T : ISymbolParser {
			Type type = typeof(T);

			if (!parserInstances.ContainsKey(type)) throw new InterpreterException(
				this, $"No parser of type '{type.FullName}' is registered."
			);

			char symbol = parserInstances[type].Symbol;

			parserTypes.Remove(symbol);
			parserInstances.Remove(type);
		}

		/// <summary>
		/// Runs the interpreter.
		/// </summary>
		/// <exception cref="InterpreterException">Thrown when an internal exception is thrown,
		/// including the current interpreter.</exception>
		public void Run(string commandString) {
			instructionPointer = 0;
			SetCommandString(commandString);
			isRunning = true;

			while (InstructionPointer < CommandString.Length) {
				OnProgramStep?.Invoke(this);

				bool success;
				do {
					char current = CommandString[InstructionPointer];
					success = ParseSymbol(current);

					InstructionPointer++;
				} while (!success && InstructionPointer < CommandString.Length);

				Thread.Sleep(Config.StepDelay);
			}

			OnProgramStep?.Invoke(this);
			isRunning = false;
			SetCommandString(string.Empty);
			OnProgramExit?.Invoke(this);
		}
		private bool ParseSymbol(char symbol) {
			if (
				parserTypes.TryGetValue(symbol, out Type type) &&
				parserInstances.TryGetValue(type, out ISymbolParser parser)
			) {
				try {
					parser.Parse(this);
				} catch (Exception e) {
					SetCommandString(string.Empty);
					isRunning = false;

					throw new InterpreterException(
						this, $"Parser error in '{parser.GetType().FullName}'", e
					);
				}

				return true;
			}
			
			return false;
		}
		private void SetCommandString(string value) {
			if (value != commandString) {
				commandString = value;
				OnCommandStringChanged?.Invoke(this, commandString);
			}
		}

	}
}
