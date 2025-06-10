using Pos.Common.MongoDB;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using Pos.Transaction.Service.Entities;
using Pos.Transaction.Service.Clients;
using Pos.Transaction.Service;

var builder = WebApplication.CreateBuilder(args);
BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
// Add services to the container.
builder.Services.AddControllers(
    options => options.SuppressAsyncSuffixInActionNames = false
);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
  builder.Services.AddLogging();

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddMongo().AddMongoRepository<Sales>("sales");
builder.Services.AddMongo().AddMongoRepository<SaleItems>("saleItems");

builder.Services.AddTransient<CustomerClient>(provider =>
    new CustomerClient("https://localhost:7184"));

builder.Services.AddHttpClient<ProductClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7116");
});

// builder.Services.AddTransient<ProductClient>(provider =>
//     new ProductClient("https://localhost:7116"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
