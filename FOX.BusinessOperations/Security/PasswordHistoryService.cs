using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FOX.DataModels.Models.Security;
using FOX.DataModels.Models.Settings.User;
using FOX.DataModels.GenericRepository;
using FOX.DataModels.Context;
using FOX.BusinessOperations.CommonService;
using System.Security.Cryptography;

namespace FOX.BusinessOperations.Security
{
    public class PasswordHistoryService : IPasswordHistoryService
    {
        private DbContextSecurity context = new DbContextSecurity();
        private readonly GenericRepository<PasswordHistory> _passwordHistoryRepositoy;
        private readonly GenericRepository<User> _userRepository;
        public PasswordHistoryService()
        {
            _passwordHistoryRepositoy = new GenericRepository<PasswordHistory>(context);
            _userRepository = new GenericRepository<User>(context);
        }
        public bool CheckIfPasswordIsPreviouslyUser(PreviousPasswordCheck model, UserProfile profile)
        {
            var user = _userRepository.GetByID(model.user_id);
            bool isPreviousPassword = false;
            if (user != null)
            {
                var newPassword = Encrypt.getEncryptedCode(model.password);
                List<PasswordHistory> previousPasswords = context.PasswordHistories.OrderByDescending(x => x.PASSWORD_ID).Where(u => u.USER_ID.Equals(user.USER_ID) && u.DELETED == false).Take(5).ToList();
                foreach (var password in previousPasswords)
                {
                    if (VerifyHashedPassword(password.PASSWORD, model.password) || Equals(Encrypt.getEncryptedCode(model.password), password.PASSWORD))
                    {
                        isPreviousPassword = true;
                    }
                }
            }
            return isPreviousPassword;
        }
        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return buffer3.SequenceEqual(buffer4);
        }

    }
}