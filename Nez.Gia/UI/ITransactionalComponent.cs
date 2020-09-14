using Nez.UI;

namespace Nez
{
    public interface ITransactionalComponent<T>
    {
        void Take(UserInterface.TransactionalBinding<T> transaction);
    }
}
