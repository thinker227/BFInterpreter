namespace BFInterpreter.Parsers {
	/// <summary>
	/// Represents a BF symbol parser.
	/// </summary>
	public interface ISymbolParser {

		/// <summary>
		/// Gets the symbol represented by this <see cref="ISymbolParser"/>.
		/// </summary>
		/// <returns>The symbol corresponding to the action of this <see cref="ISymbolParser"/>.</returns>
		public char GetSymbol();
		/// <summary>
		/// Parses a symbol and performs a corresponding action.
		/// </summary>
		/// <param name="interpreter">The <see cref="Interpreter"/> which is handling the current symbol.</param>
		public void Parse(Interpreter interpreter);

	}
}
