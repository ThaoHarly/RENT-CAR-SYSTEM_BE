using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RentCarSystem.Reponsitories
{
    public class TokenReponsitory : ITokenReponsitory
    {
        private readonly IConfiguration configuration;

        public TokenReponsitory(IConfiguration configuration) //get data from app.json(Jwt:key,...)
        {
            this.configuration = configuration;
        }


        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            //create claims
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Email, user.Email)); //account

            //xac dinh nguoi dung co vai tro gi 
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])); //lay key bi mat tu app.json
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //thong tin xac thuc va ma hoa

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"], //ai phat hanh token(my application)
                configuration["Jwt:Audience"],// ai nhan va su dung token(client,app...)
                claims, //list claims ve thong tin ng dung`
                expires: DateTime.Now.AddMinutes(15),//thoi gian het han
                signingCredentials: credential); // thong tin ky so de dky token)

            return new JwtSecurityTokenHandler().WriteToken(token);// chuyen doi object JwtSecurityToken thanh JWT co the gui cho client
        }
    }
}
