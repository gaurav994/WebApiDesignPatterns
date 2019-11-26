using DataModel.GenericRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.UnitOfWork
{
    public class UnitOfWork : IDisposable
    {

        #region private member variables 
        private testDatabaseEntities _context = null;
        private GenericRepository<User> _userRepository;
        private GenericRepository<Token> _tokenRepository;
        private GenericRepository<Product> _productRepository;
        #endregion
        public UnitOfWork()
        {
            _context = new testDatabaseEntities();
        }

        #region public repository creation properties
        public GenericRepository<Product> ProductRepository
        {
            get
            {
                if (this._productRepository == null)
                    this._productRepository = new GenericRepository<Product>(_context);
                return _productRepository;
            }
        }

        public GenericRepository<User> UserRepository
        {
            get
            {
                if (this._userRepository == null)
                {
                    this._userRepository = new GenericRepository<User>(_context);

                }
                return _userRepository;
            }
        }

        public GenericRepository<Token> TokenRepository
        {
            get
            {
                if (this._tokenRepository == null)
                {
                    this._tokenRepository = new GenericRepository<Token>(_context);

                }
                return _tokenRepository;
            }
        }
        #endregion


        #region public member methods
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var outputLines = new List<string>();
                foreach (var ever in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors;",
                        DateTime.Now, ever.Entry.Entity.GetType().Name, ever.Entry.State));
                    foreach (var ve in ever.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \" {0} \". Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);
                throw e;
            }
        }

        #endregion
        #region implement dispose variable declaration
        #region private dispose variable declaration
        private bool disposed = false;
        #endregion
        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("Unit of Work being disposed");
                    _context.Dispose();
                }
            }
            this.disposed = true;

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
