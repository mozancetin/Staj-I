using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PassKeeperWeb.Models;

namespace PassKeeperWeb
{
    public class DBUtils
    {
        public static PassKeeperContext ctx = new PassKeeperContext();

        public static Exception AddUser(string username, string password)
        {
            /*
            using (PassKeeperEntities1 ctx2 = new PassKeeperEntities1())
            {

            } 
            */
            try
            {
                (_, Exception err) = GetUserByUsername(username);
                if (err == null) { throw new Exception("Bu kullanıcı adı alınmış!"); }
                User user = new User()
                {
                    Username = username,
                    Password = Utils.Encrypt(password),
                };
                ctx.Users.Add(user);
                ctx.SaveChanges();

                (user, err) = GetUserByUsername(username);
                if (err != null) { return err; }

                err = AddCategory("Uncategorized", user.UserId);
                if (err != null) { return err; }

                return null;
            }
            catch (Exception error)
            {
                return error;
            }
        }

        public static Exception DeleteUserByID(int UserID)
        {
            try
            {
                (User user, Exception err) = GetUserByID(UserID);
                if (err != null)
                {
                    return err;
                }

                err = DeletePasswordsByUserID(UserID);
                if (err != null)
                {
                    return err;
                }

                err = DeleteCategoriesByUserID(UserID);
                if (err != null)
                {
                    return err;
                }

                ctx.Users.Remove(user);
                ctx.SaveChanges();
                return null;
            }
            catch (Exception error)
            {
                return error;
            }
            
        }

        public static (User, Exception) GetUserByID(int userId, bool withID = true)
        {
            try
            {
                if (withID)
                {
                    User user = ctx.Users.Find(userId);
                    return (user, null);
                }
                else
                {
                    User user = ctx.Users.Find(userId);
                    ICollection<Category> newCategories = new HashSet<Category>();
                    ICollection<Password> newPasswords = new HashSet<Password>();
                    foreach(var category in user.Categories)
                    {
                        newCategories.Add(new Category()
                        {
                            Name = category.Name,
                        });
                    }

                    foreach(var password in user.Passwords)
                    {
                        newPasswords.Add(new Password()
                        {
                            Name = password.Name,
                            Password1 = password.Password1,
                        });
                    }
                    return (new User() { Categories=newCategories, Password=user.Password, Passwords=newPasswords, Username=user.Username}, null);
                }
                
            }
            catch (Exception error)
            {
                return (new User(), error);
            }
        }

        public static (User, Exception) GetUserByUsername(string username)
        {
            try
            {
                User user = ctx.Users.Where(s => s.Username == username).First();
                return (user, null);
            }
            catch (Exception error)
            {
                return (new User(), error);
            }
        }

        public static Exception AddCategory(string name, int userId)
        {
            try
            {
                (_, Exception err) = GetCategoryByName(name, userId);
                if (err == null) { throw new Exception("Zaten böyle bir kategoriye sahipsin!"); }
                Category category = new Category()
                {
                    Name = name,
                    UserId = userId,
                };

                ctx.Categories.Add(category);
                ctx.SaveChanges();
                return null;
            }
            catch (Exception error)
            {
                return error;
            }
        }

        public static (Category, Exception) GetCategoryByCategoryID(int CategoryId)
        {
            try
            {
                Category category = ctx.Categories.Find(CategoryId);
                return (category, null);
            }
            catch (Exception error)
            {
                return (new Category(), error);
            }
        }

        public static (Category, Exception) GetCategoryByName(string categoryName, int UserId)
        {
            try
            {
                Category category = ctx.Categories.Where(c => c.UserId == UserId && c.Name == categoryName).First();
                return (category, null);
            }
            catch (Exception error)
            {
                return (new Category(), error);
            }
        }

        public static Exception AddPassword(string name, string pass, int UserID, int CategoryID)
        {
            try
            {
                (_, Exception err) = GetPasswordByName(name, UserID);
                if (err == null) { throw new Exception("Bu isme sahip bir şifren zaten var!"); }
                Password newPassword = new Password()
                {
                    Name = name,
                    Password1 = Utils.Encrypt(pass),
                    UserId = UserID,
                    CategoryId = CategoryID
                };

                ctx.Passwords.Add(newPassword);
                ctx.SaveChanges();

                return null;
            }
            catch (Exception err)
            {
                return err;
            }
        }

        public static (Password, Exception) GetPasswordByID(int PasswordId)
        {
            try
            {
                Password password = ctx.Passwords.Find(PasswordId);
                return (password, null);
            }
            catch (Exception err)
            {
                return (new Password(), err);
            }
        }

        public static (Password, Exception) GetPasswordByName(string name, int UserId)
        {
            try
            {
                Password password = ctx.Passwords.Where(p => p.Name == name && p.UserId == UserId).First();
                return (password, null);
            }
            catch (Exception err)
            {
                return (new Password(), err);
            }
        }

        public static Exception UpdatePasswordByID(int PasswordID, string newName = null, string newPassword = null, int newCategoryId = -1)
        {
            try
            {
                if (newName == null && newPassword == null && newCategoryId == -1) { throw new Exception("Güncellemek için yeterli parametre girilmedi!"); }
                Password updatePassword = ctx.Passwords.Find(PasswordID);

                (_, Exception err) = GetPasswordByName(newName, updatePassword.UserId);
                if (err == null) { throw new Exception("Bu isme sahip bir şifren zaten var!"); }

                if (newName != null) { updatePassword.Name = newName; }
                if (newPassword != null) { updatePassword.Password1 = newPassword; }
                if (newCategoryId != -1) { updatePassword.CategoryId = newCategoryId; }

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
                Password updatePassword = ctx.Passwords.Where(p => p.Name == currentName && p.UserId == UserID).First();

                if (newName != null) { updatePassword.Name = newName; }
                if (newPassword != null) { updatePassword.Password1 = Utils.Encrypt(newPassword); }
                if (newCategoryId != -1) { updatePassword.CategoryId = newCategoryId; }

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
                Category category = ctx.Categories.Find(categoryID);

                if (category.Name == "Uncategorized") { throw new Exception("Bu kategori default olduğu için değiştirilemez!"); }

                (_, Exception err) = GetCategoryByName(newName, category.UserId);
                if (err == null) { throw new Exception("Zaten böyle bir kategoriye sahipsin!"); }

                category.Name = newName;
                ctx.SaveChanges();
                return null;
            }
            catch (Exception err)
            {
                return err;
            }
        }

        public static Exception UpdateCategoryByName(string currentName, int UserID, string newName = null)
        {
            try
            {
                if (currentName == "Uncategorized") { throw new Exception("Bu kategori default olduğu için değiştirilemez!"); }

                (_, Exception err) = GetCategoryByName(newName, UserID);
                if (err == null) { throw new Exception("Zaten böyle bir kategoriye sahipsin!"); }

                if (newName == null) { throw new Exception("Kategoriyi güncellemek için yeni ismini girmelisiniz!"); }
                Category category = ctx.Categories.Where(c => c.Name == currentName && c.UserId == UserID).First();
                category.Name = newName;
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
                (Password password, Exception err) = GetPasswordByID(PasswordID);
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

        public static Exception DeletePasswordByName(string PasswordName, int UserID)
        {
            try
            {
                (Password password, Exception err) = GetPasswordByName(PasswordName, UserID);
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
                (Category category, Exception err) = GetCategoryByCategoryID(CategoryID);
                if (category.Name == "Uncategorized") { throw new Exception("Bu kategori default olduğu için silinemez!"); }
                if (err != null) { return err; }
                ctx.Categories.Remove(category);
                ctx.SaveChanges();
                return null;
            }
            catch (Exception err)
            {
                return err;
            }
        }

        public static Exception DeleteCategoryByName(string CategoryName, int UserID)
        {
            try
            {
                if (CategoryName == "Uncategorized") { throw new Exception("Bu kategori default olduğu için silinemez!"); }

                (Category category, Exception err) = GetCategoryByName(CategoryName, UserID);
                if (err != null) { return err; }
                (Category Uncategorized, Exception err2) = GetCategoryByName("Uncategorized", UserID);
                if (err2 != null) { return err; }
                err = UpdatePassCatsByCatID(category.CategoryId, UserID, Uncategorized.CategoryId);
                if (err != null) { return err; }
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
                List<Password> password = ctx.Passwords.Where(p => p.CategoryId == CategoryID && p.UserId == UserID).ToList();
                password.ForEach(p =>
                {
                    p.CategoryId = newCategoryID;
                });
                ctx.SaveChanges();
                return null;
            }
            catch (Exception error)
            {
                return error;
            }

        }

        public static Exception AddMultipleUsers(List<User> AllUsers)
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

        public static Exception AddUserWithUserObject(User user)
        {
            try
            {
                ctx.Users.Add(user);
                ctx.SaveChanges();
                return null;
            }
            catch (Exception err)
            {
                return err;
            }
        }

        public static Exception AddMultiplePasswords(List<Password> AllPasswords, int UserID = -1)
        {
            try
            {
                if (UserID == -1)
                {
                    ctx.Passwords.AddRange(AllPasswords);
                    ctx.SaveChanges();
                    return null;
                }
                else
                {
                    List<Password> newAllPasswords = new List<Password>();
                    AllPasswords.ForEach(p =>
                    {
                        (Category _category, Exception err) = GetCategoryByName(p.Category.Name, UserID);
                        if (err != null) 
                        {
                            throw new Exception(err.Message);
                        }
                        newAllPasswords.Add(new Password()
                        {
                            Name = p.Name,
                            Password1 = p.Password1,
                            CategoryId = _category.CategoryId,
                            UserId = UserID
                        });
                    });

                    ctx.Passwords.AddRange(newAllPasswords);
                    ctx.SaveChanges();
                    return null;
                }
            }
            catch (Exception err)
            {
                return err;
            }
        }

        public static Exception AddMultipleCategories(List<Category> AllCategories, int UserID = -1)
        {
            try
            {
                if (UserID == -1)
                {
                    ctx.Categories.AddRange(AllCategories);
                    ctx.SaveChanges();
                    return null;
                }
                else
                {
                    List<Category> newAllCategories = new List<Category>();
                    foreach(var category in AllCategories)
                    {
                        newAllCategories.Add(new Category { Name = category.Name, UserId=UserID });
                    }
                    ctx.Categories.AddRange(newAllCategories);
                    ctx.SaveChanges();
                    return null;
                }
            }
            catch (Exception err)
            {
                return err;
            }
        }

        public static (List<User>, Exception) GetAllUsers()
        {
            try
            {
                return (ctx.Users.ToList(), null);
            }
            catch (Exception err)
            {
                return (new List<User>(), err);
            }
        }

        public static (List<Password>, Exception) GetAllPasswords()
        {
            try
            {
                return (ctx.Passwords.ToList(), null);
            }
            catch (Exception err)
            {
                return (new List<Password>(), err);
            }
        }

        public static (List<Category>, Exception) GetAllCategories()
        {
            try
            {
                return (ctx.Categories.ToList(), null);
            }
            catch (Exception err)
            {
                return (new List<Category>(), err);
            }
        }

        public static (List<Password>, Exception) GetAllPasswordsByUserID(int UserID, string like = null, bool withID = true)
        {
            try
            {
                if (like == null)
                {
                    if (!withID)
                    {
                        List<Password> passwords = ctx.Passwords.Where(p => p.UserId == UserID).ToList();
                        List<Password> data = new List<Password>();
                        passwords.ForEach(p =>
                        {
                            Password newPass = new Password()
                            {
                                Category = p.Category,
                                CategoryId = p.CategoryId,
                                Name = p.Name,
                                Password1 = p.Password1,
                                User = p.User,
                                UserId = p.UserId
                            };
                            data.Add(newPass);
                        });
                        return (data, null);
                    }

                    return (ctx.Passwords.Where(p => p.UserId == UserID).ToList(), null);
                }
                else
                {
                    if (!withID)
                    {
                        List<Password> passwords = ctx.Passwords.Where(p => p.UserId == UserID && p.Name.Contains(like)).ToList();
                        List<Password> data = new List<Password>();
                        passwords.ForEach(p =>
                        {
                            Password newPass = new Password()
                            {
                                Category = p.Category,
                                CategoryId = p.CategoryId,
                                Name = p.Name,
                                Password1 = p.Password1,
                                User = p.User,
                                UserId = p.UserId
                            };
                            data.Add(newPass);
                        });
                        return (data, null);
                    }
                    return (ctx.Passwords.Where(p => p.UserId == UserID && p.Name.Contains(like)).ToList(), null);
                }

            }
            catch (Exception err)
            {
                return (new List<Password>(), err);
            }
        }

        public static (List<Category>, Exception) GetAllCategoriesByUserID(int UserID, string like = null, bool withID = true)
        {
            try
            {
                if (like == null)
                {
                    if (!withID)
                    {

                        List<Category> categories = ctx.Categories.Where(c => c.UserId == UserID).ToList();
                        List<Category> data = new List<Category>();
                        categories.ForEach(c => { 
                            Category newCat = new Category() { 
                                Name = c.Name, 
                                Passwords = c.Passwords, 
                                User = c.User, 
                                UserId = c.UserId 
                            };
                            data.Add(newCat);
                        });
                        return (data, null);
                    }

                    return (ctx.Categories.Where(c => c.UserId == UserID).ToList(), null);
                }
                else
                {
                    if (!withID)
                    {
                        List<Category> categories = ctx.Categories.Where(c => c.UserId == UserID && c.Name.Contains(like)).ToList();
                        List<Category> data = new List<Category>();
                        categories.ForEach(c => {
                            Category newCat = new Category() { 
                                Name = c.Name, 
                                Passwords = c.Passwords, 
                                User = c.User, 
                                UserId = c.UserId 
                            };
                            data.Add(newCat);
                        });
                        return (data, null);
                    }
                    return (ctx.Categories.Where(c => c.UserId == UserID && c.Name.Contains(like)).ToList(), null);
                }
            }
            catch (Exception err)
            {
                return (new List<Category>(), err);
            }
        }

        public static (List<Password>, Exception) GetAllPasswordsByCategoryID(int CategoryID, int UserID, string like = null)
        {
            try
            {
                if (like == null)
                {
                    return (ctx.Passwords.Where(p => p.UserId == UserID && p.CategoryId == CategoryID).ToList(), null);
                }
                else
                {
                    return (ctx.Passwords.Where(p => p.UserId == UserID && p.CategoryId == CategoryID && p.Name.Contains(like)).ToList(), null);
                }

            }
            catch (Exception err)
            {
                return (new List<Password>(), err);
            }
        }

        public static Exception DeletePasswordsByUserID(int UserID)
        {
            try
            {
                ctx.Passwords.RemoveRange(ctx.Passwords.Where(p => p.UserId == UserID).ToList());
                ctx.SaveChanges();
                return null;
            }
            catch(Exception error)
            {
                return error;
            }
        }

        public static Exception DeleteCategoriesByUserID(int UserID)
        {
            try
            {
                ctx.Categories.RemoveRange(ctx.Categories.Where(c => c.UserId == UserID).ToList());
                ctx.SaveChanges();
                return null;
            }
            catch (Exception error)
            {
                return error;
            }
        }

    }
}
