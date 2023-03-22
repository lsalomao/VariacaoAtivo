var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Default", config =>
    {
        config
        .SetIsOriginAllowed(_ => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Default");

app.UseHttpsRedirection();

app.MapGet("/valoresativo", async (string ativo) =>
{
    using var client = new HttpClient();

    var result = await client.GetFromJsonAsync<object>($"https://query2.finance.yahoo.com/v8/finance/chart/{ativo}.SA?interval=1d&range=30d");

    return result is not null ? Results.Ok(result) : Results.NotFound();
})
.WithName("ObterValoresAtivo");

app.Run();
