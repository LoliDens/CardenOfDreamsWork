public interface IPayloadState<TPayload> : IExcitableState
{
    void Enter(TPayload payload);
}
