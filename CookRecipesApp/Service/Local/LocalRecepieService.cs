using Android.Net.Wifi.Rtt;
using CookRecipesApp.Model.Category;
using CookRecipesApp.Model.Ingredient;
using CookRecipesApp.Model.Recepie;
using CookRecipesApp.Service.Interface;
using Microsoft.Maui.Graphics.Text;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CookRecipesApp.Service
{

    public class LocalRecepieService : IRecepieService
    {
        private readonly ISQLiteAsyncConnection _database;
        private readonly IIngredientService _ingredientService;
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;

        public LocalRecepieService(SQLiteConnectionFactory factory, IIngredientService ingredientsService, ICategoryService categoryService, IUserService userService)
        {
            _database = factory.CreateConnection();
            _ingredientService = ingredientsService;
            _categoryService = categoryService;
            _userService = userService;
        }

        private async Task<RecepieIngredient> DbToRecepieIngredientAsync(RecepieIngredientDbModel dbModel)
        {
            var ingredient = await _ingredientService.GetIngredientAsync(dbModel.IngredientId);
            var unit = await _database.Table<UnitDbModel>().FirstOrDefaultAsync(u => u.Id == dbModel.UnitId);

            var sui = new Ingredient.IngredientUnitInfo { Unit = unit, ConversionFactor = dbModel.ConversionFactor };

            return new RecepieIngredient
            {
                Id = dbModel.Id,
                RecepieId = dbModel.RecepieId,
                Ingredient = ingredient,
                Quantity = dbModel.Quantity,
                SelectedUnitInfo = sui
            };
        }

        private RecepieIngredientDbModel RecepieIngredientToDbAsync(RecepieIngredient recepie)
        {
            return new RecepieIngredientDbModel
            {
                Id = recepie.Id,
                RecepieId = recepie.RecepieId,
                IngredientId = recepie.Ingredient.Id,
                Quantity = recepie.Quantity,
                UnitId = recepie.SelectedUnitInfo.Unit.Id,
                ConversionFactor = recepie.SelectedUnitInfo.ConversionFactor
            };
        }




        public async Task<List<RecepieIngredient>> GetAllIngredientsForRecepieAsync(int recepieId)
        {

            var dbModels = await _database.Table<RecepieIngredientDbModel>().Where(ri => ri.RecepieId == recepieId).ToListAsync();

            var result = new List<RecepieIngredient>();
            foreach (var model in dbModels)
            {
                result.Add(await DbToRecepieIngredientAsync(model));
            }
            return result;
        }

        public async Task<List<RecepieStep>> GetAllStepsForRecepieAsync(int recepieId)
        {
            var rsDbModels = await _database.Table<RecepieStepDbModel>().Where(rs => recepieId == rs.RecepieId).ToListAsync();
            List<RecepieStep> result = new();

            foreach (var model in rsDbModels)
            {
                result.Add(new RecepieStep
                {
                    Id = model.Id,
                    RecepieId = model.RecepieId,
                    Description = model.Description,
                    StepNumber = model.StepNumber,
                });
            }

            return result;
        }

        public async Task<List<Comment>> GetAllCommentsForRecepieAsync(int recepieId)
        {
            var cDbModels = await _database.Table<CommentDbModel>().Where(c => c.RecepieId == recepieId).ToListAsync();
            List<Comment> result = new();

            foreach (var model in cDbModels)
            {
                var user = await _userService.GetUserByIdAsync(model.UserId);
                result.Add(new Comment
                {
                    Id = model.Id,
                    RecepieId = model.RecepieId,
                    UserId = model.UserId,
                    UserName = user.Name ?? "null",
                    Text = model.Text,
                    Rating = model.Rating,
                    CreatedAt = DateTime.TryParse(model.CreatedAt, out var date) ? date : DateTime.Now,
                });
            }

            return result;

        }

        public async Task<Recepie> RecepieDbModelToRecepieAsync(RecepieDbModel recepieDbModel)
        {
            Recepie recepie = new Recepie()
            {
                Id = recepieDbModel.Id,
                UserId = recepieDbModel.UserId,
                Title = recepieDbModel.Title,
                PhotoPath = recepieDbModel.PhotoPath,
                Steps = await GetAllStepsForRecepieAsync(recepieDbModel.Id),
                CookingTime = recepieDbModel.CookingTime,
                Servings = recepieDbModel.Servings,
                ServingUnit = await _database.Table<UnitDbModel>().FirstOrDefaultAsync(x => x.Id == recepieDbModel.ServingUnitId),
                DifficultyLevel = (DifficultyLevel)recepieDbModel.Difficulty,
                Ingredients = await GetAllIngredientsForRecepieAsync(recepieDbModel.Id),
                Comments = await GetAllCommentsForRecepieAsync(recepieDbModel.Id),
                Categories = await _categoryService.GetRecepieCategoriesAsync(recepieDbModel.Id),

                Calories = recepieDbModel.Calories,
                Proteins = recepieDbModel.Proteins,
                Fats = recepieDbModel.Fats,
                Carbohydrates = recepieDbModel.Carbohydrates,
                Fiber = recepieDbModel.Fiber,

                RecepieCreated = DateTime.TryParse(recepieDbModel.RecepieCreated, out var date) ? date : DateTime.Now,
                Rating = recepieDbModel.Rating,
                UsersRated = recepieDbModel.UsersRated,

            };
            return recepie;
        }

        public RecepieDbModel RecepieToRecepieDbModel(Recepie recepie)
        {
            RecepieDbModel recepieDbModel = new()
            {
                Id = recepie.Id,
                UserId = recepie.UserId,
                Title = recepie.Title,
                PhotoPath = recepie.PhotoPath,
                //steps
                CookingTime = recepie.CookingTime,
                Servings = recepie.Servings,
                ServingUnitId = recepie.ServingUnit.Id,
                Difficulty = (int)recepie.DifficultyLevel,
                //ingredients

                Calories = recepie.Calories,
                Proteins = recepie.Proteins,
                Fats = recepie.Fats,
                Carbohydrates = recepie.Carbohydrates,
                Fiber = recepie.Fiber,

                RecepieCreated = recepie.RecepieCreated.ToString(),
                Rating = recepie.Rating,
                UsersRated = recepie.UsersRated,
                //comments

                //categories
            };

            return recepieDbModel;
        }



        public async Task DeleteRecepieAsync(int id)
        {
            await _database.DeleteAsync<RecepieDbModel>(id);
            await _database.Table<RecepieStepDbModel>().DeleteAsync(x => x.RecepieId == id);
            await _database.Table<RecepieIngredientDbModel>().DeleteAsync(x => x.RecepieId == id);
            await _database.Table<CommentDbModel>().DeleteAsync(x => x.RecepieId == id);
            await _database.Table<RecepieCategoryDbModel>().DeleteAsync(x => x.RecepieId == id);
        }

        public async Task<List<Recepie>> GetRecepiesAsync(int amount)
        {
            var recepieDbList = new List<RecepieDbModel>();
            if (amount == -1)
            {
                recepieDbList = await _database.Table<RecepieDbModel>().ToListAsync();
            }
            else
            {
                recepieDbList = await _database.Table<RecepieDbModel>().Take(amount).ToListAsync();
            }
            List<Recepie> recepieList = new();

            foreach (var recepie in recepieDbList)
            {
                recepieList.Add(await RecepieDbModelToRecepieAsync(recepie));
            }
            return recepieList;

        }

        public async Task<Recepie> GetRecepieAsync(int id)
        {
            var recepieDbModel = await _database.Table<RecepieDbModel>()
                                                .FirstOrDefaultAsync(r => r.Id == id);
            if (recepieDbModel == null) throw new ArgumentNullException("Object not found in database");
            return await RecepieDbModelToRecepieAsync(recepieDbModel);
        }

        public async Task SaveRecepieAsync(Recepie recepie)
        {
            if (recepie == null) throw new ArgumentNullException("Cant save null object to database");

            var recepieDbModel = RecepieToRecepieDbModel(recepie);
            await _database.InsertAsync(recepieDbModel);
            recepie.Id = recepieDbModel.Id;

            await SaveAllRecepieStepsAsync(recepie.Id, recepie.Steps);
            await SaveRecepieIngredientsAsync(recepie.Id, recepie.Ingredients);
            await SaveAllRecepieCommentsAsync(recepie.Id, recepie.Comments);
            await SaveRecepieCategoriesAsync(recepie.Id, recepie.Categories);

            return;
        }

        public async Task UpdateRecepieAsync(Recepie recepie)
        {
            if (recepie == null) throw new ArgumentNullException("Cant update null object to database");
            var recepieDbModel = RecepieToRecepieDbModel(recepie);
            await _database.UpdateAsync(recepieDbModel);

            await SaveAllRecepieStepsAsync(recepie.Id, recepie.Steps);
            await SaveRecepieIngredientsAsync(recepie.Id, recepie.Ingredients);
            await SaveAllRecepieCommentsAsync(recepie.Id, recepie.Comments);
            await SaveRecepieCategoriesAsync(recepie.Id, recepie.Categories);

            return;
        }

        private async Task SaveAllRecepieStepsAsync(int recepieId, List<RecepieStep> steps)
        {
            await _database.Table<RecepieStepDbModel>().DeleteAsync(x => x.RecepieId == recepieId);

            foreach (var rs in steps)
            {
                await _database.InsertAsync(new RecepieStepDbModel
                {
                    RecepieId = recepieId,
                    Description = rs.Description,
                    StepNumber = rs.StepNumber
                });
            }
        }

        private async Task SaveRecepieIngredientsAsync(int recepieId, List<RecepieIngredient> ingredients)
        {
            await _database.Table<RecepieIngredientDbModel>().DeleteAsync(x => x.RecepieId == recepieId);

            var dbList = ingredients.Select(i => new RecepieIngredientDbModel
            {
                RecepieId = recepieId,
                IngredientId = i.Ingredient.Id,
                Quantity = i.Quantity,
                UnitId = i.SelectedUnitInfo.Unit.Id,
                ConversionFactor = i.SelectedUnitInfo.ConversionFactor,
            }).ToList();

            await _database.InsertAllAsync(dbList);
        }

        private async Task SaveAllRecepieCommentsAsync(int recepieId, List<Comment> comments)
        {
            await _database.Table<CommentDbModel>().DeleteAsync(x => x.RecepieId == recepieId);

            var dbList = comments.Select(c => new CommentDbModel
            {
                RecepieId = recepieId,
                UserId = c.UserId,
                Text = c.Text,
                Rating = c.Rating,
                CreatedAt = c.CreatedAt.ToString()
            });

            await _database.InsertAllAsync(dbList);

        }


        private async Task SaveRecepieCategoriesAsync(int recepieId, List<Category> categories)
        {
            await _database.Table<RecepieCategoryDbModel>().DeleteAsync(rc => rc.RecepieId == recepieId);

            var links = categories.Select(c => new RecepieCategoryDbModel
            {
                RecepieId = recepieId,
                CategoryId = c.Id
            }).ToList();

            await _database.InsertAllAsync(links);
        }

        public async Task ChangeFavoriteAsync(int recepieId, int userId)
        {
            var ru = await _database.Table<RecepieUserDbModel>().FirstOrDefaultAsync(u => u.RecepieId == recepieId && u.UserId == userId);
            if (ru == null)
            {
                ru = new RecepieUserDbModel()
                {
                    RecepieId = recepieId,
                    UserId = userId,
                    IsFavorite = true
                };
                await _database.InsertAsync(ru);
            }
            else
            {
                ru.IsFavorite = !ru.IsFavorite;
                await _database.UpdateAsync(ru);
            }
        }

        public async Task<bool> IsFavoriteAsync(int recepieId, int userId)
        {
            var ru = await _database.Table<RecepieUserDbModel>().FirstOrDefaultAsync(u => u.RecepieId == recepieId && u.UserId == userId);
            if (ru == null)
            {
                return false;
            }
            else
            {
                return ru.IsFavorite;
            }
        }

        public async Task<bool> UserCommentedAsync(int recepieId, int userId)
        {
            var comment = await _database.Table<CommentDbModel>().FirstOrDefaultAsync(c => c.RecepieId == recepieId && c.UserId == userId);
            if (comment == null)
            {
                return false;
            }
            return true;

        }

        private async Task<(float, int)> RecalculateRecipeRatingAsync(int recepieId)
        {
            var comments = await _database
                .Table<CommentDbModel>()
                .Where(c => c.RecepieId == recepieId)
                .ToListAsync();

            if (!comments.Any())
            {
                await _database.ExecuteAsync(
                    "UPDATE RecepieDbModel SET Rating = 0, UsersRated = 0 WHERE Id = ?",
                    recepieId
                );
                return(0, 0);
            }

            var avgRating = comments.Average(c => c.Rating);
            var usersRated = comments.Count;

            await _database.ExecuteAsync(
                "UPDATE RecepieDbModel SET Rating = ?, UsersRated = ? WHERE Id = ?",
                (float)Math.Round(avgRating, 2),
                usersRated,
                recepieId
            );
            return ((float)Math.Round(avgRating, 2), usersRated);
        }

        public async Task<(float, int)> PostCommentAsync(Comment comment)
        {
            if (comment == null)
            {
                return(0, 0);
            }

            CommentDbModel model = new()
            {
                RecepieId = comment.RecepieId,
                UserId = comment.UserId,
                Rating = comment.Rating,
                Text = comment.Text,
                CreatedAt = comment.ToString() ?? DateTime.Now.ToString(),
            };

            await _database.InsertAsync(model);
            var rating = await RecalculateRecipeRatingAsync(comment.RecepieId);
            return (rating);
        }

        public async Task<Comment?> GetCommentByUserAndRecepieAsync(int recepieId, int userId)
        {
            var comment = await _database.Table<CommentDbModel>().FirstAsync(c => c.RecepieId == recepieId && c.UserId == userId);
            if(comment == null)
            {
                return null;
            }
            var user = await _userService.GetUserByIdAsync(comment.UserId);
            return new Comment()
            {
                Id = comment.Id,
                RecepieId = comment.RecepieId,
                UserId = comment.UserId,
                UserName = user.Name,
                Rating = comment.Rating,
                Text = comment.Text,
                CreatedAt = DateTime.TryParse(comment.CreatedAt, out var date) ? date : DateTime.Now,
            };
        }

        public async Task<(float, int)> DeleteCommentByUserAndRecepieAsync(int recepieId, int userId)
        {
            var comment = await _database.Table<CommentDbModel>().FirstAsync(c => c.RecepieId == recepieId && c.UserId == userId);

            if(comment == null)
            {
                return (0, 0);
            }

            await _database.DeleteAsync(comment);
            var rating = await RecalculateRecipeRatingAsync(recepieId);
            return(rating);
        }
    }
}
