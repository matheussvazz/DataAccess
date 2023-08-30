using System;
using Dapper;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using BaltaDataAcess.Models;


namespace BaltaDataAcess
{
    class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=localhost,1433;Database=balta;User ID=sa;Password=1q2w3e4r@#$";


            using (var connection = new SqlConnection(connectionString))
            {

                //CreateCategory(connection);
                //CreateManyCategory(connection);
                // UpdateCategory(connection);
                // ListCategories(connection);
                //ExecuteProcedure(connection);
                // ExecuteReadProcedure(connection);
                //ExecuteScalar(connection);
                // ReadView(connection);
                //OneToOne(connection);
                // SelectIn(connection);
                // Like(connection);
                //Transaction(connection);

            }

        }
        static void ListCategories(SqlConnection connection)
        {
            var categories = connection.Query<Category>("SELECT Id, Title FROM Category");
            foreach (var item in categories)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
            }

        }
        static void CreateCategory(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var insertSql = @"INSERT INTO 
                    CATEGORY VALUES(
                    @Id(), 
                    @Title, 
                    @Url, 
                    @Summary, 
                    @Order, 
                    @Description, 
                    @Featured)";

            var rows = connection.Execute(insertSql, new
            {
                category.Id,
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
            });
            Console.WriteLine($"{rows} Linhas inseridas");

        }
        static void UpdateCategory(SqlConnection connection)
        {
            var updateQuery = "UPDATE Category SET Title=@title WHERE Id =@id";
            var rows = connection.Execute(updateQuery, new
            {
                id = new Guid("af3407aa-11ae-4621-a2ef-2028b85507c4"),
                title = "Frontend 2021"
            });

            Console.WriteLine($"{rows} registros atualizados");
        }
        static void CreateManyCategory(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;


            var category2 = new Category();
            category2.Id = Guid.NewGuid();
            category2.Title = "Categoria Nova";
            category2.Url = "categoria-nova";
            category2.Description = "Categoria nova";
            category2.Order = 8;
            category2.Summary = "Categoria";
            category2.Featured = false;

            var insertSql = @"INSERT INTO 
                    CATEGORY VALUES(
                    @Id(), 
                    @Title, 
                    @Url, 
                    @Summary, 
                    @Order, 
                    @Description, 
                    @Featured)";

            var rows = connection.Execute(insertSql, new[]
                  new
                {

                    category.Id,
                    category.Title,
                    category.Url,
                    category.Summary,
                    category.Order,
                    category.Description,
                    category.Featured

                },
                  new
                {

                    category2.Id,
                    category2.Title,
                    category2.Url,
                    category2.Summary,
                    category2.Order,
                    category2.Description,
                    category2.Featured

                }
                });
            Console.WriteLine($"{rows} Linhas inseridas");

        }
        static void ExecuteProcedure(SqlConnection connection)


        {
            var procedure = "spDeleteStudent";
            var pars = new { StudentId = "2bfdef40-1b2e-41f3-9429-94496ed0a0e4" };
            var affectedRows = connection.Execute(
              procedure,
              pars,
              commandType: System.Data.CommandType.StoredProcedure);
            System.Console.WriteLine($"{affectedRows} Linhas afetadas");
        }
        static void ExecuteReadProcedure(SqlConnection connection)

        {
            var procedure = "spGetCoursesByCategory";
            var pars = new { CategoryId = "2bfdef40-1b2e-41f3-9429-94496ed0a0e4" };
            var affectedRows = connection.Execute(
              procedure,
              pars,
              commandType: System.Data.CommandType.StoredProcedure);
            System.Console.WriteLine($"{affectedRows} Linhas afetadas");
        }
        static void ExecuteScalar(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var insertSql = @"INSERT INTO 
                    CATEGORY 
                VALUES(
                    @Id(), 
                    @Title, 
                    @Url, 
                    @Summary, 
                    @Order, 
                    @Description, 
                    @Featured) 
                SELECT SCOPE_IDENTITY()";

            var id = connection.ExecuteScalar<Guid>(insertSql, new
            {
                category.Title,
                category.Url,
                category.Summary,
                category.Order,
                category.Description,
                category.Featured
            });
            Console.WriteLine($"A categoria inserida foi: {id}");
        }

        static void ReadView(SqlConnection connection)

        {
            var sql = "SELECT * FROM vwCourses";
            var courses = connection.Query(sql);
            foreach (var item in courses)
            {
                Console.WriteLine($"{item.Id} - {item.Title}");
            }
        }

        static void OneToOne(SqlConnection connection)

        {
            var sql = @"
              SELECT 
                 * 
              FROM 
                CareerItem
              INNER JOIN
                 Course ON CareerItem.CourseId = Course.Id";

            var items = connection.Query<CareerItem, Course, CareerItem>
            (
            sql, (careerItem, course) =>
            {
                careerItem.Course = course;
                return careerItem;
            }, splitOn: "Id");

            foreach (var item in items)
            {
                Console.WriteLine($"{item.Title} - Curso: {item.Course.Title}");
            }


        }
        static void OneToMany(SqlConnection connection)
        {
            var sql = @"
                 SELECT 
                    Career.Id,
                    Career.Title,
                    CareerItem.CareerId,
                    CareerItem.Title
                FROM
                    Career
                INNER JOIN 
                    CareerItem ON CareerItem.CareerId = Career.Id
                ORDER BY 
                 Career.Title";

            var carres = new List<Career>();
            var items = connection.Query<Career, CareerItem, Career>
            (
            sql,
            (career, item) =>
            {

                return career;
            }, splitOn: "CareerId");

            foreach (var career in careers)
            {
                System.Console.WriteLine($"{career.Title}");
                foreach (var item in career.Items)
                {
                    Console.WriteLine($" - {item.Title}");
                }
            }


        }

        static void QueryMultiple(SqlConnection connection)
        {
            var query = "SELECT * FROM CATEGORY; SELECT * FROM COURSE";

            using (var multi = connection.QueryMultiple(query))
            {
                var categories = multi.Read<Category>;
                var courses = multi.Read<Course>;
            }

            foreach (var item in courses)
            {
                Console.WriteLine(item.Title);
            }
        }

        static void SelectIn(SqlConnection connection)
        {
            var query = @"SELECT * FROM  Career WHERE Id ";

            var items = connection.Query<Career>(query, new
            {
                Id = new[]{
                   "4327ac7e-963b-4893-9f31-9a3b28a4e72b",
                   "e6730d1c-6870-4df3-ae68-438624e04c72"
                    }
            });

        }

        static void Like(SqlConnection connection, string term)
        {
            var query = @"SELECT * FROM  Course WHERE Title LIKE @exp ";

            var items = connection.Query<Course>(query, new
            {
                exp = $"%{term}%"

            });

            foreach (var item in items)
            {
                Console.WriteLine(item.Title);
            }

        }

        static void Transaction(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon AWS";
            category.Url = "amazon";
            category.Description = "Categoria destinada a serviços do AWS";
            category.Order = 8;
            category.Summary = "AWS Cloud";
            category.Featured = false;

            var insertSql = @"INSERT INTO 
                    CATEGORY VALUES(
                    @Id(), 
                    @Title, 
                    @Url, 
                    @Summary, 
                    @Order, 
                    @Description, 
                    @Featured)";

            using (var transaction = connection.BeginTransaction())
            {
                var rows = connection.Execute(insertSql, new
                {
                    category.Id,
                    category.Title,
                    category.Url,
                    category.Summary,
                    category.Order,
                    category.Description,
                    category.Featured
                }, transaction);

                transaction.Commit();
                transaction.Rollback();

                Console.WriteLine($"{rows} Linhas inseridas");
            }

        }
    }
}
