namespace StockBot
{
    using System.Collections.Generic;
    using System.Linq;

    public class ValidCommandsCatalog : IValidCommandsCatalog
    {
        private readonly string[] validCommands = { "stock" };

        public bool IsValidCommand(string commandKey) => this.validCommands.Contains(commandKey);

        public IEnumerable<string> GetValidCommands() => this.validCommands;
    }
}
