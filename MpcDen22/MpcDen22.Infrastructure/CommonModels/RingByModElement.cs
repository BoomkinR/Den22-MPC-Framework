namespace MpcDen22.Infrastructure.CommonModels;

public abstract class RingByModElement<T>(T primeValue, T value) : RingElement
{
    public T PrimeValue { get; init; } = primeValue;
    public T Value { get; set; } = value;
}