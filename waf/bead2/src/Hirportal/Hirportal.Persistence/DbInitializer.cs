using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hirportal.Persistence
{
    public static class DbInitializer
    {
        private static NewsContext _context;

        public static void Initialize(NewsContext context, UserManager<Author> userManager = null)
        {
            _context = context;

            //_context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Database.Migrate();

            if (_context.Articles.Any())
            {
                return;
            }
            else
            {
                Author gloria = new Author() { UserName = "pdp", Name = "Glorya Borger" };
                Author nervHQ = new Author() { UserName = "notkaji", Name = "Nerv HQ" };
                Author nyan = new Author() { UserName = "nyancat", Name = "Nyan Cat" };
                if (userManager != null)
                {
                    var x1 = userManager.CreateAsync(gloria, "hmmm").Result;
                    var x2 = userManager.CreateAsync(nervHQ, "impact").Result;
                    var x3 = userManager.CreateAsync(nyan, "nyan").Result;
                }
                _context.Authors.Add(gloria);
                _context.Authors.Add(nervHQ);
                _context.Authors.Add(nyan);
                _context.SaveChanges();
                EvaNews(nervHQ);
                CatNews(nyan);
            }
        }

        private static void EvaNews(Author nerv)
        {
            Article article = new Article()
            {
                Author = nerv,
                Title = "Local Teen Ruins Everything",
                Description = "Even after radical goodbyes from Misato, Shinji doesn't look like the guy to save the afternoon.",
                Leading = true,
                Content = @"NERV Headquarters is under heavy attack since this morning by none other than it's sustainer
                          organisation, SEELE. Events escalete as the latter tries to set the Human Instrumentality Project in
                          motion using Evangelion Unit 01 pilot Ikari Shinji and the mass-produced EVA Series, while Shinji's dad
                          and Chief of Command Ikari Gendo also moves in to gain control of Instrumentality.

                          As per Misato's last wish, Shinji takes on piloting the EVA one last time, but is heavily affected
                          by seeing the remains of the just destroyed Evangelion Unit 02, piloted by the fellow German ace-pilot
                          Sohryu Langley Asuka. The stress once again renders our supposed-to-be hero incapabable of acting
                          reasonably, and SEELE does not stop to think for a second to use Shinji and EVA 01 as the core to the
                          Humand Instrumentality Project.

                          Right now, the Geofront with NERV HQ on the top of it is ascending through the atmosphere, while Professor
                          Fuyutsuki is making incomprehensible comments about the stages of Human Instrumentality in the Command Center.
                          We'll be reporting as soon as any new details surface!"
            };

            article.Images.Add(
                new ArticleImage()
                {
                    ImageData = Properties.Resources.impact1,
                }
            );
            article.Images.Add(
                new ArticleImage()
                {
                    ImageData = Properties.Resources.impact2,
                }
            );

            _context.Articles.Add(article);
            _context.SaveChanges();
        }

        private static void CatNews(Author cat)
        {
            for (int i = 0; i < 45; i++)
            {
                Article article = new Article()
                {
                    Author = cat,
                    Title = "NyanNyanNyaNyan NyaNyaNyaNyan NyanNyan",
                    Leading = false,
                    Description = "Have my breakfast spaghetti yarn",
                    Content = "Chew the plant demand to be let outside at once, and expect owner to wait for me as i think about it friends" +
                              "are not food so loves cheeseburgers for spend six hours per day washing, but still have a crusty butthole." +
                              "Lay on arms while you're using the keyboard reward the chosen human with a slow blink chase red laser dot but" +
                              "that box? i can fit in that box but wake up wander around the house making large amounts of noise jump on top" +
                              "of your human's bed and fall asleep again, yet fish i must find my red catnip fishy fish and sit in a box for" +
                              "hours. Hunt by meowing loudly at 5am next to human slave food dispenser go into a room to decide you didn't" +
                              "want to be in there anyway find a way to fit in tiny box so sun bathe suddenly go on wild-eyed crazy rampage." +
                              "Cat cat moo moo lick ears lick paws small kitty warm kitty little balls of fur inspect anything brought into" +
                              "the house, and present belly, scratch hand when stroked leave fur on owners clothes eat prawns daintily with a" +
                              "claw then lick paws clean wash down prawns with a lap of carnation milk then retire to the warmest spot on the" +
                              "couch to claw at the fabric before taking a catnap or shake treat bag."
                };
                _context.Articles.Add(article);
                _context.SaveChanges();
            }
        }
    }



}
