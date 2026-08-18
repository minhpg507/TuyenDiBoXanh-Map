using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

app.UseCors();

app.MapGet("/api/points", () =>
{
    var points = new List<object>();

    string connectionString = @"Server=MINHMINH\MINH;Database=master;Trusted_Connection=True;TrustServerCertificate=True;";

    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        conn.Open();
        SqlCommand cmd = new SqlCommand("SELECT PointID, Name, Latitude, Longitude, EnglishGuide FROM GreenRoutePoints", conn);
        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            points.Add(new
            {
                Id = reader["PointID"],
                Name = reader["Name"].ToString(),
                Lat = Convert.ToDouble(reader["Latitude"]),
                Lng = Convert.ToDouble(reader["Longitude"]),
                Guide = reader["EnglishGuide"].ToString()
            });
        }
    }
    return points;
});

app.Run();