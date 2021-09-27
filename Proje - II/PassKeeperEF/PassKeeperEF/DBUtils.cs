using System;
using System.Collections.Generic;
using System.Linq;

namespace PassKeeperEF
{
    public static class DBUtils
    {
        public static PassKeeperEntities1 ctx = new PassKeeperEntities1();
        
        public static Exception AddUser(string username, string password)
        {
            try
            {
                (_, Exception err) = GetUserByUsername(username);
                if(err == null) { throw new Exception("Bu kullanıcı adı alınmış!"); }

                Users user = new Users()
                {
                    username = username,
                    password = Utils.Encrypt(password),
                };
                ctx.Users.Add(user);
                ctx.SaveChanges();

                (user, err) = GetUserByUsername(username);
                if (err != null) { return err; }

                err = AddCategory("Uncategorized", user.UserID);
                if (err != null) { return err; }

                return null;
            }
            catch(Exception error)
            {
                return error;
            } 
        }

        public static (Users, Exception) GetUserByID(int userId)
        {
            try
            {
                Users user = ctx.Users.Find(userId);
                return(user, null);
            }
            catch(Exception error)
            {
                return(new Users(), error);
            }
        }

        public static (Users, Exception) GetUserByUsername(string username)
        {
            try
            {
                Users user = ctx.Users.Where(s => s.username == username).First();
                return (user, null);
            }
            catch (Exception error)
            {
                return (new Users(), error);
            }
        }

        public static Exception AddCategory(string name, int userId)
        {
            try
            {
                (_, Exception err) = GetCategoryByName(name, userId);
                if (err == null) { throw new Exception("Zaten böyle bir kategoriye sahipsin!"); }
                Categories category = new Categories()
                {
                    name = name,
                    UserID = userId,
                };

                ctx.Categories.Add(category);
                ctx.SaveChanges();
                return null;
            }
            catch(Exception error)
            {
                return error;
            }
        }

        public static (Categories, Exception) GetCategoryByCategoryID(int CategoryId)
        {
            try
            {
                Categories category = ctx.Categories.Find(CategoryId);
                return (category, null);
            }
            catch(Exception error)
            {
                return (new Categories(), error);
            }
        }

        public static (Categories, Exception) GetCategoryByName(string categoryName, int UserId)
        {
            try
            {
                Categories category = ctx.Categories.Where(c => c.UserID == UserId && c.name == categoryName).First();
                return (category, null);
            }
            catch (Exception error)
            {
                return (new Categories(), error);
            }
        }

        public static Exception AddPassword(string name, string pass, int UserID, int CategoryID)
        {
            try
            {
                (_, Exception err) = GetPasswordByName(name, UserID);
                if (err == null) { throw new Exception("Bu isme sahip bir şifren zaten var!"); }
                Passwords newPassword = new Passwords()
                {
                    name = name,
                    password = Utils.Encrypt(pass),
                    UserID = UserID,
                    CategoryID = CategoryID
                };

                ctx.Passwords.Add(newPassword);
                ctx.SaveChanges();

                return null;
            }
            catch(Exception err)
            {   
                return err;
            }
        }

        public static (Passwords, Exception) GetPasswordByID(int PasswordId)
        {
            try
            {
                Passwords password = ctx.Passwords.Find(PasswordId);
                return (password, null);
            }
            catch(Exception err)
            {
                return (new Passwords(), err);
            }
        }

        public static (Passwords, Exception) GetPasswordByName(string name, int UserId)
        {
            try
            {
                Passwords password = ctx.Passwords.Where(p => p.name == name && p.UserID == UserId).First();
                return (password, null);
            }
            catch(Exception err)
            {
                return (new Passwords(), err);
            }
        }

        public static Exception UpdatePasswordByID(int PasswordID, string newName = null, string newPassword = null, int newCategoryId = -1)
        {
            try
            {
                if(newName == null && newPassword == null && newCategoryId == -1) { throw new Exception("Güncellemek için yeterli parametre girilmedi!"); }
                Passwords updatePassword = ctx.Passwords.Find(PasswordID);

                (_, Exception err) = GetPasswordByName(newName, updatePassword.UserID);
                if (err == null) { throw new Exception("Bu isme sahip bir şifren zaten var!"); }

                if (newName != null) { updatePassword.name = newName; }
                if(newPassword != null) { updatePassword.password = newPassword; }
                if (newCategoryId != -1) { updatePassword.CategoryID = newCategoryId; }

                ctx.SaveChanges();
                return null;
            }
            catch (Exception err)
            {
                return err;
            }
        }

        public static Exception UpdatePasswordByName(string currentName, int UserID, string newName = null, string newPassword = null, int newCategoryId = -1)
        {
            try
            {
                (_, Exception err) = GetPasswordByName(newName, UserID);
                if (err == null) { throw new Exception("Bu isme sahip bir şifren zaten var!"); }

                if (newName == null && newPassword == null && newCategoryId == -1) { throw new Exception("Güncellemek için yeterli parametre girilmedi!"); }
                Passwords updatePassword = ctx.Passwords.Where(p => p.name == currentName && p.UserID == UserID).First();

                if (newName != null) { updatePassword.name = newName; }
                if (newPassword != null) { updatePassword.password = newPassword; }
                if (newCategoryId != -1) { updatePassword.CategoryID = newCategoryId; }

                ctx.SaveChanges();
                return null;
            }
            catch (Exception err)
            {
                return err;
            }
        }

        public static Exception UpdateCategoryByID(int categoryID, string newName = null)
        {
            try
            {

                if (newName == null) { throw new Exception("Kategoriyi güncellemek için yeni ismini girmelisiniz!"); }
                Categories category = ctx.Categories.Find(categoryID);

                if (category.name == "Uncategorized") { throw new Exception("Bu kategori default olduğu için değiştirilemez!"); }

                (_, Exception err) = GetCategoryByName(newName, category.UserID);
                if (err == null) { throw new Exception("Zaten böyle bir kategoriye sahipsin!"); }

                category.name = newName;
                ctx.SaveChanges();
                return null;
            }
            catch(Exception err)
            {
                return err;
            }
        }

        public static Exception UpdateCategoryByName(string currentName, int UserID, string newName = null)
        {
            try
            {
                if (currentName == "Uncategorized") { throw new Exception("Bu kategori default olduğu için değiştirilemez!"); }

                (_, Exception err) = GetCategoryByName(currentName, UserID);
                if (err == null) { throw new Exception("Zaten böyle bir kategoriye sahipsin!"); }

                if (newName == null) { throw new Exception("Kategoriyi güncellemek için yeni ismini girmelisiniz!"); }
                Categories category = ctx.Categories.Where(c => c.name == currentName && c.UserID == UserID).First();
                category.name = newName;
                ctx.SaveChanges();
                return null;
            }
            catch (Exception err)
            {
                return err;
            }
        }

        public static Exception DeletePasswordByID(int PasswordID)
        {
            try
            {
                (Passwords password, Exception err) = GetPasswordByID(PasswordID);
                if(err != null) { return err; }
                ctx.Passwords.Remove(password);
                ctx.SaveChanges();
                return null;
            }
            catch(Exception err)
            {
                return err;
            }
        }

        public static Exception DeletePasswordByName(string PasswordName, int UserID)
        {
            try
            {
                (Passwords password, Exception err) = GetPasswordByName(PasswordName, UserID);
                if (err != null) { return err; }
                ctx.Passwords.Remove(password);
                ctx.SaveChanges();
                return null;
            }
            catch (Exception err)
            {
                return err;
            }
        }

        public static Exception DeleteCategoryByID(int CategoryID)
        {
            try
            {
                (Categories category, Exception err) = GetCategoryByCategoryID(CategoryID);
                if (category.name == "Uncategorized") { throw new Exception("Bu kategori default olduğu için silinemez!"); }
                if (err != null) { return err; }
                ctx.Categories.Remove(category);
                ctx.SaveChanges();
                return null;
            }
            catch(Exception err)
            {
                return err;
            }
        }

        public static Exception DeleteCategoryByName(string CategoryName, int UserID)
        {
            try
            {
                if (CategoryName == "Uncategorized") { throw new Exception("Bu kategori default olduğu için silinemez!"); }

                (Categories category, Exception err) = GetCategoryByName(CategoryName, UserID);
                if (err != null) { return err; }
                (Categories Uncategorized, Exception err2) = GetCategoryByName("Uncategorized", UserID);
                if(err2 != null) { return err; }
                err = UpdatePassCatsByCatID(category.CategoryID, UserID, Uncategorized.CategoryID);
                if(err!= null) { return err; }
                ctx.Categories.Remove(category);
                ctx.SaveChanges();
                return null;
            }
            catch (Exception err)
            {
                return err;
            }
        }

        public static Exception UpdatePassCatsByCatID(int CategoryID, int UserID, int newCategoryID)
        {
            try
            {
                List<Passwords> password = ctx.Passwords.Where(p => p.CategoryID == CategoryID && p.UserID == UserID).ToList();
                password.ForEach(p =>
                {
                    p.CategoryID = newCategoryID;
                });
                ctx.SaveChanges();
                return null;
            }
            catch(Exception error)
            {
                return error;
            }
            
        }

        public static Exception AddMultipleUsers(List<Users> AllUsers)
        {
            try
            {
                ctx.Users.AddRange(AllUsers);
                ctx.SaveChanges();
                return null;
            }
            catch (Exception err)
            {
                return err;
            }
        }

        public static Exception AddMultiplePasswords(List<Passwords> AllPasswords)
        {
            try
            {
                ctx.Passwords.AddRange(AllPasswords);
                ctx.SaveChanges();
                return null;
            }
            catch (Exception err)
            {
                return err;
            }
        }

        public static Exception AddMultipleCategories(List<Categories> AllCategories)
        {
            try
            {
                ctx.Categories.AddRange(AllCategories);
                ctx.SaveChanges();
                return null;
            }
            catch (Exception err)
            {
                return err;
            }
        }

        public static (List<Users>, Exception) GetAllUsers()
        {
            try
            {
                return (ctx.Users.ToList(), null);
            }
            catch (Exception err)
            {
                return (new List<Users>(), err);
            }
        }

        public static (List<Passwords>, Exception) GetAllPasswords()
        {
            try
            {
                return (ctx.Passwords.ToList(), null);
            }
            catch (Exception err)
            {
                return (new List<Passwords>(), err);
            }
        }

        public static (List<Categories>, Exception) GetAllCategories()
        {
            try
            {
                return (ctx.Categories.ToList(), null);
            }
            catch (Exception err)
            {
                return (new List<Categories>(), err);
            }
        }

        public static (List<Passwords>, Exception) GetAllPasswordsByUserID(int UserID, string like = null)
        {
            try
            {
                if (like == null)
                {
                    return (ctx.Passwords.Where(p => p.UserID == UserID).ToList(), null);
                }
                else
                {
                    return (ctx.Passwords.Where(p => p.UserID == UserID && p.name.Contains(like)).ToList(), null);
                }
                
            }
            catch(Exception err)
            {
                return (new List<Passwords>(), err);
            }
        }

        public static (List<Categories>, Exception) GetAllCategoriesByUserID(int UserID, string like = null)
        {
            try
            {
                if (like == null)
                {
                    return (ctx.Categories.Where(c => c.UserID == UserID).ToList(), null);
                }
                else
                {
                    return (ctx.Categories.Where(c => c.UserID == UserID && c.name.Contains(like)).ToList(), null);
                }
            }
            catch (Exception err)
            {
                return (new List<Categories>(), err);
            }
        }

        public static (List<Passwords>, Exception) GetAllPasswordsByCategoryID(int CategoryID, int UserID, string like = null)
        {
            try
            {
                if (like == null)
                {
                    return (ctx.Passwords.Where(p => p.UserID == UserID && p.CategoryID == CategoryID).ToList(), null);
                }
                else
                {
                    return (ctx.Passwords.Where(p => p.UserID == UserID && p.CategoryID == CategoryID && p.name.Contains(like)).ToList(), null);
                }
                
            }
            catch (Exception err)
            {
                return (new List<Passwords>(), err);
            }
        }
    }
}
