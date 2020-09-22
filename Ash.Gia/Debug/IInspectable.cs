namespace Ash
{
    public interface IInspectable
    {
        void Inspect(GiaScene context);
        void Uninspect(GiaScene context);
    }
}
