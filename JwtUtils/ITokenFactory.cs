namespace JwtUtils
{
    public interface ITokenFactory
    {
        string GenerateToken(int size = 32);
    }
}
