
public interface Converter {
    object Convert(object obj);
}

public abstract class Converter<T1, T2> : Converter {
    protected abstract T2 Convert(T1 obj);

    public virtual object Convert(object obj) {
        return Convert((T1) obj);
    }
}
