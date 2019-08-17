namespace StockBot
{
    using System.Collections.Generic;

    public interface IValidCommandsCatalog
    {
        bool IsValidCommand(string commandKey);

        IEnumerable<string> GetValidCommands();
    }
}