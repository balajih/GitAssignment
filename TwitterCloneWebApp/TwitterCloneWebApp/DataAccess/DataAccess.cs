using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TwitterCloneWebApp.DataAccess;
using TwitterCloneWebApp.Models;

namespace TwitterCloneWebApp
{
    public class DataAccessLayer
    {
        public static async Task<int> CreatePersonAsync(Person person)
        {
            using (var dbContext = new Entities())
            {
                dbContext.People.Add(person);
                return await dbContext.SaveChangesAsync();
            }               
        }
        public static Person GetPersonAsync(string userName)
        {
            using (var dbContext = new Entities())
            {
                dbContext.Configuration.LazyLoadingEnabled = false;

                var result = dbContext.People
                    .FirstOrDefault(x=> x.UserId.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
                return result;
            }
        }

        public static bool FollowUser(string followUserId ,string loggedInUserId)
        {
            using (var dbContext = new Entities())
            {
                dbContext.Followings.Add(new Following { UserId = loggedInUserId, FollowingId = followUserId });
                dbContext.SaveChanges();
                return true;
            }
            return false;
        }


        public static List<Person> IsValidUser(string username, string password = "")
        {
            using (var dbContext = new Entities())
            {
                var result = dbContext.People.Where(x =>
                x.UserId.Equals(username, StringComparison.InvariantCultureIgnoreCase));

                if(password != "")
                {
                    result = result.Where(x => x.Pwd.Equals(password));
                }

                return result.ToList();
            }
        }

        public static TweetsViewModel GetTweetsForCurrentUser(string userName)
        {
            TweetsViewModel viewModel = new TweetsViewModel();
            using (var dbContext = new Entities())
            {
                var followingIDs = dbContext.Followings
                                           .Where(x => x.UserId == userName)
                                           .Select(x => x.FollowingId);

                var noOfFollowers = dbContext.Followings
                                           .Count(x => x.FollowingId == userName);


                var mytweets = dbContext.Tweets.Where(x => x.UserId == userName)
                                               .ToList();

                var followersTweet = dbContext.Tweets.Where(x => followingIDs.Contains(x.UserId))
                                                     .ToList();

                var allTweets = mytweets.Union(followersTweet)
                                        .OrderByDescending(x=> x.Created);
                                            

                viewModel.NoOfFollowings = followingIDs.Count();
                viewModel.NoOfFollowers = noOfFollowers;
                viewModel.NoOfTweets = mytweets.Count();
                viewModel.Tweets = allTweets.ToList();
                
                return viewModel;
            }
        }
        public static bool UpdateTweet(string msg, string userId)
        {
            bool result = false;
            using (var dbContext = new Entities())
            {
                dbContext.Tweets.Add(new Tweet
                {
                    Message = msg,
                    UserId = userId,
                    Created = DateTime.Now
                });
                dbContext.SaveChanges();
                result = true;
            }
            return result;
        }
    }
}