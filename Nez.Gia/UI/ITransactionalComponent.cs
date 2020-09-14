using Nez.UI;

namespace Nez
{
    public interface ITransactionalComponent<T>
    {
        void Take(UICluster.TransactionalValue<T> transaction);
    }
}
