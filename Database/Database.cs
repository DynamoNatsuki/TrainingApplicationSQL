using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class Database
    {
        private readonly SqlConnection connection;

        public Database(string connectionString)
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        public List<Session> GetAllSessions()
        {
            string query = "SELECT * FROM [TrainingDiariesDB].[dbo].[Training]";

            SqlCommand cmd = new SqlCommand(query, connection);

            var reader = cmd.ExecuteReader();

            var sessions = new List<Session>();

            while (reader.Read())
            {
                int id = int.Parse(reader["Id"].ToString());
                string exerciseName = reader["ExerciseName"].ToString();
                int sets = int.Parse(reader["Sets"].ToString());
                int reps = int.Parse(reader["Reps"].ToString());
                int weight = int.Parse(reader["Weight"].ToString());

                sessions.Add(new Session()
                {
                    Id = id,
                    ExerciseName = exerciseName,
                    Sets = sets,
                    Reps = reps,
                    Weight = weight
                });
            }

            return sessions;
        }

        public void CreateSession(string exerciseName, int sets, int reps, int weight)
        {
            string query = "INSERT INTO Training (ExerciseName, Sets, Reps, Weight) VALUES (@ExerciseName, @Sets, @Reps, @Weight)";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@ExerciseName", exerciseName);
                cmd.Parameters.AddWithValue("@Sets", sets);
                cmd.Parameters.AddWithValue("@Reps", reps);
                cmd.Parameters.AddWithValue("@Weight", weight);

                cmd.ExecuteNonQuery();
            }
        }

        public Session GetSessionById(int id)
        {
            // Corrected SQL query to use the @Id parameter
            string query = "SELECT [Id], [ExerciseName], [Sets], [Reps], [Weight] FROM [TrainingDiariesDB].[dbo].[Training] WHERE [Id] = @Id";


            // Ensure the SqlCommand is created with the corrected query and the open connection
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                // Add the id parameter to the command
                cmd.Parameters.AddWithValue("@Id", id);

                // Execute the query and use SqlDataReader to read the results
                using (var reader = cmd.ExecuteReader())
                {
                    // Check if there is at least one record
                    if (reader.Read())
                    {
                        // Parse the data from the reader and create a Session object
                        int sessionId = int.Parse(reader["Id"].ToString());
                        string exerciseName = reader["ExerciseName"].ToString();
                        int sets = int.Parse(reader["Sets"].ToString());
                        int reps = int.Parse(reader["Reps"].ToString());
                        int weight = int.Parse(reader["Weight"].ToString());

                        return new Session
                        {
                            Id = sessionId, // Include Id if needed
                            ExerciseName = exerciseName,
                            Sets = sets,
                            Reps = reps,
                            Weight = weight
                        };
                    }
                    else
                    {
                        // Return null if no record is found
                        return null;
                    }
                }
            }
        }

        public void EditSessionById(int id, string exerciseName, int sets, int reps, int weight)
        {
            string query = "UPDATE Training SET ExerciseName = @ExerciseName, Sets = @Sets, Reps = @Reps, Weight = @Weight WHERE Id = @Id";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@ExerciseName", exerciseName);
                cmd.Parameters.AddWithValue("@Sets", sets);
                cmd.Parameters.AddWithValue("@Reps", reps);
                cmd.Parameters.AddWithValue("@Weight", weight);

                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteSessionById(int id)
        {
            string query = "DELETE FROM [TrainingDiariesDB].[dbo].[Training] WHERE [Id] = @Id";

            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
