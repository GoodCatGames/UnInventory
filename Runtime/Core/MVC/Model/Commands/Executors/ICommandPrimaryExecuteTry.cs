namespace UnInventory.Core.MVC.Model.Commands.Executors
{
    public interface ICommandPrimaryExecuteTry
    {
        bool ExecuteTry(ICommand command);
    }
}