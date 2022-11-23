namespace ServerAPI.Servieces.JWT
{
    public interface IJWTServiece
    {
        string CreateToken(User user);
    }
}
