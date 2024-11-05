using Microsoft.AspNetCore.Identity;
<<<<<<< HEAD
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using RentCarSystem.Models.Domain;
=======
using Microsoft.IdentityModel.Tokens;
>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7
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


<<<<<<< HEAD
        public string CreateJWTToken(User user, List<string> roles)
        {
            //create claims for the token
=======
        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            //create claims
>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Email, user.Email)); //account

<<<<<<< HEAD
            //add role claims
=======
            //xac dinh nguoi dung co vai tro gi 
>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

<<<<<<< HEAD
            //Generate security key from the secret in appsettings.json
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])); //lay key bi mat tu app.json

            //Create signing credentials
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //thong tin xac thuc va ma hoa

            //Define the token properties
=======

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])); //lay key bi mat tu app.json
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //thong tin xac thuc va ma hoa

>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"], //ai phat hanh token(my application)
                configuration["Jwt:Audience"],// ai nhan va su dung token(client,app...)
                claims, //list claims ve thong tin ng dung`
                expires: DateTime.Now.AddMinutes(15),//thoi gian het han
                signingCredentials: credential); // thong tin ky so de dky token)

<<<<<<< HEAD
            //return the JWT token as a string
=======
>>>>>>> 245adf983c80f561f0c244ccf5e507c9b3b495e7
            return new JwtSecurityTokenHandler().WriteToken(token);// chuyen doi object JwtSecurityToken thanh JWT co the gui cho client
        }
    }
}
