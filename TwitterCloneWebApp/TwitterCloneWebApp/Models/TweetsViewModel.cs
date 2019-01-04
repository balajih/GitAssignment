using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TwitterCloneWebApp.DataAccess;

namespace TwitterCloneWebApp.Models
{
    public class TweetsViewModel
    {
        public string Username { get; set; }

        public int  NoOfFollowers { get; set; }

        public int NoOfFollowings { get; set; }

        public int NoOfTweets { get; set; }

        public List<Tweet> Tweets { get; set; }

    }
}