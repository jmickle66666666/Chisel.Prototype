public class IntToFloatConverter : Converter<int, float> {
    protected override float Convert(int obj) {
        return obj;
    }
}
