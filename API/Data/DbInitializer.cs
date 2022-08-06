using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(GameContext gameContext)
        {
            if (gameContext.player.Any()) return;
            var player = new List<Player> {
                new Player {
                id = 1,
                name ="Himanshu Sharma",
                avatarURL = "/Public/Avatars/batmanAvatars.png",
                pictureURL="",
                description="God of Thunder"
                },

                new Player {
                id = 2,
                name ="Sherleen Surya",
                avatarURL = "/Public/Avatars/avengerAvatar.png",
                pictureURL="",
                description="Avengers Assemble"
                },

                new Player {
                id = 3,
                name ="Smriti Sharma",
                avatarURL = "/Public/Avatars/ladyPirate.png",
                pictureURL="",
                description="Wonder Woman"
                },

                new Player {
                id = 4,
                name ="Aniket Shrivastava",
                avatarURL = "/Public/Avatars/bobAvatar.png",
                pictureURL="",
                description="I am Bob the Builder"
                },

                new Player {
                id = 5,
                name ="Aryan Sethi",
                avatarURL = "/Public/Avatars/hitmanAvatar.png",
                pictureURL="",
                description="I am The Hitman"
                },
           };

            foreach (var singlePlayer in player)
            {
                gameContext.player.Add(singlePlayer);
            }

           await gameContext.SaveChangesAsync();
        }
    }
}
