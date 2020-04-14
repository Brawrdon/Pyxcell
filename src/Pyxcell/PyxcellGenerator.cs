namespace Pyxcell
{
    public interface IPyxcellGenerator
    {
        void Generate(string message);

        string DrawToBase64();
    }
}