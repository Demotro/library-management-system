namespace Knihovna
{
    internal static class Program
    {
        /// <summary>
        /// Hlavní vstupní bod aplikace.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            //vytvori databazi a tabulky, pokud jeste neexistuji
            DatabaseInitializer.Initialize();

            Application.Run(new Form1());
        }
    }
}