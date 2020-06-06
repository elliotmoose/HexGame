public class Numbers
{
    public static string TwoDecimalPlace(float number)
    {
        return string.Format("{0:0.##}", number);
    }
}