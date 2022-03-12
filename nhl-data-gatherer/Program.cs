using nhl_data_builder.DataGetter;
using nhl_data_builder.Entities;

var gameParser = new GameParser();
var requestMaker = new RequestMaker();
var dataGetter = new DataGetter(gameParser, requestMaker);
await dataGetter.GetData();
