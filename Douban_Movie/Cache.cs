using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanoramaApp2
{
    class Cache
    {
        private static LinkedList<Movie> movieList = new LinkedList<Movie>();
        private static int MOVIE_MAX_SIZE = 10;
        private static readonly object movieSyncLock = new object();

        private static LinkedList<Review> reviewList = new LinkedList<Review>();
        private static int REVIEW_MAX_SIZE = 10;
        private static readonly object reviewSyncLock = new object();

        private static LinkedList<MovieImage> imageList = new LinkedList<MovieImage>();
        private static int IMAGE_MAX_SIZE = 150;
        private static readonly object imageSyncLock = new object();

        private static LinkedList<People> peopleList = new LinkedList<People>();
        private static int PEOPLE_MAX_SIZE = 10;
        private static readonly object peopleSyncLock = new object();

        public static void insertReview(Review r) {
            lock (reviewSyncLock) {
                if (reviewList.Count >= REVIEW_MAX_SIZE) {
                    reviewList.RemoveLast();
                }
                reviewList.AddFirst(r);
            }
        }

        public static Review getReview(string id)
        {
            Review rw = null;
            lock (reviewSyncLock)
            {
                foreach (Review r in reviewList)
                {
                    if (r.id == id)
                    {
                        rw = r;
                        break;
                    }
                }
                if (rw == null)
                {
                    return null;
                }
                reviewList.Remove(rw);
                reviewList.AddFirst(rw);
            }
            return rw;
        }

        public static void insertPeople(People p)
        {
            lock (peopleSyncLock)
            {
                if (peopleList.Count >= PEOPLE_MAX_SIZE)
                {
                    peopleList.RemoveLast();
                }
                peopleList.AddFirst(p);
            }
        }

        public static People getPeople(string id)
        {
            People p = null;
            lock (peopleSyncLock)
            {
                foreach (People pp in peopleList)
                {
                    if (pp.id == id)
                    {
                        p = pp;
                        break;
                    }
                }
                if (p == null)
                {
                    return null;
                }
                peopleList.Remove(p);
                peopleList.AddFirst(p);
            }
            return p;
        }

        public static void insertImage(MovieImage img)
        {
            lock (imageSyncLock)
            {
                if (imageList.Count >= IMAGE_MAX_SIZE)
                {
                    imageList.RemoveLast();
                }
                imageList.AddFirst(img);
            }
        }

        public static MovieImage getImage(string id)
        {
            MovieImage img = null;
            lock (imageSyncLock)
            {
                foreach (MovieImage i in imageList)
                {
                    if (i.id == id)
                    {
                        img = i;
                        break;
                    }
                }
                if (img == null)
                {
                    return null;
                }
                imageList.Remove(img);
                imageList.AddFirst(img);
            }
            return img;
        }


        public static void insertMovie(Movie m)
        {
            lock (movieSyncLock)
            {
                if (movieList.Count >= MOVIE_MAX_SIZE)
                {
                    movieList.RemoveLast();
                }
                movieList.AddFirst(m);
            }
        }

        public static Movie getMovie(string id)
        {
            Movie result = null;
            lock (movieSyncLock)
            {
                foreach (Movie movie in movieList)
                {
                    if (movie.id == id)
                    {
                        result = movie;
                        break;
                    }
                }
                if (result == null)
                {
                    return null;
                }
                movieList.Remove(result);
                movieList.AddFirst(result);
            }
            return result;
        }
    }
}
