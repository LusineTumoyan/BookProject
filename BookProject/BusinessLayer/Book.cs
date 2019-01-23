using BusinessLayer.Enums;
using System;
using DataAccessLayer.DBTools;
using System.Collections.Generic;
using DataAccessLayer.Interfaces;
using System.Data.SqlClient;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BusinessLayer
{
    public class Book : IDataMapper
    {
        public int? ID { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public float? Price { get; set; }
        public short? PageCount { get; set; }
        public BookState? State { get; set; }

        private static DBAccess db = null;
        
        static Book()
        {
            db = new DBAccess();
        }
        public Book()
        {

        }
        public Book(int? iD, DateTime? releaseDate, string author, string name,
                    string genre, float? price, short? pageCount, BookState? state)
        {
            ID = iD;
            ReleaseDate = releaseDate;
            Author = author;
            Name = name;
            Genre = genre;
            Price = price;
            PageCount = pageCount;
            State = state;
        }


        public async Task<Book> AddBook()
        {
            List<SPParam> paramList = new List<SPParam>()
            {
                new SPParam("@ReleaseDate", this.ReleaseDate),
                new SPParam("@Author", this.Author),
                new SPParam("@Name", this.Name),
                new SPParam("@Genre", this.Genre),
                new SPParam("@Price", this.Price),
                new SPParam("@PageCount", this.PageCount),
                new SPParam("@State", this.State)
            };

            Book addedBook = await db.ExecuteSPWithSingleResult<Book>("AddBook_Lusine", paramList);
            return addedBook;
        }

        public async Task<Book> UpdateBook()
        {
            List<SPParam> paramList = new List<SPParam>()
            {
                new SPParam("@ID", this.ID),
                new SPParam("@ReleaseDate", this.ReleaseDate),
                new SPParam("@Author", this.Author),
                new SPParam("@Name", this.Name),
                new SPParam("@Genre", this.Genre),
                new SPParam("@Price", this.Price),
                new SPParam("@PageCount", this.PageCount),
                new SPParam("@State", this.State)
            };

            Book updatedBook = await db.ExecuteSPWithSingleResult<Book>("UpdateBook_Lusine", paramList);
            return updatedBook;
        }

        public async Task DeleteBook()
        {
            List<SPParam> paramList = new List<SPParam>()
            {
                new SPParam("@ID", this.ID),
                new SPParam("@ReleaseDate", this.ReleaseDate),
                new SPParam("@Author", this.Author),
                new SPParam("@Name", this.Name),
                new SPParam("@Genre",this.Genre),
                new SPParam("@Price", this.Price),
                new SPParam("@PageCount", this.PageCount),
                new SPParam("@State", this.State)
            };

            await db.ExecuteSPWithNoResult<Book>("DeleteBook_Lusine", paramList);
        }

        public static async Task<List<Book>> AddBooks(List<Book> bookList)
        {
            List<List<SPParam>> paramList = new List<List<SPParam>>();

            foreach (Book book in bookList)
            {
                List<SPParam> myParams = new List<SPParam>()
                {
                    new SPParam("@ReleaseDate", book.ReleaseDate),
                    new SPParam("@Author", book.Author),
                    new SPParam("@Name", book.Name),
                    new SPParam("@Genre", book.Genre),
                    new SPParam("@Price", book.Price),
                    new SPParam("@PageCount", book.PageCount),
                    new SPParam("@State", book.State)
                };
                paramList.Add(myParams);
            }

            List<Book> addedBooks = await db.ExecuteSPWithListResult<Book>("AddBook_Lusine", paramList);

            return addedBooks;
        }

        public static async Task<List<Book>> UpdateBooks(List<Book> bookList)
        {
            List<List<SPParam>> paramList = new List<List<SPParam>>();

            foreach (Book book in bookList)
            {
                List<SPParam> myParams = new List<SPParam>()
                {
                    new SPParam("@ID", book.ID),
                    new SPParam("@ReleaseDate", book.ReleaseDate),
                    new SPParam("@Author", book.Author),
                    new SPParam("@Name", book.Name),
                    new SPParam("@Genre", book.Genre),
                    new SPParam("@Price", book.Price),
                    new SPParam("@PageCount", book.PageCount),
                    new SPParam("@State", book.State)
                };
                paramList.Add(myParams);
            }

            List<Book> updatedBooks = await db.ExecuteSPWithListResult<Book>("UpdateBook_Lusine", paramList);

            return updatedBooks;
        }

        public static async Task<List<Book>> GetBooks()
        {
            List<Book> myBooks = await db.ExecuteSPWithListResult<Book>("GetBooks_Lusine", null);
            return myBooks;
        }

        void IDataMapper.MapEntity(SqlDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                string columnName = reader.GetName(i);
                switch (columnName)
                {
                    case "ID":
                        this.ID = (reader[i] != DBNull.Value) ? Convert.ToInt32(reader[i]) : default(int?);
                        break;
                    case "ReleaseDate":
                        this.ReleaseDate = (reader[i] != DBNull.Value) ? Convert.ToDateTime(reader[i]) : default(DateTime?);
                        break;
                    case "Author":
                        this.Author = (reader[i] != DBNull.Value) ? Convert.ToString(reader[i]) : null;
                        break;
                    case "Name":
                        this.Name = (reader[i] != DBNull.Value) ? Convert.ToString(reader[i]) : null;
                        break;
                    case "Genre":
                        this.Genre = (reader[i] != DBNull.Value) ? Convert.ToString(reader[i]) : null;
                        break;
                    case "Price":
                        this.Price = (reader[i] != DBNull.Value) ? Convert.ToSingle(reader[i]) : default(float?);
                        break;
                    case "PageCount":
                        this.PageCount = (reader[i] != DBNull.Value) ? Convert.ToInt16(reader[i]) : default(short?);
                        break;
                    case "State":
                        this.State = (reader[i] != DBNull.Value) ? (BookState?)Convert.ToByte(reader[i]) : default(BookState?);
                        break;
                }
            }
        }
    }
}
