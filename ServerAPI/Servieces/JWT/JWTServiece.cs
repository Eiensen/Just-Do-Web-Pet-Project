using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace ServerAPI.Servieces.JWT
{
    public class JWTServiece : IJWTServiece
    {
        private readonly SymmetricSecurityKey _key;

        private readonly IConfiguration _configuration;

        public JWTServiece(IConfiguration configuration)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
                    (
                        configuration.GetSection("Jwt:Key").Value
                    ));

            _configuration = configuration;
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id)
            };

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _configuration.GetSection("Jwt:Issuer").Value,
                Audience = _configuration.GetSection("Jwt:Issuer").Value,
                Expires = DateTime.UtcNow.AddMonths(6),
                SigningCredentials = credentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
