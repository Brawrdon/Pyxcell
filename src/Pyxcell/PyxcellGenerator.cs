namespace Pyxcell
{
    public abstract class PyxcellGenerator
    {
        public abstract void Generate(string message);

        public abstract string DrawToBase64();
    }
}