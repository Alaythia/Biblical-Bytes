namespace BiblicalBytes.Contracts;

public interface IConverter<in TInput, out TOutput>
{
    TOutput Convert(TInput input);
}