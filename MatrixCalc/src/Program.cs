namespace MatrixCalc
{
    /// <summary>
    ///     Main class
    /// </summary>
    internal class Program
    {
        /// <summary>
        ///     Main Method
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            while (true)
            {
                MenuController.ShowMenu();
                var actionIndex = MenuController.AskUserForAction();

                MenuController.ShowInputOptions(actionIndex);
                var inputOptionIndex = MenuController.AskUserForInputMethod(actionIndex);

                MenuController.DoAction(actionIndex, inputOptionIndex);
            }
        }
    }
}