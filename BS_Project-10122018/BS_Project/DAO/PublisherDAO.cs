using BS_Project.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace BS_Project.DAO
{
    public class PublisherDAO
    {
        private BSDBContext db = new BSDBContext();
        /// <summary>
        /// Tao ra danh sach cac Publisher dua vao phan trang, moi lan se co pageSize = 5
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<Publisher> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Publisher> model = db.Publishers.Where(x => x.isDeleted == false);
            if (!string.IsNullOrEmpty(searchString))
            {
                model = db.Publishers.Where(x => x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.PublisherID).ToPagedList(page, pageSize);
        }

        public int Insert(Publisher publisher)
        {
            db.Publishers.Add(publisher);
            db.SaveChanges();
            return publisher.PublisherID;
        }

        public Publisher ViewDetail(int id)
        {
            return db.Publishers.Find(id);
        }

        public bool Update(Publisher publisher, string imageURL)
        {
            try
            {
                var publisherOld = db.Publishers.Find(publisher.PublisherID);
                publisherOld.Name = publisher.Name;
                publisherOld.ImageURL = imageURL;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var publisher = db.Publishers.Find(id);
                publisher.isDeleted = true;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}