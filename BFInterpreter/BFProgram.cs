using System;
using System.Collections.Generic;
using System.Linq;

namespace BFInterpreter {
	/// <summary>
	/// Represents a BF program.
	/// </summary>
	public sealed class BFProgram {

		/// <summary>
		/// The default memory size.
		/// </summary>
		public const int DefaultMemorySize = 30000;
		private readonly byte[] memory;
		private int memoryPointer;



		/// <summary>
		/// The current position of the memory pointer.
		/// </summary>
		public int MemoryPointer => memoryPointer;
		/// <summary>
		/// The size of the program memory.
		/// </summary>
		public int MemorySize { get; }
		/// <summary>
		/// The current memory pointed to by the memory pointer.
		/// </summary>
		public byte CurrentMemory => memory[MemoryPointer];
		/// <summary>
		/// The configuration for the program.
		/// </summary>
		public IProgramConfig Config { get; }



		/// <summary>
		/// Initializes a new <see cref="BFProgram"/>.
		/// </summary>
		/// <param name="config">The configuration for the program.</param>
		public BFProgram(IProgramConfig config) {
			if (config.MemorySize <= 0) throw new ArgumentException(
				"Memory size can't be less or equal to 0.", nameof(config)
			);
			
			Config = config;
			memory = new byte[Config.MemorySize];
			memoryPointer = 0;
			MemorySize = Config.MemorySize;
		}



		private void SetPointer(int position) {
			memoryPointer = position;
			if (memoryPointer < 0) memoryPointer = MemorySize - 1;
			if (memoryPointer >= MemorySize) memoryPointer = 0;
		}
		/// <summary>
		/// Increments the position of the memory pointer.
		/// </summary>
		public void IncrementPointer() => SetPointer(MemoryPointer + 1);
		/// <summary>
		/// Decrements the position of the memory pointer.
		/// </summary>
		public void DecrementPointer() => SetPointer(MemoryPointer - 1);

		/// <summary>
		/// Sets the current memory pointed to by the memory pointer to an
		/// inputted value from the program's specified <see cref="IInput"/>.
		/// </summary>
		public void InputCurrentMemory() => memory[MemoryPointer] = Config.Input.GetInput();
		private void SetCurrentMemory(bool increment) {
			int value = increment ? memory[MemoryPointer] + 1 : memory[MemoryPointer] - 1;

			switch (Config.MemoryOverflowBehavior) {
				case OverflowBehavior.Wrap: {
					if (value < byte.MinValue) value = byte.MaxValue;
					if (value > byte.MaxValue) value = byte.MinValue;
				} break;

				case OverflowBehavior.Freeze: {
					if (value < byte.MinValue) value = byte.MinValue;
					if (value > byte.MaxValue) value = byte.MaxValue;
				} break;

				case OverflowBehavior.Throw: {
					if (value < byte.MinValue || value > byte.MaxValue) throw new ArithmeticException(
						"An over/underflow occured."
					);
				} break;
			}

			memory[MemoryPointer] = (byte)value;
		}
		/// <summary>
		/// Increments the value of the current memory pointed to by the memory pointer.
		/// </summary>
		public void IncrementCurrentMemory() => SetCurrentMemory(true);
		/// <summary>
		/// Decrements the value of the current memory pointed to by the memory pointer.
		/// </summary>
		public void DecrementCurrentMemory() => SetCurrentMemory(false);
		/// <summary>
		/// Gets the <see cref="IEnumerator{T}"/> of the memory.
		/// </summary>
		/// <returns>The <see cref="IEnumerator{T}"/> of the program's memory.</returns>
		public IEnumerator<byte> GetMemoryEnumerator() => memory.AsEnumerable().GetEnumerator();

		/// <summary>
		/// Writes the current memory pointed to by the memory pointer
		/// as output using the program's specified <see cref="IOutput"/>.
		/// </summary>
		public void WriteOutput() => Config.Output.WriteOutput(memory[MemoryPointer]);

	}
}
