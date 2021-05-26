﻿using System;
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
					throw new InterpreterException(
						this, $"Parser error in '{parser.GetType().FullName}'", e
					);
				}
			}
		}

	}
}
