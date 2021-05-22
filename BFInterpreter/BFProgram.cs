using System;
using System.Collections.Generic;
using System.Linq;

namespace BFInterpreter {
	public class BFProgram {

		public event ProgramExitEventHandler OnProgramExit;



		public const int DefaultMemorySize = 30000;
		private readonly byte[] memory;
		private int memoryPointer;



		public int MemoryPointer => memoryPointer;
		public int MemorySize { get; }



		public BFProgram(int memorySize) {
			if (memorySize <= 0) throw new ArgumentException(
				"Memory size can't be less or equal to 0.", nameof(memorySize)
			);

			memory = new byte[memorySize];
			MemorySize = memorySize;
			memoryPointer = 0;
		}



		private void PointerSet(int position) {
			memoryPointer = position;
			if (memoryPointer < 0) memoryPointer = MemorySize - 1;
			if (memoryPointer >= MemorySize) memoryPointer = 0;
		}
		public void PointerIncrement() => PointerSet(MemoryPointer + 1);
		public void PointerDecrement() => PointerSet(MemoryPointer - 1);

		public byte GetCurrentMemory() => memory[memoryPointer];
		public void SetCurrentMemory(byte value) => memory[memoryPointer] = value;
		public void CurrentMemoryIncrement() => memory[memoryPointer]++;
		public void CurrentMemoryDecrement() => memory[memoryPointer]--;

		public IEnumerator<byte> GetMemoryEnumerator() => memory.AsEnumerable().GetEnumerator();

	}
}
