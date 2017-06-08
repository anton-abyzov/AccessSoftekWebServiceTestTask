## Tasks for basic C# knowledge
1. SQL tables
Let we have a production database on a large web application like Amazon. Let we have two tables:
table Order
  OrderID int,
  ... -- // Other order columns

table OrderItem
  OrderItemID int,
  OrderID int, -- // Points to Order table.
  ... -- // Other order item columns
Let we have Order row with OrderID = 1 and several OrderItem rows with OrderID = 1 we need to change OrderID to 2. How to implement this? You need to provide SQL fragment that solves this task.
2. Foo code
What is the aim of the following code fragment? What are adv/disadv of this solution? Implement better fragment.
```C#
static int Foo( int a, int b, int c )
{
  if ( 1 < c )
    return Foo( b, b + a, c - 1 );
  else
    return a;
}
```
3. Another foo code.
What is the aim of the following fragment? What are adv/disadv of this solution? You need to propose several possible improvements.
```C#
int[] Foo( int[] arr )
{
  for( int i = 0 ; i < arr.Length ; i ++ )
    for ( int j = 0 ; j < arr.Lenght - 1 ; j ++ )
      if ( arr[ j ] > arr[ j + 1 ] )
      {
        int t = arr[ j ];
        arr[ j ] = arr[ j + 1 ];
        arr[ j + 1 ] = t;
      }
  return arr;
}
4. What is wrong in the following code?
You are involved in a new project. This is a web service that implements access to database for a web application like Amazon. Your IT lead asks you to analyze following code fragment. Your goal is to identify as much more issues as you can and compose a list of issues in the following form: brief issue description - how to resolve.
1. There is a division to zero issue. Need to apply check if input parameter is not zero.
2. …
Implement a refactoring and present your version of this fragment.
 

...
```C#
[WebMethod]
public Order LoadOrderInfo( string orderCode )
{
  try
  {
    Debug.Assert( null != orderCode && orderCode != "" );
    Stopwatch stopWatch = new Stopwatch();
    stopWatch.Start();
    lock ( cache )
    {
      if ( cache.ContainsKey( orderCode ) )
      {
        stopWatch.Stop();
        logger.Log( "INFO",
          "Elapsed - {0}", stopWatch.Elapsed );
        return cache[ orderCode ];
      }
    }
    string queryTemplate =
      "SELECT OrderID, CustomerID, TotalMoney" +
      "  FROM dbo.Orders where OrderCode='{0}'";
    string query = string.Format( queryTemplate, orderCode );
    SqlConnection connection =
      new SqlConnection( this.connectionString );
    SqlCommand command =
      new SqlCommand( query, connection );
    connection.Open();
    SqlDataReader reader = command.ExecuteReader();
    if ( reader.Read() )
    {
      Order order = new Order(
        ( string ) reader[ 0 ],
        ( string ) reader[ 1 ],
        ( int ) reader[ 2 ] );
      lock ( cache )
      {
        if ( !cache.ContainsKey( orderCode ) )
          cache[ orderCode ] = order;
      }
      stopWatch.Stop();
      logger.Log( "INFO", "Elapsed - {0}", stopWatch.Elapsed );
      return order;
    }
    stopWatch.Stop();
    logger.Log( "INFO", "Elapsed - {0}", stopWatch.Elapsed );
    return null;
  }
  catch ( SqlException ex )
  {
    logger.Log( "ERROR", ex.Message );
    throw new ApplicationException( "Error" );
  }
}
public IDictionary<string, Order> cache;
...
