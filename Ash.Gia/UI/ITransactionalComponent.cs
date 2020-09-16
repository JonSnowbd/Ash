using Ash.UI;

namespace Ash
{
    public interface ITransactionalComponent<T>
    {
        void Take(UserInterface.TransactionalBinding<T> transaction);
    }
}
