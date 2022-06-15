
namespace Project.Pool
{
    #region Standard

    public interface IEnqueued
    {
        void OnEnqueued();
    }
    public interface IDequeued
    {
        void OnDequeued();
    }

    public interface IPooled : IEnqueued, IDequeued
    {

    }

    #endregion
}